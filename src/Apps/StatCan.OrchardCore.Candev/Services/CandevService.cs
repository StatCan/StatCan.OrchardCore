using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using StatCan.OrchardCore.Candev.Indexes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json.Linq;
using OrchardCore.ContentManagement;
using OrchardCore.Queries;
using YesSql;
using OrchardCore.Settings;
using OrchardCore.Entities;
using OrchardCore.Users.Models;
using OrchardCore.Users;
using Microsoft.AspNetCore.Identity;

namespace StatCan.OrchardCore.Candev.Services
{
    public class CandevService : ICandevService
    {
        private readonly YesSql.ISession _session;
        private readonly IContentManager _contentManager;
        private readonly IQueryManager _queryManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISiteService _siteService;
        private readonly UserManager<IUser> _userManager;

        public CandevService(YesSql.ISession session,
            IStringLocalizer<CandevService> localizer,
            IContentManager contentManager,
            IQueryManager queryManager,
            IHttpContextAccessor httpContextAccessor,
            ISiteService siteService,
            UserManager<IUser> userManager
        )
        {
            _session = session;
            _contentManager = contentManager;
            _queryManager = queryManager;
            _httpContextAccessor = httpContextAccessor;
            T = localizer;
            _siteService = siteService;
            _userManager = userManager;
        }

        public IStringLocalizer T { get; }

        public Task<User> GetParticipantAsync()
        {
            var user = _httpContextAccessor.HttpContext.User;
            if (!user.Identity.IsAuthenticated)
            {
                return Task.FromResult<User>(null);
            }
            return _session.Query<User, CandevUsersIndex>(x => x.UserId == user.FindFirst(ClaimTypes.NameIdentifier).Value).FirstOrDefaultAsync();
        }

        public Task<int> GetTeamMemberCount(string teamContentItemId)
        {
            return _session.QueryIndex<CandevUsersIndex>(x => x.TeamContentItemId == teamContentItemId).CountAsync();
        }

        public Task<IEnumerable<User>> GetTeamMembers(string teamContentItemId)
        {
            return _session.Query<User, CandevUsersIndex>(x => x.TeamContentItemId == teamContentItemId).ListAsync();
        }

        public async Task<bool> TeamExists(string teamContentItemId)
        {
            var teamCount = await _session.QueryIndex<CandevItemsIndex>(x => x.ContentItemId == teamContentItemId && x.ContentType == "Team" && x.Published).CountAsync();
            return teamCount > 0;
        }

        public async Task<bool> IsTeamFull(string teamContentItemId)
        {
            var site = await _siteService.GetSiteSettingsAsync();
            var hackathonCustomSettings = site.As<ContentItem>("HackathonCustomSettings");

            var members = await _session.QueryIndex<CandevUsersIndex>(x => x.TeamContentItemId == teamContentItemId).ListAsync();
            if (members.Any())
            {
                if (members.Count() >= (int)hackathonCustomSettings.Content["TeamCustomSettings"]["TeamSize"].Value.Value)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<ContentItem> JoinTeam(string teamContentItemId, ModelStateDictionary modelState)
        {
            var user = await GetParticipantAsync();

            if (user == null || !user.RoleNames.Contains("Hacker"))
            {
                modelState.AddModelError("error", T["You are not a hacker"].Value);
                return null;
            }

            if (user.HasTeam())
            {
                modelState.AddModelError("error", T["Already have a team"].Value);
                return null;
            }

            var team = await _contentManager.GetAsync(teamContentItemId);
            if (team == null)
            {
                modelState.AddModelError("error", T["Team does not exist"].Value);
                return null;
            }

            if (await IsTeamFull(teamContentItemId))
            {
                modelState.AddModelError("error", T["Team is full"]);
                return null;
            }

            var contentItem = await GetSettings(user, "Hacker");

            contentItem.Content.Hacker.Team = JObject.FromObject(new { ContentItemIds = new string[] { team.ContentItemId } });
            user.Properties["Hacker"] = JObject.FromObject(contentItem);
            _session.Save(user);

            return team;
        }

        public async Task<ContentItem> CreateTeam(ModelStateDictionary modelState)
        {
            var user = await GetParticipantAsync();
            if (user == null || !user.RoleNames.Contains("Hacker"))
            {
                modelState.AddModelError("error", T["You are not a hacker"].Value);
                return null;
            }

            if (user.HasTeam())
            {
                modelState.AddModelError("error", T["Already on a team"].Value);
                return null;
            }

            var team = await _contentManager.NewAsync("Team");
            team.Owner = user.UserId;

            await _contentManager.CreateAsync(team, VersionOptions.Published);
            team.Content.Team.TeamCaptain = JObject.FromObject(new { UserIds = new string[] { user.UserId } });
            await _contentManager.UpdateAsync(team);

            var contentItem = await GetSettings(user, "Hacker");
            contentItem.Content.Hacker.Team = JObject.FromObject(new { ContentItemIds = new string[] { team.ContentItemId } });
            user.Properties["Hacker"] = JObject.FromObject(contentItem);
            _session.Save(user);

            return team;
        }

        public async Task<bool> LeaveTeam(ModelStateDictionary modelState)
        {
            var user = await GetParticipantAsync();
            var team = await _session.Query<ContentItem, CandevItemsIndex>(x => x.ContentItemId == user.GetTeamId() && x.ContentType == "Team" && x.Published).FirstOrDefaultAsync();
            var teamContentItemId = user.GetTeamId();

            if (user == null || !user.RoleNames.Contains("Hacker"))
            {
                modelState.AddModelError("error", T["You are not a hacker"].Value);
                return false;
            }

            if (!user.HasTeam())
            {
                modelState.AddModelError("error", T["You are not part of a team"].Value);
                return false;
            }

            //Remove user from team
            var contentItem = await GetSettings(user, "Hacker");
            contentItem.Content.Hacker.Team?.ContentItemIds.Clear();
            user.Properties["Hacker"] = JObject.FromObject(contentItem);
            _session.Save(user);

            //if the team has no member left, delete it
            if (await GetTeamMemberCount(teamContentItemId) > 0)
            {
                //if the user was the team captain, make another hacker the team captain
                if (team.Content.Team?.TeamCaptain?.UserIds?.First == user.UserId)
                {
                    var hacker = await _session.Query<User, CandevUsersIndex>(x => x.TeamContentItemId == teamContentItemId).FirstOrDefaultAsync();
                    team.Content.Team.TeamCaptain = JObject.FromObject(new { UserIds = new string[] { hacker.UserId } });
                    await _contentManager.UpdateAsync(team);
                }
            }
            else
            {
                await _contentManager.RemoveAsync(team);
            }

            return true;
        }

        public string RootUrl(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
            {
                return "/";
            }
            if (!slug.StartsWith("/"))
            {
                return "/" + slug;
            }
            return slug;
        }

        // TODO: move into utils
        private string randomName()
        {
            string[] prefix = new string[10] { "awesome", "great", "insane", "matrix", "elite", "excellent", "amazing", "virtual", "crazy", "super" };
            string[] sufix = new string[7] { "hackers", "coders", "cyberpunks", "programmers", "engineers", "scripters", "architects" };

            Random rand = new Random();
            return prefix[rand.Next(0, prefix.Length)] + "-" + sufix[rand.Next(0, sufix.Length)] + "-" + rand.Next(0, 9);
        }

        public async Task<bool> MatchTeams()
        {
            var site = await _siteService.GetSiteSettingsAsync();
            var hackersWithoutTeams = (await _session.Query<User, CandevUsersIndex>(x => (x.TeamContentItemId == null || x.TeamContentItemId == "") && x.Roles.Contains("Hacker")).ListAsync()).ToList();

            var hackathonCustomSettings = site.As<ContentItem>("HackathonCustomSettings");
            int maxTeamSize = (int)hackathonCustomSettings.Content["TeamCustomSettings"]["TeamSize"].Value.Value;
            await CleanupTeams();
            var unorderedTeams = await _session.QueryIndex<CandevItemsIndex>(x => x.ContentType == "Team" && x.Published).ListAsync();
            var teams = unorderedTeams.OrderByDescending(x => (GetTeamMemberCount(x.ContentItemId).GetAwaiter().GetResult())).ToList();

            var incompleteTeams = Array.Empty<CandevItemsIndex>().ToList();
            int smallestTeamCount = (teams.Count > 0) ? await GetTeamMemberCount(teams.Last().ContentItemId) : 0;
            while (teams.Count > 0)
            {
                var team = teams[0];
                int memberCount = await GetTeamMemberCount(team.ContentItemId);

                // If the team is empty, just remove it from the list.
                if (memberCount != 0)
                {
                    // Combine teams that will form a full team together
                    while (memberCount + smallestTeamCount <= maxTeamSize && team.ContentItemId != teams.Last().ContentItemId)
                    {
                        await CombineTeams(team.ContentItemId, teams.Last());
                        teams.RemoveAt(teams.Count - 1);
                        memberCount += smallestTeamCount;
                        smallestTeamCount = await GetTeamMemberCount(teams.Last().ContentItemId);
                    }

                    // Move teams that are not full to a new list of teams to fill with hackers without teams.
                    if (memberCount < maxTeamSize)
                    {
                        incompleteTeams.Add(team);
                    }
                }
                // Remove the team from the list.
                teams.RemoveAt(0);
            }

            // Add hackers without teams to incomplete teams
            while (incompleteTeams.Count > 0 && hackersWithoutTeams.Count > 0)
            {
                var team = incompleteTeams[0];
                int memberCount = await GetTeamMemberCount(team.ContentItemId);
                while (memberCount < maxTeamSize && hackersWithoutTeams.Count > 0)
                {
                    await AddHackerToTeam(team.ContentItemId, hackersWithoutTeams[0].UserId);
                    hackersWithoutTeams.RemoveAt(0);
                    memberCount++;
                }
                if (memberCount >= maxTeamSize)
                {
                    incompleteTeams.RemoveAt(0);
                }
            }

            // Create teams with the remaining hackers.
            int n = 0;
            string teamId = null;
            while (hackersWithoutTeams.Count > 0)
            {
                var hacker = hackersWithoutTeams[0];
                if (n == 0)
                {
                    teamId = await CreateTeam(hacker.UserId);
                    await AddHackerToTeam(teamId, hacker.UserId);
                    n++;
                    hackersWithoutTeams.RemoveAt(0);
                }
                else
                {
                    await AddHackerToTeam(teamId, hacker.UserId);
                    n++;
                    hackersWithoutTeams.RemoveAt(0);
                }
                if (n == maxTeamSize)
                {
                    teamId = null;
                    n = 0;
                }
            }
            // Delete remaining empty teams
            await CleanupTeams();
            return true;
        }

        private async Task<bool> CombineTeams(string teamContentId1, CandevItemsIndex team2)
        {
            var team2Members = await GetTeamMembers(team2.ContentItemId);
            var team1 = JObject.FromObject(new { ContentItemIds = new string[] { teamContentId1 } });
            foreach (var member in team2Members)
            {
                var contentItem = await GetSettings(member, "Hacker");

                contentItem.Content.Hacker.Team.ContentItemIds.Add(teamContentId1);
                member.Properties["Hacker"] = JObject.FromObject(contentItem);
                _session.Save(member);
            }
            // Delete team 2
            await _contentManager.RemoveAsync(await _contentManager.GetAsync(team2.ContentItemId));
            return true;
        }

        private async Task<bool> AddHackerToTeam(string teamContentId, string participantContentId)
        {
            var participant = await _session.Query<User, CandevUsersIndex>(x => x.UserId == participantContentId).FirstOrDefaultAsync();

            var contentItem = await GetSettings(participant, "Hacker");

            contentItem.Content.Hacker.Team = JObject.FromObject(new { ContentItemIds = new string[] { teamContentId } });
            participant.Properties["Hacker"] = JObject.FromObject(contentItem);
            _session.Save(participant);

            return true;
        }

        private async Task<string> CreateTeam(string UserId)
        {
            var team = await _contentManager.NewAsync("Team");
            team.DisplayText = randomName();
            await _contentManager.CreateAsync(team, VersionOptions.Published);
            team.Content.Team.TeamCaptain = JObject.FromObject(new { UserIds = new string[] { UserId } });
            await _contentManager.UpdateAsync(team);
            return team.ContentItemId;
        }

        public async Task<string> CreateTeam(string teamName, string[] topics)
        {
            var team = await _contentManager.NewAsync("Team");

            team.Content.Team.Name = JObject.FromObject(new { Text = teamName });
            team.Content.Team.Topics = JObject.FromObject(new { ContentItemIds = topics });

            await _contentManager.CreateAsync(team, VersionOptions.Published);
            await _contentManager.UpdateAsync(team);
            return team.ContentItemId;
        }

        private async Task<bool> CleanupTeams()
        {
            foreach (var team in await _session.Query<ContentItem, CandevItemsIndex>(x => x.ContentType == "Team").ListAsync())
            {
                var count = await GetTeamMemberCount(team.ContentItemId);
                if (count == 0)
                {
                    await _contentManager.RemoveAsync(team);
                }
            }
            return true;
        }

        public async Task<bool> RunUpdateHandlers()
        {
            var allItems = await _session.Query<ContentItem, CandevItemsIndex>(x => x.Published).ListAsync();
            foreach (var item in allItems)
            {
                await _contentManager.UpdateAsync(item);
            }
            return true;
        }

        public async Task<ContentItem> GetSettings(User user, string type)
        {
            ContentItem contentItem;
            if (user.Properties.TryGetValue(type, out var properties))
            {
                var existing = properties.ToObject<ContentItem>();
                contentItem = await _contentManager.NewAsync(type);
                contentItem.Merge(existing);
            }
            else
            {
                contentItem = await _contentManager.NewAsync(type);
            }

            return contentItem;
        }

        public async Task<bool> RemoveTeamMember(string hackerContentItemId, ModelStateDictionary modelState)
        {
            var participant = await _session.Query<User, CandevUsersIndex>(x => x.UserId == hackerContentItemId).FirstOrDefaultAsync();
            var user = await GetParticipantAsync();
            var team = await _session.Query<ContentItem, CandevItemsIndex>(x => x.ContentItemId == participant.GetTeamId() && x.ContentType == "Team" && x.Published).FirstOrDefaultAsync();

            if (!participant.HasTeam())
            {
                modelState.AddModelError("error", T["You are not part of a team"].Value);
                return false;
            }

            if (participant == null || team.Content.Team.TeamCaptain?.UserIds?.First != user.UserId)
            {
                modelState.AddModelError("error", T["You are not the team captain. Only the team captain can perform this action"].Value);
                return false;
            }

            var contentItem = await GetSettings(participant, "Hacker");

            contentItem.Content.Hacker.Team.ContentItemIds.Clear();
            participant.Properties["Hacker"] = JObject.FromObject(contentItem);
            _session.Save(participant);

            return true;
        }

        public async Task<bool> SaveTeam(string teamName, string teamDescription, string challenge, ModelStateDictionary modelState)
        {
            var user = await GetParticipantAsync();

            var team = await _session.Query<ContentItem, CandevItemsIndex>(x => x.ContentItemId == user.GetTeamId() && x.ContentType == "Team" && x.Published).FirstOrDefaultAsync();
            if (team == null || team.ContentType != "Team")
            {
                modelState.AddModelError("error", T["Team doesn't exist"].Value);
                return false;
            }

            if (!user.HasTeam())
            {
                modelState.AddModelError("error", T["You are not part of a team"].Value);
                return false;
            }

            if (user == null || team.Content.Team.TeamCaptain?.UserIds?.First != user.UserId)
            {
                modelState.AddModelError("error", T["You are not the team captain. Only the team captain can perform this action"].Value);
                return false;
            }
            team.Content.Team.Name = JObject.FromObject(new { Text = teamName });
            team.Content.Team.Description = JObject.FromObject(new { Text = teamDescription });
            team.Content.Team.Challenge = JObject.FromObject(new { ContentItemIds = new string[] { challenge } });
            await _contentManager.UpdateAsync(team);

            return true;
        }

        public async Task<bool> AssignCases()
        {
            var site = await _siteService.GetSiteSettingsAsync();
            var teams = await _session.Query<ContentItem, CandevItemsIndex>(x => x.ContentType == "Team" && x.Published).ListAsync();
            var topics = await _session.Query<ContentItem, CandevItemsIndex>(x => x.ContentType == "Topic" && x.Published).ListAsync();
            double maxCases = Math.Ceiling(Convert.ToDouble(teams.Count()) / Convert.ToDouble(topics.Count()));

            IDictionary<string, int> casesCount = new Dictionary<string, int>();
            foreach (var topic in topics)
            {
                casesCount.Add(topic.ContentItemId, 0);
            }

            foreach (var team in teams)
            {
                if (team.Content.Team.Topics.ContentItemIds.Count == 0)
                {
                    foreach (var topic in topics)
                    {
                        if (casesCount.Where(x => x.Key == topic.ContentItemId).Select(x => x.Value).FirstOrDefault() < maxCases)
                        {
                            var challenge = topics.Where(x => x.ContentItemId == topic.ContentItemId.ToString()).Select(x => x.Content.Topic.Challenge).FirstOrDefault();
                            team.Content.Team.Challenge = challenge;
                            await _contentManager.UpdateAsync(team);
                            casesCount[topic.ContentItemId.ToString()]++;
                            break;
                        }
                    }
                }
                else
                {
                    foreach (var topic in team.Content.Team.Topics.ContentItemIds)
                    {
                        if (casesCount.Where(x => x.Key == topic.ToString()).Select(x => x.Value).FirstOrDefault() < maxCases)
                        {
                            var challenge = topics.Where(x => x.ContentItemId == topic.ToString()).Select(x => x.Content.Topic.Challenge).FirstOrDefault();
                            team.Content.Team.Challenge = challenge;
                            await _contentManager.UpdateAsync(team);
                            casesCount[topic.ToString()]++;
                            break;
                        }
                    }
                }
            }

            return true;
        }

        public async Task<string> CreateChallenge(string challengeTitle)
        {
            var challenge = await _contentManager.NewAsync("Challenge");

            challenge.Content.Challenge.Title = JObject.FromObject(new { Text = challengeTitle });

            await _contentManager.CreateAsync(challenge, VersionOptions.Published);
            await _contentManager.UpdateAsync(challenge);
            return challenge.ContentItemId;
        }

        public async Task<string> CreateTopic(string topicName, string challengeId)
        {
            var topic = await _contentManager.NewAsync("Topic");

            topic.Content.Topic.Name = JObject.FromObject(new { Text = topicName });
            topic.Content.Topic.Challenge = JObject.FromObject(new { ContentItemIds = new string[] { challengeId } });

            await _contentManager.CreateAsync(topic, VersionOptions.Published);
            await _contentManager.UpdateAsync(topic);
            return topic.ContentItemId;
        }

        public async Task<ContentItem> JoinTeam(string teamContentItemId, string userName)
        {
            var user = await _session.Query<User, CandevUsersIndex>(x => x.UserName == userName).FirstOrDefaultAsync();

            if (user == null || !user.RoleNames.Contains("Hacker"))
            {
                return null;
            }

            if (user.HasTeam())
            {
                return null;
            }

            var team = await _contentManager.GetAsync(teamContentItemId);
            if (team == null)
            {
                return null;
            }

            if (await IsTeamFull(teamContentItemId))
            {
                return null;
            }

            var contentItem = await GetSettings(user, "Hacker");

            contentItem.Content.Hacker.Team = JObject.FromObject(new { ContentItemIds = new string[] { team.ContentItemId } });
            user.Properties["Hacker"] = JObject.FromObject(contentItem);
            _session.Save(user);

            return team;
        }

        public async Task<bool> SelectNHackers(int n)
        {
            var users = await _session.Query<User, CandevUsersIndex>(x => x.Roles.Contains("Hacker")).ListAsync();
            var participants = await _session.QueryIndex<CandevUsersIndex>(x => x.Roles.Contains("Hacker")).ListAsync();
            var attendingParticipantList = participants.Where(x => x.WillAttend).ToList();
            var notAttendingParticipantList = participants.Where(x => !x.WillAttend).ToList();

            foreach (var participant in notAttendingParticipantList)
            {
                var user = users.Where(x => x.UserId == participant.UserId).FirstOrDefault();
                if (user.HasTeam())
                {
                    RemoveFromTeam(user);
                }
                _userManager.RemoveFromRoleAsync(user, "Hacker").GetAwaiter();
            }

            if (n < attendingParticipantList.Count)
            {
                attendingParticipantList.RemoveRange(0, n);

                foreach (var participant in attendingParticipantList)
                {
                    var user = users.Where(x => x.UserId == participant.UserId).FirstOrDefault();
                    if (user.HasTeam())
                    {
                        RemoveFromTeam(user);
                    }
                    _userManager.RemoveFromRoleAsync(user, "Hacker").GetAwaiter();
                }
            }

            return true;
        }

        public async Task<bool> CheckIn()
        {
            var users = await _session.Query<User, CandevUsersIndex>(x => x.Roles.Contains("Hacker") && !x.CheckIn).ListAsync();
            var participants = await _session.QueryIndex<CandevUsersIndex>(x => x.Roles.Contains("Hacker") && !x.CheckIn).ListAsync();

            foreach (var participant in participants)
            {
                var user = users.Where(x => x.UserId == participant.UserId).FirstOrDefault();
                if (user.HasTeam())
                {
                    RemoveFromTeam(user);
                }
                _userManager.RemoveFromRoleAsync(user, "Hacker").GetAwaiter();
            }

            return true;
        }

        public async Task<bool> EliminateTeams()
        {
            var teams = await _session.Query<ContentItem, CandevItemsIndex>(x => x.ContentType == "Team" && x.Published).ListAsync();
            var challenges = await _session.Query<ContentItem, CandevItemsIndex>(x => x.ContentType == "Challenge" && x.Published && x.Culture == "en" ).ListAsync();
            var scores = await _session.Query<ContentItem, CandevItemsIndex>(x => x.ContentType == "Score" && x.Published).ListAsync();
            var teamsScores = new Dictionary<string, int>();
            var highestScore = 0;

            foreach(var challenge in challenges)
            {
                var teamlist = teams.Where(x => x.Content.Team.Challenge.ContentItemIds.First == challenge.ContentItemId).ToList();
                teamsScores.Clear();

                if (teamlist.Count != 0)
                {
                    foreach (var team in teamlist)
                    {
                        var scoreList = scores.Where(x => x.Content.Score.Team.ContentItemIds.First == team.ContentItemId).ToList();
                        var score = scoreList.Sum(x => x.Content.Score.Score.Value);

                        if (scoreList.Count != 0)
                        {
                            teamsScores.Add(team.ContentItemId, score);
                        }
                    }

                    highestScore = teamsScores.Max(x => x.Value);

                    foreach (var teamScore in teamsScores)
                    {
                        if (teamScore.Value == highestScore)
                        {
                            var team = teams.Where(x => x.ContentItemId == teamScore.Key).FirstOrDefault();
                            team.Content.Team.InTheRunning = JObject.FromObject(new { Value = true });
                            await _contentManager.UpdateAsync(team);
                        }
                    }
                }
            }

            return true;
        }

        private async void RemoveFromTeam(User user)
        {
            var team = await _session.Query<ContentItem, CandevItemsIndex>(x => x.ContentItemId == user.GetTeamId() && x.ContentType == "Team" && x.Published).FirstOrDefaultAsync();
            var teamContentItemId = user.GetTeamId();

            //Remove user from team
            var contentItem = await GetSettings(user, "Hacker");
            contentItem.Content.Hacker.Team?.ContentItemIds.Clear();
            user.Properties["Hacker"] = JObject.FromObject(contentItem);
            _session.Save(user);

            //if the team has no member left, delete it
            if (await GetTeamMemberCount(teamContentItemId) > 0)
            {
                //if the user was the team captain, make another hacker the team captain
                if (team.Content.Team?.TeamCaptain?.UserIds?.First == user.UserId)
                {
                    var hacker = await _session.Query<User, CandevUsersIndex>(x => x.TeamContentItemId == teamContentItemId).FirstOrDefaultAsync();
                    team.Content.Team.TeamCaptain = JObject.FromObject(new { UserIds = new string[] { hacker.UserId } });
                    await _contentManager.UpdateAsync(team);
                }
            }
            else
            {
                await _contentManager.RemoveAsync(team);
            }
        }
    }
}

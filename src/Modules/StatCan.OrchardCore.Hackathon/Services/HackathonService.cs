using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using StatCan.OrchardCore.Hackathon.Indexes;
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

namespace StatCan.OrchardCore.Hackathon.Services
{
    public class HackathonService : IHackathonService
    {
        private readonly YesSql.ISession _session;
        private readonly IContentManager _contentManager;
        private readonly IQueryManager _queryManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISiteService _siteService;

        public HackathonService(YesSql.ISession session,
            IStringLocalizer<HackathonService> localizer,
            IContentManager contentManager,
            IQueryManager queryManager,
            IHttpContextAccessor httpContextAccessor,
            ISiteService siteService
        )
        {
            _session = session;
            _contentManager = contentManager;
            _queryManager = queryManager;
            _httpContextAccessor = httpContextAccessor;
            T = localizer;
            _siteService = siteService;
        }

        public IStringLocalizer T { get; }

        public Task<User> GetParticipantFromSetAsync()
        {
            var user = _httpContextAccessor.HttpContext.User;
            if (!user.Identity.IsAuthenticated)
            {
                return Task.FromResult<User>(null);
            }
            return _session.Query<User, HackathonUsersIndex>(x => x.UserId == user.FindFirst(ClaimTypes.NameIdentifier).Value).FirstOrDefaultAsync();
        }

        public Task<int> GetTeamMemberCount(string teamContentItemId)
        {
            return _session.QueryIndex<HackathonUsersIndex>(x => x.TeamContentItemId == teamContentItemId).CountAsync();
        }

        public Task<IEnumerable<User>> GetTeamMembers(string teamContentItemId)
        {
            return _session.Query<User, HackathonUsersIndex>(x => x.TeamContentItemId == teamContentItemId).ListAsync();
        }

        public async Task<bool> TeamExists(string teamContentItemId)
        {
            var teamCount = await _session.QueryIndex<HackathonItemsIndex>(x => x.ContentItemId == teamContentItemId && x.ContentType == "Team" && x.Published).CountAsync();
            return teamCount > 0;
        }

        public async Task<bool> IsTeamFull(string teamContentItemId)
        {
            var site = await _siteService.GetSiteSettingsAsync();
            var hackathonCustomSettings = site.As<ContentItem>("HackathonCustomSettings");

            var members = await _session.QueryIndex<HackathonUsersIndex>(x => x.TeamContentItemId == teamContentItemId).ListAsync();
            if (members.Any())
            {
                if (members.Count() >= hackathonCustomSettings.Content["TeamCustomSettings"]["TeamSize"].Value.Value)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<ContentItem> JoinTeam(string teamContentItemId, ModelStateDictionary modelState)
        {
            var user = await GetParticipantFromSetAsync();

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

            var hacker = user.Properties.ToObject<ContentItem>();
            hacker.Content.Hacker.Hacker.Team.ContentItemIds.Add(team.ContentItemId);
            user.Properties = JObject.FromObject(hacker);

            _session.Save(user);

            return team;
        }

        public async Task<ContentItem> CreateTeam(ModelStateDictionary modelState)
        {
            var user = await GetParticipantFromSetAsync();
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
            await _contentManager.UpdateAsync(team);

            var hacker = user.Properties.ToObject<ContentItem>();
            hacker.Content.Hacker.Hacker.Team.ContentItemIds.Add(team.ContentItemId);
            user.Properties = JObject.FromObject(hacker);

            _session.Save(user);

            return team;
        }

        public async Task<bool> LeaveTeam(ModelStateDictionary modelState)
        {
            var user = await GetParticipantFromSetAsync();
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

            var hacker = user.Properties.ToObject<ContentItem>();
            hacker.Content.Hacker.Hacker.Team.ContentItemIds.Clear();
            user.Properties = JObject.FromObject(hacker);

            _session.Save(user);

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
            var hackathon = await _session.Query<ContentItem, HackathonItemsIndex>(x => x.ContentType == "Hackathon").FirstOrDefaultAsync();
            var hackersWithoutTeams = (await _session.Query<ContentItem, HackathonItemsIndex>(x => x.TeamContentItemId == null && x.ContentType == "Hacker" && x.Published).ListAsync()).ToList();
            // remove non attending hackers from algo
            hackersWithoutTeams = hackersWithoutTeams.Where(h => h.Content.ParticipantPart?.Attending?.Value == true).ToList();

            int maxTeamSize = (int)hackathon.Content.Hackathon.MaxTeamSize?.Value;
            await CleanupTeams();
            var unorderedTeams = await _session.QueryIndex<HackathonItemsIndex>(x => x.ContentType == "Team" && x.Published).ListAsync();
            var teams = unorderedTeams.OrderByDescending(x => (GetTeamMemberCount(x.ContentItemId).GetAwaiter().GetResult())).ToList();

            var incompleteTeams = Array.Empty<HackathonItemsIndex>().ToList();
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
                    if (memberCount <= maxTeamSize)
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
                    await AddHackerToTeam(team.ContentItemId, hackersWithoutTeams[0].ContentItemId);
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
                    teamId = await CreateTeam();
                    await AddHackerToTeam(teamId, hacker.ContentItemId);
                    n++;
                    hackersWithoutTeams.RemoveAt(0);
                }
                else
                {
                    await AddHackerToTeam(teamId, hacker.ContentItemId);
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

        private async Task<bool> CombineTeams(string teamContentId1, HackathonItemsIndex team2)
        {
            var team2Members = await GetTeamMembers(team2.ContentItemId);
            var team1 = JObject.FromObject(new { ContentItemIds = new string[] { teamContentId1 } });
            foreach (var member in team2Members)
            {
                /*member.ContentItem.Content.Hacker.Team = team1;
                await _contentManager.UpdateAsync(member);*/
            }
            // Delete team 2 
            await _contentManager.RemoveAsync(await _contentManager.GetAsync(team2.ContentItemId));
            return true;
        }

        private async Task<bool> AddHackerToTeam(string teamContentId, string participantContentId)
        {
            var participant = await _session.Query<ContentItem, HackathonItemsIndex>(x => x.ContentItemId == participantContentId && x.ContentType == "Hacker" && x.Published).FirstOrDefaultAsync();
            participant.Content.Hacker.Team = JObject.FromObject(new { ContentItemIds = new string[] { teamContentId } });
            await _contentManager.UpdateAsync(participant);
            return true;
        }

        private async Task<string> CreateTeam()
        {
            var team = await _contentManager.NewAsync("Team");
            team.DisplayText = randomName();
            await _contentManager.CreateAsync(team, VersionOptions.Published);
            await _contentManager.UpdateAsync(team);
            return team.ContentItemId;
        }

        private async Task<bool> CleanupTeams()
        {
            foreach (var team in await _session.Query<ContentItem, HackathonItemsIndex>(x => x.ContentType == "Team").ListAsync())
            {
                var count = await GetTeamMemberCount(team.ContentItemId);
                if (count == 0)
                {
                    await _contentManager.RemoveAsync(team);
                }
                // remove teams with 1 member that are not attending.
                if (count == 1)
                {
                    var member = (await GetTeamMembers(team.ContentItemId)).FirstOrDefault();
                    if (member != null)
                    {
                        /*if (member.Content.ParticipantPart?.Attending?.Value != true)
                        {
                            await _contentManager.RemoveAsync(team);
                            member.Content.Hacker.Team = JObject.FromObject(new { ContentItemIds = new string[] { } });
                            await _contentManager.UpdateAsync(member);
                        }*/
                    }
                }
            }
            return true;
        }

        public async Task<bool> RunUpdateHandlers()
        {
            var allItems = await _session.Query<ContentItem, HackathonItemsIndex>(x => x.Published).ListAsync();
            foreach (var item in allItems)
            {
                await _contentManager.UpdateAsync(item);
            }
            return true;
        }
    }
}

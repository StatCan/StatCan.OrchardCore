using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StatCan.OrchardCore.Hackathon.Indexes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json.Linq;
using OrchardCore.ContentManagement;
using OrchardCore.Queries;
using YesSql;

namespace StatCan.OrchardCore.Hackathon.Services
{
    public class HackathonService : IHackathonService
    {
        private readonly YesSql.ISession _session;
        private readonly IContentManager _contentManager;
        private readonly IQueryManager _queryManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HackathonService(YesSql.ISession session,
            IStringLocalizer<HackathonService> localizer,
            IContentManager contentManager,
            IQueryManager queryManager,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _session = session;
            _contentManager = contentManager;
            _queryManager = queryManager;
            _httpContextAccessor = httpContextAccessor;
            T = localizer;
        }

        public IStringLocalizer T { get; }

        public Task<ContentItem> GetParticipantFromSetAsync()
        {
            var user = _httpContextAccessor.HttpContext.User;
            if (!user.Identity.IsAuthenticated)
            {
                return Task.FromResult<ContentItem>(null);
            }
            return _session.Query<ContentItem, HackathonItemsIndex>(x => x.Owner == user.Identity.Name && (x.ContentType == "Hacker" || x.ContentType == "Volunteer")).FirstOrDefaultAsync();
        }

        public Task<int> GetTeamMemberCount(string teamContentItemId)
        {
            return _session.QueryIndex<HackathonItemsIndex>(x => x.TeamContentItemId == teamContentItemId && x.ContentType == "Hacker" && x.Published).CountAsync();
        }

        public Task<IEnumerable<ContentItem>> GetTeamMembers(string teamContentItemId)
        {
            return _session.Query<ContentItem, HackathonItemsIndex>(x => x.TeamContentItemId == teamContentItemId && x.ContentType == "Hacker" && x.Published).ListAsync();
        }

        public async Task<bool> TeamExists(string teamContentItemId)
        {
            var teamCount = await _session.QueryIndex<HackathonItemsIndex>(x => x.ContentItemId == teamContentItemId && x.ContentType == "Team" && x.Published).CountAsync();
            return teamCount > 0;
        }

        public async Task<bool> IsTeamFull(string teamContentItemId)
        {
            var members = await _session.QueryIndex<HackathonItemsIndex>(x => x.TeamContentItemId == teamContentItemId && x.ContentType == "Hacker" && x.Published).ListAsync();
            if (members.Any())
            {
                var hackathon = await _session.Query<ContentItem, HackathonItemsIndex>(x => x.ContentType == "Hackathon").FirstOrDefaultAsync();
                if (hackathon != null && members.Count() >= hackathon.Content.Hackathon.MaxTeamSize?.Value?.ToObject<int>())
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<ContentItem> JoinTeam(string teamContentItemId, ModelStateDictionary modelState)
        {
            var participant = await GetParticipantFromSetAsync();
            if (participant == null || participant.ContentType != "Hacker")
            {
                modelState.AddModelError("error", T["You are not a hacker"].Value);
                return null;
            }

            if (participant.HasTeam())
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

            participant.Content.Hacker.Team = JObject.FromObject(new { ContentItemIds = new string[] { team.ContentItemId } });

            await _contentManager.UpdateAsync(participant);

            return team;
        }

        public async Task<ContentItem> CreateTeam(ModelStateDictionary modelState)
        {
            var participant = await GetParticipantFromSetAsync();
            if (participant == null || participant.ContentType != "Hacker")
            {
                modelState.AddModelError("error", T["You are not a hacker"].Value);
                return null;
            }

            if (participant.HasTeam())
            {
                modelState.AddModelError("error", T["Already on a team"].Value);
                return null;
            }

            var user = _httpContextAccessor.HttpContext.User;
            var team = await _contentManager.NewAsync("Team");
            team.Owner = user.Identity.Name;

            await _contentManager.CreateAsync(team, VersionOptions.Published);

            await _contentManager.UpdateAsync(team);
            participant.Content.Hacker.Team = JObject.FromObject(new { ContentItemIds = new string[] { team.ContentItemId } });
            await _contentManager.UpdateAsync(participant);

            return team;
        }

        public async Task<bool> LeaveTeam(ModelStateDictionary modelState)
        {
            var participant = await GetParticipantFromSetAsync();
            if (participant == null || participant.ContentType != "Hacker")
            {
                modelState.AddModelError("error", T["You are not a hacker"].Value);
                return false;
            }

            if (!participant.HasTeam())
            {
                modelState.AddModelError("error", T["You are not part of a team"].Value);
                return false;
            }

            participant.Content.Hacker.Team = JObject.FromObject(new { ContentItemIds = new string[0] });
            await _contentManager.UpdateAsync(participant);

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
                member.ContentItem.Content.Hacker.Team = team1;
                await _contentManager.UpdateAsync(member);
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
                        if (member.Content.ParticipantPart?.Attending?.Value != true)
                        {
                            await _contentManager.RemoveAsync(team);
                            member.Content.Hacker.Team = JObject.FromObject(new { ContentItemIds = new string[] { } });
                            await _contentManager.UpdateAsync(member);
                        }
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

        public async Task<bool> RemoveTeamMember(string hackerContentItemId, ModelStateDictionary modelState)
        {
            var participant = await _session.Query<ContentItem, HackathonItemsIndex>(x => x.ContentItemId == hackerContentItemId && x.ContentType == "Hacker" && x.Published).FirstOrDefaultAsync();
            if (participant == null || participant.ContentType != "Hacker")
            {
                modelState.AddModelError("error", T["You are not a hacker"].Value);
                return false;
            }

            if (!participant.HasTeam())
            {
                modelState.AddModelError("error", T["You are not part of a team"].Value);
                return false;
            }

            participant.Content.Hacker.Team = JObject.FromObject(new { ContentItemIds = new string[0] });
            await _contentManager.UpdateAsync(participant);

            return true;
        }

        public async Task<bool> SaveTeam(string teamContentItemId, string teamDescription, string challenge, ModelStateDictionary modelState)
        {
            var team = await _session.Query<ContentItem, HackathonItemsIndex>(x => x.ContentItemId == teamContentItemId && x.ContentType == "Team" && x.Published).FirstOrDefaultAsync();
            if (team == null || team.ContentType != "Team")
            {
                modelState.AddModelError("error", T["Team doesn't exist"].Value);
                return false;
            }

            team.Content.Team.Description = JObject.FromObject(new { Text = teamDescription });
            team.Content.Team.Challenge = JObject.FromObject(new { ContentItemIds = challenge });
            await _contentManager.UpdateAsync(team);

            return true;
        }
    }
}

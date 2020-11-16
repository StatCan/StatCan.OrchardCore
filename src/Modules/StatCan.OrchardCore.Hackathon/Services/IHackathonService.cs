using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OrchardCore.ContentLocalization.Models;
using OrchardCore.ContentManagement;

namespace StatCan.OrchardCore.Hackathon.Services
{
    public interface IHackathonService
    {
        Task<ContentItem> GetParticipantFromSetAsync();
        Task<int> GetTeamMemberCount(string teamContentItemId);
        Task<IEnumerable<ContentItem>> GetTeamMembers(string teamContentItemId);
        Task<bool> TeamExists(string teamContentItemId);
        Task<bool> IsTeamFull(string teamContentItemId);
        Task<ContentItem> CreateTeam(ModelStateDictionary modelState);
        Task<ContentItem> JoinTeam(string teamContentItemId, ModelStateDictionary modelState);
        Task<bool> LeaveTeam(ModelStateDictionary modelState);
        Task<bool> MatchTeams();
        string RootUrl(string hackathonSlug);
        Task<bool> RunUpdateHandlers();
    }
}
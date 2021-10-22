using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OrchardCore.ContentLocalization.Models;
using OrchardCore.ContentManagement;
using OrchardCore.Users.Models;

namespace StatCan.OrchardCore.Candev.Services
{
    public interface ICandevService
    {
        Task<User> GetParticipantAsync();
        Task<int> GetTeamMemberCount(string teamContentItemId);
        Task<IEnumerable<User>> GetTeamMembers(string teamContentItemId);
        Task<bool> TeamExists(string teamContentItemId);
        Task<bool> IsTeamFull(string teamContentItemId);
        Task<ContentItem> CreateTeam(ModelStateDictionary modelState);
        Task<string> CreateTeam(string teamName, string[] topics);
        Task<ContentItem> JoinTeam(string teamContentItemId, ModelStateDictionary modelState);
        Task<ContentItem> JoinTeam(string teamContentItemId, string userName);
        Task<bool> LeaveTeam(ModelStateDictionary modelState);
        Task<bool> MatchTeams();
        string RootUrl(string hackathonSlug);
        Task<bool> RunUpdateHandlers();
        Task<bool> RemoveTeamMember(string hackerContentItemId, ModelStateDictionary modelState);
        Task<bool> SaveTeam(string teamName, string teamDescription, string challenge, ModelStateDictionary modelState);
        Task<bool> AssignCases();
        Task<string> CreateChallenge(string challengeTitle);
        Task<string> CreateTopic(string topicName, string challengeId);
    }
}

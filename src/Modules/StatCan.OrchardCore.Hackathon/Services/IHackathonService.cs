using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OrchardCore.ContentLocalization.Models;
using OrchardCore.ContentManagement;

namespace StatCan.OrchardCore.Hackathon.Services
{
    public interface IHackathonService
    {
        //Task<LocalizationEntry> GetLocalizationEntryAsync(string hackathonSlug);
        Task<ContentItem> GetParticipantFromSetAsync();
        Task<int> GetTeamMemberCount(string teamContentItemId);
        Task<IEnumerable<ContentItem>> GetTeamMembers(string teamContentItemId);
        Task<bool> TeamExists(string teamContentItemId);
        Task<bool> IsTeamFull(string teamContentItemId);
        //Task<bool> SetCase(string caseLocalizationSet, ModelStateDictionary modelState);
        Task<ContentItem> CreateTeam(ModelStateDictionary modelState);
        Task<ContentItem> JoinTeam(string teamContentItemId, ModelStateDictionary modelState);
        Task<bool> LeaveTeam(ModelStateDictionary modelState);
        Task<ContentItem> SetTeamRepository(string teamContentItemId, string repositoryPath);
        Task<bool> SetAttendance(ModelStateDictionary modelState);
        Task<bool> MatchTeams();
        Task<bool> AssignCases();
        //Task<ContentItem> GetHackathon(string hackathonLocalizationSet);
        /// <summary>
        /// Prepends '/' to the slug if null or missing
        /// </summary>
        string RootUrl(string hackathonSlug);
        Task<bool> SelectNParticipants(int n);
        Task<bool> RunUpdateHandlers();
        Task<bool> ToggleParticipantRegistration();
        Task<bool> ToggleVolunteerRegistration();
    }
}
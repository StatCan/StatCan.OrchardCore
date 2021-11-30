using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.DisplayManagement.ModelBinding;
using Microsoft.AspNetCore.Authorization;
using StatCan.OrchardCore.Candev.Services;
using OrchardCore.DisplayManagement.Notify;
using Microsoft.AspNetCore.Mvc.Localization;
using OrchardCore.Settings;
using OrchardCore.ContentManagement;
using OrchardCore.Entities;
using OrchardCore.Modules;

namespace StatCan.OrchardCore.Candev.Controllers
{
    [RequireFeatures(FeatureIds.Candev)]
    [Authorize]
    public class CandevController : Controller, IUpdateModel
    {
        private readonly ICandevService _candevService;
        private readonly IAuthorizationService _authorizationService;
        private readonly INotifier _notifier;
        private readonly YesSql.ISession _session;
        private readonly ISiteService _siteService;

        public CandevController(ICandevService hackathonService,
            IHtmlLocalizer<CandevController> htmlLocalizer,
            IAuthorizationService authorizationService,
            INotifier notifier,
            YesSql.ISession session,
            ISiteService siteService
        ){
            _candevService = hackathonService;
            H = htmlLocalizer;
            _authorizationService = authorizationService;
            _notifier = notifier;
            _session = session;
            _siteService = siteService;
        }

        public IHtmlLocalizer H {get;}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTeam(string returnUrl)
        {
            if (!HttpContext.User.IsInRole("Hacker"))
            {
                return NotFound();
            }

            var site = await _siteService.GetSiteSettingsAsync();
            var hackathonCustomSettings = site.As<ContentItem>("HackathonCustomSettings");

            if (hackathonCustomSettings.Content["TeamCustomSettings"]["TeamEditable"].Value == false)
            {
                return Unauthorized();
            }

            await _candevService.CreateTeam(ModelState);
            if (ModelState.IsValid)
            {
                _notifier.Success(H["Successfully created team"]);
            }

            await _session.SaveChangesAsync();
            return LocalRedirect(returnUrl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> JoinTeam(string teamContentItemId, string returnUrl)
        {
            if (!HttpContext.User.IsInRole("Hacker"))
            {
                return NotFound();
            }

            if (string.IsNullOrEmpty(teamContentItemId))
            {
                _notifier.Error(H["Enter a team ID"]);
                return LocalRedirect(returnUrl);
            }

            var site = await _siteService.GetSiteSettingsAsync();
            var hackathonCustomSettings = site.As<ContentItem>("HackathonCustomSettings");

            if (hackathonCustomSettings.Content["TeamCustomSettings"]["TeamEditable"].Value == false)
            {
                return Unauthorized();
            }

            await _candevService.JoinTeam(teamContentItemId, ModelState);
            if (ModelState.IsValid)
            {
                _notifier.Success(H["Successfully joined team"]);
            } else {
                _notifier.Error(H["Team could not be found"]);
            }

            await _session.SaveChangesAsync();
            return LocalRedirect(returnUrl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LeaveTeam(string returnUrl)
        {
            if (!HttpContext.User.IsInRole("Hacker"))
            {
                return NotFound();
            }

            var site = await _siteService.GetSiteSettingsAsync();
            var hackathonCustomSettings = site.As<ContentItem>("HackathonCustomSettings");

            if (hackathonCustomSettings.Content["TeamCustomSettings"]["TeamEditable"].Value == false)
            {
                return Unauthorized();
            }

            await _candevService.LeaveTeam(ModelState);
            if (ModelState.IsValid)
            {
                _notifier.Success(H["Successfully left team"]);
            }

            await _session.SaveChangesAsync();
            return LocalRedirect(returnUrl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveTeamMember(string hackerContentItemId, string returnUrl)
        {
            if (!HttpContext.User.IsInRole("Hacker"))
            {
                return NotFound();
            }

            var site = await _siteService.GetSiteSettingsAsync();
            var hackathonCustomSettings = site.As<ContentItem>("HackathonCustomSettings");
            if (hackathonCustomSettings.Content["TeamCustomSettings"]["TeamEditable"].Value == false)
            {
                return Unauthorized();
            }

            await _candevService.RemoveTeamMember(hackerContentItemId, ModelState);
            if (ModelState.IsValid)
            {
                _notifier.Success(H["Member successfully removed from the team"]);
            }

            await _session.SaveChangesAsync();
            return LocalRedirect(returnUrl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveTeam(string teamName, string teamDescription, string challenge, string returnUrl)
        {
            if (!HttpContext.User.IsInRole("Hacker"))
            {
                return NotFound();
            }

            var site = await _siteService.GetSiteSettingsAsync();
            var hackathonCustomSettings = site.As<ContentItem>("HackathonCustomSettings");
            if (hackathonCustomSettings.Content["TeamCustomSettings"]["TeamEditable"].Value == false)
            {
                return Unauthorized();
            }

            await _candevService.SaveTeam(teamName, teamDescription, challenge, ModelState);
            if (ModelState.IsValid)
            {
                _notifier.Success(H["Team info successfully updated"]);
            }

            await _session.SaveChangesAsync();
            return LocalRedirect(returnUrl);
        }
    }
}

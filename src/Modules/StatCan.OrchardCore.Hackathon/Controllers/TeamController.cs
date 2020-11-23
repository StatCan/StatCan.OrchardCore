using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using OrchardCore.DisplayManagement;
using OrchardCore.DisplayManagement.ModelBinding;
using Microsoft.AspNetCore.Authorization;
using StatCan.OrchardCore.Hackathon.Services;
using OrchardCore.DisplayManagement.Notify;
using Microsoft.AspNetCore.Mvc.Localization;
using OrchardCore.Settings;
using OrchardCore.ContentManagement;
using OrchardCore.Entities;
using OrchardCore.Modules;

namespace StatCan.OrchardCore.Hackathon.Controllers
{
    [RequireFeatures(FeatureIds.Team)]
    [Authorize]
    public class TeamController : Controller, IUpdateModel
    {
        private readonly IHackathonService _hackathonService;
        private readonly IAuthorizationService _authorizationService;
        private readonly INotifier _notifier;
        private readonly YesSql.ISession _session;
        private readonly ISiteService _siteService;
        public dynamic New { get; set; }

        public TeamController(IShapeFactory shapeFactory,
            IHackathonService hackathonService,
            IHtmlLocalizer<TeamController> htmlLocalizer,
            IAuthorizationService authorizationService,
            INotifier notifier,
            YesSql.ISession session,
            ISiteService siteService
        ){
            New = shapeFactory;
            _hackathonService = hackathonService;
            H = htmlLocalizer;
            _authorizationService = authorizationService;
            _notifier = notifier;
            _session = session;
            _siteService = siteService;
        }

        public IHtmlLocalizer H {get;}

        private string GetPrefixedUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return "~/";
            }
            if (!url.StartsWith('/'))
            {
                url = "/" + url;
            }
            return "~" + url;
        }

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

            await _hackathonService.CreateTeam(ModelState);
            if (ModelState.IsValid)
            {
                _notifier.Success(H["Successfully created team"]);
            }

            await _session.CommitAsync();
            return LocalRedirect(GetPrefixedUrl(returnUrl));
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
                return LocalRedirect(GetPrefixedUrl(returnUrl));
            }

            var site = await _siteService.GetSiteSettingsAsync();
            var hackathonCustomSettings = site.As<ContentItem>("HackathonCustomSettings");

            if (hackathonCustomSettings.Content["TeamCustomSettings"]["TeamEditable"].Value == false)
            {
                return Unauthorized();
            }

            await _hackathonService.JoinTeam(teamContentItemId, ModelState);
            if (ModelState.IsValid)
            {
                _notifier.Success(H["Successfully joined team"]);
            } else {
                _notifier.Error(H["Team could not be found"]);
            }

            await _session.CommitAsync();
            return LocalRedirect(GetPrefixedUrl(returnUrl));
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

            await _hackathonService.LeaveTeam(ModelState);
            if (ModelState.IsValid)
            {
                _notifier.Success(H["Successfully left team"]);
            }

            await _session.CommitAsync();
            return LocalRedirect(GetPrefixedUrl(returnUrl));
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

            await _hackathonService.RemoveTeamMember(hackerContentItemId, ModelState);
            if (ModelState.IsValid)
            {
                _notifier.Success(H["Member successfully removed from the team"]);
            }

            await _session.CommitAsync();
            return LocalRedirect(GetPrefixedUrl(returnUrl));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveTeam(string teamContentItemId, string teamDescription, string challenge, string returnUrl)
        {
            if (!HttpContext.User.IsInRole("Hacker"))
            {
                return NotFound();
            }

            await _hackathonService.SaveTeam(teamContentItemId, teamDescription, challenge, ModelState);
            if (ModelState.IsValid)
            {
                _notifier.Success(H["Team info successfully updated"]);
            }

            await _session.CommitAsync();
            return LocalRedirect(GetPrefixedUrl(returnUrl));
        }
    }
}

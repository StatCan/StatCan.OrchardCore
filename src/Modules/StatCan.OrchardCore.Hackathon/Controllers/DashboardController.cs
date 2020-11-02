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

namespace StatCan.OrchardCore.Hackathon.Controllers
{
    [Authorize]
    public class DashboardController : Controller, IUpdateModel
    {
        private readonly IHackathonService _hackathonService;
        private readonly IAuthorizationService _authorizationService;
        private readonly INotifier _notifier;
        //private readonly ISchoolService _schoolService;
        //private readonly IDepartmentService _departmentService;
        private readonly YesSql.ISession _session;
        private readonly ISiteService _siteService;
        public dynamic New { get; set; }

        public DashboardController(IShapeFactory shapeFactory,
            IHackathonService hackathonService,
            IHtmlLocalizer<DashboardController> htmlLocalizer,
            //ISchoolService schoolService,
            //IDepartmentService departmentService,
            IAuthorizationService authorizationService, INotifier notifier, YesSql.ISession session, ISiteService siteService)
        {
            New = shapeFactory;
            _hackathonService = hackathonService;
            H = htmlLocalizer;
            //_schoolService = schoolService;
            //_departmentService = departmentService;
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
        /*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetCase(string hackathonLocalizationSet, string caseLocalizationSet, string returnUrl)
        {
            if (!(await _authorizationService.AuthorizeAsync(User, hackathonLocalizationSet, ParticipantType.Hacker)))
            {
                return NotFound();
            }
            var hackathon = await _hackathonService.GetHackathon(hackathonLocalizationSet);
            if(hackathon.Content.Hackathon.CasesSelectable?.Value == false)
            {
                return Unauthorized();
            }
            await _hackathonService.SetCase(hackathonLocalizationSet, caseLocalizationSet, ModelState);

            if (ModelState.IsValid)
            {
                _notifier.Success(H["Successfully selected a challenge"]);
            }
            await _session.CommitAsync();
            return LocalRedirect(GetPrefixedUrl(returnUrl));
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Attendance(string hackathonLocalizationSet, string returnUrl)
        {
            if (!await _authorizationService.AuthorizeAsync(User, hackathonLocalizationSet, ParticipantType.Hacker))
            {
                return NotFound();
            }
            var hackathon = await _hackathonService.GetHackathon(hackathonLocalizationSet);
            await _hackathonService.SetAttendance(hackathonLocalizationSet, ModelState);
            if (ModelState.IsValid)
            {
                _notifier.Success(H["Succesfully changed attendance."]);
            }
            await _session.CommitAsync();
            return LocalRedirect(GetPrefixedUrl(returnUrl));
        }

        public JsonResult UniversityList()
        {
            return Json(_schoolService.UniversityJson);
        }

        public JsonResult DegreeList(string university)
        {
            return Json(_schoolService.DegreeJson.GetValue(university));
        }

        public JsonResult DepartmentList()
        {
            return Json(_departmentService.DepartmentJson);
        }*/
    }
}
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.ContentManagement;
using OrchardCore.DisplayManagement;
using OrchardCore.DisplayManagement.ModelBinding;
using YesSql;
using Microsoft.AspNetCore.Mvc.Localization;
using OrchardCore.Settings;
using OrchardCore.Entities;
using StatCan.OrchardCore.Candev.Services;

namespace StatCan.OrchardCore.Candev.Controllers
{
    public class AdminController : Controller, IUpdateModel
    {
        private readonly ISiteService _siteService;
        private readonly ISession _session;
        private readonly ICandevService _candevService;

        public dynamic New { get; set; }

        public AdminController(ISiteService siteService, ISession session, IShapeFactory shapeFactory,
          ICandevService hackathonService,
          IHtmlLocalizer<AdminController> localizer)
        {
            _siteService = siteService;
            _session = session;
            New = shapeFactory;
            _candevService = hackathonService;
            H = localizer;
        }

        public IHtmlLocalizer H {get;}

        public async Task<IActionResult> Index()
        {
            if (!HttpContext.User.IsInRole("Administrator"))
            {
                return Unauthorized();
            }

            var viewModel = await New.ViewModel();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignCases(string returnUrl)
        {
            if (!HttpContext.User.IsInRole("Administrator"))
            {
                return Unauthorized();
            }

            var site = await _siteService.GetSiteSettingsAsync();
            var hackathonCustomSettings = site.As<ContentItem>("HackathonCustomSettings");
            if (hackathonCustomSettings.Content["TeamCustomSettings"]["TeamEditable"].Value == true)
            {
                return Unauthorized();
            }

            await _candevService.AssignCases();

            await _session.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MatchTeams(string returnUrl)
        {
            if (!HttpContext.User.IsInRole("Administrator"))
            {
                return Unauthorized();
            }

            var site = await _siteService.GetSiteSettingsAsync();
            var hackathonCustomSettings = site.As<ContentItem>("HackathonCustomSettings");
            if (hackathonCustomSettings.Content["TeamCustomSettings"]["TeamEditable"].Value == true)
            {
                return Unauthorized();
            }

            await _candevService.MatchTeams();

            await _session.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}

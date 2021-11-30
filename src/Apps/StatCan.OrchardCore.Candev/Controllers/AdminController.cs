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
using OrchardCore.DisplayManagement.Notify;

namespace StatCan.OrchardCore.Candev.Controllers
{
    public class AdminController : Controller, IUpdateModel
    {
        private readonly ISiteService _siteService;
        private readonly ISession _session;
        private readonly ICandevService _candevService;
        private readonly INotifier _notifier;

        public dynamic New { get; set; }

        public AdminController(ISiteService siteService, ISession session, IShapeFactory shapeFactory,
          ICandevService hackathonService,
          IHtmlLocalizer<AdminController> localizer,
          INotifier notifier)
        {
            _siteService = siteService;
            _session = session;
            New = shapeFactory;
            _candevService = hackathonService;
            H = localizer;
            _notifier = notifier;
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
            _notifier.Success(H["Cases have been assigned."]);
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
            _notifier.Success(H["Teams have been matched."]);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SelectNHackers(int n)
        {
            if (!HttpContext.User.IsInRole("Administrator"))
            {
                return Unauthorized();
            }

            await _candevService.SelectNHackers(n);
            _notifier.Success(H["Participants have been selected."]);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckIn()
        {
            if (!HttpContext.User.IsInRole("Administrator"))
            {
                return Unauthorized();
            }

            await _candevService.CheckIn();
            _notifier.Success(H["Checked-In participants have been selected."]);
            return RedirectToAction("Index");
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.Modules;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StatCan.OrchardCore.Hackathon.Services;

namespace StatCan.OrchardCore.Hackathon.Controllers
{
    [RequireFeatures("StatCan.OrchardCore.Hackathon.Judging")]
    public class JudgingController : Controller, IUpdateModel
    {
        private readonly IHackathonService _hackathonService;
        private readonly IContentManager _contentManager;
        private readonly IContentItemDisplayManager _contentItemDisplayManager;
        private readonly IAuthorizationService _authorizationService;

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

        public JudgingController(
            IContentManager contentManager,
            IContentItemDisplayManager contentItemDisplayManager,
            IAuthorizationService authorizationService,
            IHackathonService hackathonService)
        {
            _authorizationService = authorizationService;
            _contentItemDisplayManager = contentItemDisplayManager;
            _contentManager = contentManager;
            _hackathonService = hackathonService;
        }

        public async Task<IActionResult> DisplayEntry(string contentItemId)
        {
            var contentItem = await _contentManager.GetAsync(contentItemId, VersionOptions.Latest);

            if (contentItem == null || contentItem.ContentType != "Team")
            {
                return NotFound();
            }

            /*var hackLocSet = (string)contentItem.Content.JudgingEntry.Hackathon?.LocalizationSets?.First;
            if (!await _authorizationService.AuthorizeAsync(User, hackLocSet, ParticipantType.Judge))
            {
                return NotFound();
            }*/

            //var judgeId = (string)contentItem.Content.JudgingEntry.Judge?.ContentItemIds?.First;
            /*var judge = await _hackathonService.GetParticipantFromSetAsync(hackLocSet);
            if (judgeId == null || judge.ContentItemId != judgeId)
            {
                return NotFound();
            }*/

            var model = await _contentItemDisplayManager.BuildDisplayAsync(contentItem, this);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PublishJudgingEntry(string contentItemId, string score, string returnUrl)
        {
            var contentItem = await _contentManager.GetAsync(contentItemId, VersionOptions.Latest);

            if (contentItem == null || contentItem.ContentType != "Team" || contentItem.Published)
            {
                return NotFound();
            }

            /*var hackLocSet = (string)contentItem.Content.JudgingEntry.Hackathon?.LocalizationSets?.First;
            if (!await _authorizationService.AuthorizeAsync(User, hackLocSet, ParticipantType.Judge))
            {
                return NotFound();
            }*/
            /*
            var judgeId = (string)contentItem.Content.JudgingEntry.Judge?.ContentItemIds?.First;
            var judge = await _hackathonService.GetParticipantFromSetAsync(hackLocSet);
            if (judgeId == null || judge.ContentItemId != judgeId)
            {
                return NotFound();
            }*/


            contentItem.ContentItem.Content.Score.Value = score;
            await _contentManager.UpdateAsync(contentItem);

            return LocalRedirect(GetPrefixedUrl(returnUrl));
        }
    }
}

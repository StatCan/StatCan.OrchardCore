using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrchardCore.ContentManagement;
using StatCan.OrchardCore.AjaxForms.Models;

namespace StatCan.OrchardCore.AjaxForms.Controllers
{
    public class AjaxFormController : Controller
    {
        private const bool V = true;
        private readonly ILogger _logger;
        private readonly IContentManager _contentManager;

        public AjaxFormController(
            ILogger<AjaxFormController> logger , IContentManager contentManager)
        {
            _logger = logger;
            _contentManager = contentManager;
        }

        [HttpPost]
        public async Task<IActionResult> Submit(string formId)
        {
            var form = await _contentManager.GetAsync(formId, VersionOptions.Published);

            if(form == null) {
              // todo, proper error code
                return NotFound();
            }

            var formPart = form.As<AjaxForm>();

            // Verify if the form is enabled
            if(!formPart.Enabled.Value)
            {
                // todo, proper error code
                return NotFound();
            }

            // form validation

            // trigger workflow if valid


            _logger.Log(LogLevel.Information, "this is a test");
            return Json("{}");
        }
    }
}

using System.Collections.Generic;
using System.Net;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.Liquid;
using OrchardCore.Scripting;
using OrchardCore.Workflows.Services;
using OrchardCore.Shortcodes.Services;
using StatCan.OrchardCore.VueForms.Models;
using StatCan.OrchardCore.VueForms.Workflows;
using StatCan.OrchardCore.Extensions;
using StatCan.OrchardCore.VueForms.Scripting;
using System.Linq;
using OrchardCore.Workflows.Http;

namespace StatCan.OrchardCore.VueForms.Controllers
{
    public class VueFormController : Controller
    {
        private readonly ILogger _logger;
        private readonly IContentManager _contentManager;
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly IContentItemDisplayManager _contentItemDisplayManager;
        private readonly IUpdateModelAccessor _updateModelAccessor;
        private readonly IScriptingManager _scriptingManager;
        private readonly ILiquidTemplateManager _liquidTemplateManager;
        private readonly HtmlEncoder _htmlEncoder;
        private readonly IShortcodeService _shortcodeService;
        private readonly IWorkflowManager _workflowManager;

        public VueFormController(
            ILogger<VueFormController> logger,
            IContentManager contentManager,
            IContentDefinitionManager contentDefinitionManager,
            IContentItemDisplayManager contentItemDisplayManager,
            IUpdateModelAccessor updateModelAccessor,
            IScriptingManager scriptingManager,
            ILiquidTemplateManager liquidTemplateManager,
            HtmlEncoder htmlEncoder,
            IShortcodeService shortcodeService,
            IWorkflowManager workflowManager = null
        )
        {
            _logger = logger;
            _contentManager = contentManager;
            _contentDefinitionManager = contentDefinitionManager;
            _contentItemDisplayManager = contentItemDisplayManager;
            _updateModelAccessor = updateModelAccessor;
            _scriptingManager = scriptingManager;
            _liquidTemplateManager = liquidTemplateManager;
            _htmlEncoder = htmlEncoder;
            _shortcodeService = shortcodeService;
            _workflowManager = workflowManager;
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Submit(string formId)
        {
            var form = await _contentManager.GetAsync(formId, VersionOptions.Published);

            if(form == null)
            {
                return NotFound();
            }

            var formPart = form.As<VueForm>();

            // Verify if the form is enabled
            if (!formPart.Enabled.Value)
            {
                return NotFound();
            }

            var scriptingProvider = new VueFormMethodsProvider(form);

            var script = form.As<VueFormScripts>();
            if (!string.IsNullOrEmpty(script?.OnValidation?.Text))
            {
                _scriptingManager.EvaluateJs(script.OnValidation.Text, scriptingProvider);
            }

            if (ModelState.ErrorCount > 0)
            {
                return Json(new { validationError = true, errors = GetErrorDictionary() });
            }

            object submitResult = null;

            if (!string.IsNullOrEmpty(script?.OnSubmitted?.Text))
            {
                submitResult = _scriptingManager.EvaluateJs(script.OnSubmitted.Text, scriptingProvider);
            }

            if (ModelState.ErrorCount > 0)
            {
                return Json(new { validationError = true, errors = GetErrorDictionary(), submitResult });
            }

            // _workflow manager is null if workflow feature is not enabled
            if (_workflowManager != null)
            {
                await _workflowManager.TriggerEventAsync(nameof(VueFormSubmittedEvent),
                    input: new { VueForm = form },
                    correlationId: form.ContentItemId
                );
            }

            // workflow added errors, return them here
            if (ModelState.ErrorCount > 0)
            {
                return Json(new { validationError = true, errors = GetErrorDictionary(), submitResult });
            }

            // Handle the redirects with ajax requests.
            // 302 are equivalent to 301 in this case. No permanent redirect.
            // This can come from a scripting method or the HttpRedirect Workflow Task
            if(HttpContext.Response.StatusCode == 301 || HttpContext.Response.StatusCode == 302)
            {
                var returnValue = new { redirect = WebUtility.UrlDecode(HttpContext.Response.Headers["Location"])};
                HttpContext.Response.Clear();
                return Json(returnValue);
            }

            // This get's set by either the HttpRedirectTask or HttpResponseTask
            if(HttpContext.Items[WorkflowHttpResult.Instance] != null)
            {
                // Let the HttpResponseTask control the response. This will fail on the client if it's anything other than json
                return new EmptyResult();
            }
            var formSuccessMessage = await _liquidTemplateManager.RenderAsync(formPart.SuccessMessage?.Text, _htmlEncoder);
            formSuccessMessage = await _shortcodeService.ProcessAsync(formSuccessMessage);
            // everything worked fine. send the success signal to the client
            return Json(new { successMessage = formSuccessMessage, submitResult });
        }
        private Dictionary<string, string[]> GetErrorDictionary()
        {
           return ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );
        }
    }
}

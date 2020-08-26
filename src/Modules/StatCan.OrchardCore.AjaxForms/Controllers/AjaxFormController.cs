using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.Environment.Shell.Descriptor.Models;
using OrchardCore.Flows.Models;
using OrchardCore.Scripting;
using OrchardCore.Workflows.Http;
using OrchardCore.Workflows.Scripting;
using OrchardCore.Workflows.Services;
using StatCan.OrchardCore.AjaxForms.Models;
using StatCan.OrchardCore.AjaxForms.Workflows;
using StatCan.OrchardCore.ContentsExtensions;

namespace StatCan.OrchardCore.AjaxForms.Controllers
{
    public class AjaxFormController : Controller
    {
        private readonly ILogger _logger;
        private readonly IContentManager _contentManager;
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly IContentItemDisplayManager _contentItemDisplayManager;
        private readonly IUpdateModelAccessor _updateModelAccessor;
        private readonly IScriptingManager _scriptingManager;
        private readonly IWorkflowManager _workflowManager;

        public AjaxFormController(
            ILogger<AjaxFormController> logger,
            IContentManager contentManager,
            IContentDefinitionManager contentDefinitionManager,
            IContentItemDisplayManager contentItemDisplayManager,
            IUpdateModelAccessor updateModelAccessor,
            IScriptingManager scriptingManager,
            IWorkflowManager workflowManager = null
            )
        {
            _logger = logger;
            _contentManager = contentManager;
            _contentDefinitionManager = contentDefinitionManager;
            _contentItemDisplayManager = contentItemDisplayManager;
            _updateModelAccessor = updateModelAccessor;
            _scriptingManager = scriptingManager;
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

            var formPart = form.As<AjaxForm>();

            // Verify if the form is enabled
            if (!formPart.Enabled.Value)
            {
                return NotFound();
            }
            var modelUpdater = _updateModelAccessor.ModelUpdater;
            // bind form values to model state for if validation errors occur
            foreach (var item in Request.Form)
            {
                modelUpdater.ModelState.SetModelValue(item.Key, item.Value, item.Value);
            }

            // form validation widgets
            var flow = form.As<FlowPart>();
            ValidateWidgets(flow.Widgets);

            // form validation script
            var script = form.As<AjaxFormScripts>();
            var scriptingProvider = new AjaxFormMethodsProvider(form, modelUpdater);
            if (!string.IsNullOrEmpty(script?.OnValidation?.Text))
            {
                _scriptingManager.EvaluateJs(script.OnValidation.Text, scriptingProvider);
            }

            var isValid = ModelState.ErrorCount == 0;

            if (!isValid)
            {
                var model = await _contentItemDisplayManager.BuildDisplayAsync(form, modelUpdater);
                var formHtml = await this.RenderViewAsync("Display", model, true);
                return Json(new { validationError = true, html = formHtml });
            }

            if (!string.IsNullOrEmpty(script?.OnSubmitted?.Text))
            {
                _scriptingManager.EvaluateJs(script.OnSubmitted.Text, scriptingProvider);
            }

            // _workflow manager is null if workflow feature is not enabled
            if (_workflowManager != null)
            {
                await _workflowManager.TriggerEventAsync(nameof(AjaxFormSubmittedEvent),
                    input: new { AjaxForm = form },
                    correlationId: form.ContentItemId
                );
            }
            

            // 302 are equivalent to 301 in this case. No permanent redirect 
            if(HttpContext.Response.StatusCode == 301 || HttpContext.Response.StatusCode == 302)
            {
                var returnValue = new { redirect = WebUtility.UrlDecode(HttpContext.Response.Headers["Location"])};
                HttpContext.Response.Clear();
                return Json(returnValue);
            }
            // we may need to send a success message
            return Json(new { error = false });
        }
        private void ValidateWidgets(IEnumerable<ContentItem> widgets)
        {
            var modelUpdater = _updateModelAccessor.ModelUpdater;
            foreach (var widget in widgets)
            {
                var formInput = widget.As<FormInput>();
                if (formInput != null)
                {
                    var value = Request.Form[formInput.Name.Text];
                    var requiredValidation = widget.As<FormRequiredValidation>();
                    if (requiredValidation.Required.Value && string.IsNullOrWhiteSpace(value))
                    {
                        //todo: localize
                        modelUpdater.ModelState.TryAddModelError(formInput.Name.Text, requiredValidation.RequiredText.Text ?? "This field is required");
                    }
                }
                var flow = widget.As<FlowPart>();
                if(flow != null)
                {
                    // recurse
                    ValidateWidgets(flow.Widgets);
                }
                // todo: check if we need to recurse on bag items as well
            }
        }
    }
}

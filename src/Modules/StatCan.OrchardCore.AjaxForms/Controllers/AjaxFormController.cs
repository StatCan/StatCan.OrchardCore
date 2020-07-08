using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.Flows.Models;
using OrchardCore.Scripting;
using OrchardCore.Workflows.Http;
using OrchardCore.Workflows.Scripting;
using OrchardCore.Workflows.Services;
using StatCan.OrchardCore.AjaxForms.Models;
using StatCan.OrchardCore.ContentsExtensions;

namespace StatCan.OrchardCore.AjaxForms.Controllers
{
    // todo: move this into it's own assembly.
    public static class ControllerExtensions
    {
        public static async Task<string> RenderViewAsync<TModel>(this Controller controller, string viewName, TModel model, bool partial = false)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = controller.ControllerContext.ActionDescriptor.ActionName;
            }

            controller.ViewData.Model = model;

            using (var writer = new StringWriter())
            {
                var viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
                ViewEngineResult viewResult = viewEngine.FindView(controller.ControllerContext, viewName, !partial);

                if (viewResult.Success == false)
                {
                    return $"A view with the name {viewName} could not be found";
                }

                ViewContext viewContext = new ViewContext(
                    controller.ControllerContext,
                    viewResult.View,
                    controller.ViewData,
                    controller.TempData,
                    writer,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);

                return writer.GetStringBuilder().ToString();
            }
        }
    }
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
            IWorkflowManager workflowManager)
        {
            _logger = logger;
            _contentManager = contentManager;
            _contentDefinitionManager = contentDefinitionManager;
            _contentItemDisplayManager = contentItemDisplayManager;
            _updateModelAccessor = updateModelAccessor;
            _scriptingManager = scriptingManager;
            _workflowManager = workflowManager;
        }


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

            // form validation
            var flow = form.As<FlowPart>();
            ValidateWidgets(flow.Widgets);

            // form validation script
            var script = form.As<AjaxFormScripts>();
            var scriptingProvider = new AjaxFormMethodsProvider(form, modelUpdater);
            if (!string.IsNullOrEmpty(script?.OnValidationScript?.Text))
            {
                _scriptingManager.EvaluateJs(script.OnValidationScript.Text, scriptingProvider);
            }

            var isValid = ModelState.ErrorCount == 0;

            if (!isValid)
            {
                var model = await _contentItemDisplayManager.BuildDisplayAsync(form, modelUpdater);
                var formHtml = await this.RenderViewAsync("Display", model, true);
                return Json(new { error = true, html = formHtml });
            }

            if (!string.IsNullOrEmpty(script?.OnSubmittedScript?.Text))
            {
                _scriptingManager.EvaluateJs(script.OnSubmittedScript.Text, scriptingProvider);
            }

            if (formPart.TriggerWorkflow.Value == true)
            {
                await _workflowManager.TriggerEventAsync("CHANGE_ME_TO_PROPER_WORKFLOW_EVENT",
                    input: new { ContentItem = form },
                    correlationId: form.ContentItemId
                );
            }

            // if the Workflow or script returned a HttpResult,
            // we may need to modify this to the format required by our form client 
            if (HttpContext.Items.ContainsKey(WorkflowHttpResult.Instance))
            {
                return new EmptyResult();
            }

            // TODO: Change to proper return value.
            // We will need to handle the case where the workflow returns a Result, especially with ajax.
            // We may need to trigger a redirect on the next request ? Or modify it into the proper request.
            // Will also need to handle the _notifier (custom messages).
            // The messages are lost after the first request as those are stored in a cookie for the next request
            return Accepted();
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
                    ValidateWidgets(flow.Widgets);
                }
            }
        }
    }
}

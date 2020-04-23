using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.DisplayManagement;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.Flows.Models;
using StatCan.OrchardCore.AjaxForms.Models;

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

        public AjaxFormController(
            ILogger<AjaxFormController> logger,
            IContentManager contentManager,
            IContentDefinitionManager contentDefinitionManager,
            IContentItemDisplayManager contentItemDisplayManager,
           IUpdateModelAccessor updateModelAccessor)
        {
            _logger = logger;
            _contentManager = contentManager;
            _contentDefinitionManager = contentDefinitionManager;
            _contentItemDisplayManager = contentItemDisplayManager;
            _updateModelAccessor = updateModelAccessor;
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

            var modelUpdater = _updateModelAccessor.ModelUpdater;

            // Verify if the form is enabled
            //if(!formPart.Enabled.Value)
            //{
            //    return NotFound();
            //}
            // bind form values to model state for if validation errors occur
            foreach (var item in Request.Form)
            {
                modelUpdater.ModelState.SetModelValue(item.Key, item.Value, item.Value);
            }

            // form validation
            var flow = form.As<FlowPart>();

            foreach (var widget in flow.Widgets)
            {
                var formInput = widget.As<FormInput>();
                if (formInput != null)
                {
                    var value = Request.Form[formInput.Name.Text];
                    if(formInput.Required.Value && string.IsNullOrWhiteSpace(value))
                    {
                        modelUpdater.ModelState.TryAddModelError(formInput.Name.Text, formInput.RequiredText.Text ?? "This field is required");
                    }
                }
            }

            var isValid = ModelState.ErrorCount == 0;

            var model = await _contentItemDisplayManager.BuildDisplayAsync(form, modelUpdater);

            var formHtml = await this.RenderViewAsync("Display", model, true);

            return Json(new { error = !isValid, html = formHtml });            
        }
    }
}

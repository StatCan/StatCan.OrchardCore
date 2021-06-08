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
using Etch.OrchardCore.ContentPermissions.Services;
using Microsoft.Extensions.Localization;
using System;
using System.Text.Json;
using Newtonsoft.Json;
using OrchardCore.Contents.Security;
using OrchardCore.Contents;
using Microsoft.AspNetCore.Authorization;

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
        private readonly IAuthorizationService _authorizationService;
        private readonly IContentPermissionsService _contentPermissionsService;
        private readonly IStringLocalizer S;
        private readonly IWorkflowManager _workflowManager;

        public VueFormController(
            ILogger<VueFormController> logger,
            IContentManager contentManager,
            IAuthorizationService authorizationService,
            IContentDefinitionManager contentDefinitionManager,
            IContentItemDisplayManager contentItemDisplayManager,
            IUpdateModelAccessor updateModelAccessor,
            IScriptingManager scriptingManager,
            ILiquidTemplateManager liquidTemplateManager,
            HtmlEncoder htmlEncoder,
            IShortcodeService shortcodeService,
            IContentPermissionsService contentPermissionsService,
            IStringLocalizer<VueFormController> stringLocalizer,
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
            _contentPermissionsService = contentPermissionsService;
            S = stringLocalizer;
            _workflowManager = workflowManager;
            _authorizationService = authorizationService;
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Submit(string formId)
        {
            var canView = ContentTypePermissionsHelper.CreateDynamicPermission(ContentTypePermissionsHelper.PermissionTemplates[CommonPermissions.ViewContent.Name], "VueForm");

            if (!await _authorizationService.AuthorizeAsync(User, canView))
            {
                return NotFound();
            }

            var form = await _contentManager.GetAsync(formId, VersionOptions.Published);

            if(form == null)
            {
                return NotFound();
            }

            var formPart = form.As<VueForm>();

            if (formPart.Disabled.Value)
            {
                return NotFound();
            }

            if (!_contentPermissionsService.CanAccess(form))
            {
                return NotFound();
            }

            var scriptingProvider = new VueFormMethodsProvider(form);

            var script = form.As<VueFormScripts>();

            // This object holds the return value of the script
            object serverScriptResult = EvaluateScript(script?.OnValidation?.Text, scriptingProvider, formPart, "OnValidation");

            // Return if any errors are returned in the OnValidation script
            if (ModelState.ErrorCount > 0)
            {
                return Json(new VueFormSubmitResult(GetErrorDictionary(), serverScriptResult, GetDebugLogs(formPart)));
            }

            serverScriptResult = EvaluateScript(script?.OnSubmitted?.Text, scriptingProvider, formPart, "OnSubmitted");

            // Return if any errors are returned from the OnSubmitted script
            if (ModelState.ErrorCount > 0)
            {
                return Json(new VueFormSubmitResult(GetErrorDictionary(), serverScriptResult, GetDebugLogs(formPart)));
            }

            // _workflow manager is null if workflow feature is not enabled
            if (_workflowManager != null)
            {
                await _workflowManager.TriggerEventAsync(nameof(VueFormSubmittedEvent),
                    input: new { VueForm = form },
                    correlationId: form.ContentItemId
                );
            }

            // Return if any errors are returned from the Workflow
            if (ModelState.ErrorCount > 0)
            {
                return Json(new VueFormSubmitResult(GetErrorDictionary(), serverScriptResult, GetDebugLogs(formPart)));
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

            // This get's set by either the workflow's HttpRedirectTask or HttpResponseTask
            if(HttpContext.Items[WorkflowHttpResult.Instance] != null)
            {
                // Let the HttpResponseTask control the response. This will fail on the client if it's anything other than json
                return new EmptyResult();
            }

            //try to get the message from the http context as set by the addSuccessMessage() scripting function
            var successMessage = string.Empty;
            if(HttpContext.Items[Constants.VueFormSuccessMessage] != null)
            {
               successMessage = (string)HttpContext.Items[Constants.VueFormSuccessMessage];
            }
            else
            {
                if(!string.IsNullOrWhiteSpace(formPart.SuccessMessage?.Text))
                {
                    var formSuccessMessage = await _liquidTemplateManager.RenderStringAsync(formPart.SuccessMessage.Text, _htmlEncoder);
                    successMessage = await _shortcodeService.ProcessAsync(formSuccessMessage);
                }
            }

            return Json(new VueFormSubmitResult(successMessage, serverScriptResult, GetDebugLogs(formPart)));
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
        private Dictionary<string, object> GetDebugLogs(VueForm form)
        {
            if(form.Debug?.Value == true)
            {
                var debugDictionary = HttpContext.Items
                    .Where(x => (x.Key as string)?.StartsWith(Constants.VueFormDebugLog) == true)
                    .ToDictionary(kvp=> ((string)kvp.Key).Replace(Constants.VueFormDebugLog, ""), kvp => kvp.Value);

                var files = Request.Form.Files.Select(f => new {
                    f.FileName,
                    f.Name,
                    f.ContentType,
                    f.Length
                });
                debugDictionary.Add("Request.Files", files);
                return debugDictionary;
            }
            return null;
        }

        private object EvaluateScript(string script,  VueFormMethodsProvider scriptingProvider, VueForm form, string scriptName)
        {
            object result = null;
            if(!string.IsNullOrWhiteSpace(script))
            {
                try
                {
                    result = _scriptingManager.EvaluateJs(script, scriptingProvider);
                }
                catch(Exception ex)
                {
                     _logger.LogError(ex, $"An error occurred evaluating the {scriptName} script");

                    if(form.Debug?.Value == true)
                    {
                        HttpContext.Items.TryAdd($"{Constants.VueFormDebugLog}Script_{scriptName}", ex.ToString());
                    }
                    ModelState.AddModelError("serverErrorMessage", S["A unhandled error occured while executing your request."].ToString());
                }
            }
            return result;
        }
    }

    public class VueFormSubmitResult
    {
        public VueFormSubmitResult(string successMessage, object scriptResult, Dictionary<string, object> debug)
        {
            SuccessMessage = successMessage;
            ServerScriptResult = scriptResult;
            Debug = debug;
        }
        public VueFormSubmitResult(Dictionary<string, string[]> errors, object scriptResult, Dictionary<string, object> debug)
        {
            Errors = errors;
            ValidationError = true;
            ServerScriptResult = scriptResult;
            Debug = debug;
        }
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public string SuccessMessage { get; }
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public object ServerScriptResult { get; }
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public bool ValidationError { get; }
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public Dictionary<string, string[]> Errors { get; }
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public Dictionary<string, object> Debug { get; }
    }
}

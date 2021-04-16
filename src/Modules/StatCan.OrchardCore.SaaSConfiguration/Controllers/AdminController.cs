using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Localization;
using OrchardCore.Deployment.Services;
using OrchardCore.DisplayManagement.Notify;
using OrchardCore.Mvc.Utilities;
using StatCan.OrchardCore.SaaSConfiguration.ViewModels;

namespace StatCan.OrchardCore.SaaSConfiguration.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly INotifier _notifier;
        private readonly IHtmlLocalizer H;
        private readonly IStringLocalizer S;
        private readonly ITenantHelperService _helperService;

        public AdminController(
            IAuthorizationService authorizationService,
            INotifier notifier,
            IHtmlLocalizer<AdminController> htmlLocalizer,
            IStringLocalizer<AdminController> stringLocalizer,
            ITenantHelperService helperService
        )
        {
            _authorizationService = authorizationService;
            _notifier = notifier;
            _helperService = helperService;

            H = htmlLocalizer;
            S = stringLocalizer;
        }

        public async Task<IActionResult> ExecuteRecipe()
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageSaaSConfiguration))
            {
                return Forbid();
            }

            var vm = new ExecuteRecipeViewModel
            {
                AllTenants = _helperService.GetTenantsExceptDefault().Select(t => new SelectListItem(t.Name, t.Name)).ToList()
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> ExecuteRecipe(ExecuteRecipeViewModel model)
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageSaaSConfiguration))
            {
                return Forbid();
            }

            if (!model.Json.IsJson())
            {
                ModelState.AddModelError(nameof(model.Json), S["The recipe is written in an incorrect json format."]);
            }

            if(model.SelectedTenantNames.Count == 0)
            {
                ModelState.AddModelError(nameof(model.SelectedTenantNames), S["Please select at least one tenant to execute the recipe"]);
            }

            if (ModelState.IsValid)
            {
                var tempArchiveFolder = PathExtensions.Combine(Path.GetTempPath(), Path.GetRandomFileName());

                try
                {
                    Directory.CreateDirectory(tempArchiveFolder);
                    System.IO.File.WriteAllText(Path.Combine(tempArchiveFolder, "Recipe.json"), model.Json);
                    // todo: fix this to only apply for selected tenants
                    var tenants = _helperService.GetTenantsExceptDefault();
                    var log = new StringBuilder();

                    await _helperService.ExecuteForTenants(tenants, async (settings, scope) => {
                        var deploymentManager = scope.ServiceProvider.GetRequiredService<IDeploymentManager>();
                        try
                        {
                            await deploymentManager.ImportDeploymentPackageAsync(new PhysicalFileProvider(tempArchiveFolder));
                        }
                        catch(Exception ex)
                        {
                            log.AppendLine(S["An exception occurred while executing the Json for tenant {0}: {1}", settings.Name, ex.Message]);
                        }
                    });
                    if(log.Length > 0)
                    {
                        ModelState.AddModelError("summary", log.ToString());
                    }
                    else
                    {
                        _notifier.Success(H["Recipe successfully executed for selected tenants"]);
                    }
                }
                finally
                {
                    if (Directory.Exists(tempArchiveFolder))
                    {
                        Directory.Delete(tempArchiveFolder, true);
                    }
                }
            }
            model.AllTenants = _helperService.GetTenantsExceptDefault().Select(t=> new SelectListItem(t.Name, t.Name, model.SelectedTenantNames.Contains(t.Name))).ToList();

            return View(model);
        }
    }
}

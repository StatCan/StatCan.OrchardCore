using System;
using System.Threading.Tasks;
using OrchardCore.Entities;
using StatCan.OrchardCore.Matomo.Settings;
using StatCan.OrchardCore.Matomo.ViewModels;
using OrchardCore.Recipes.Models;
using OrchardCore.Recipes.Services;
using OrchardCore.Settings;

namespace StatCan.OrchardCore.Matomo.Recipes
{
    /// <summary>
    /// This recipe step sets Google Analytics settings.
    /// </summary>
    public class MatomoSettingsStep : IRecipeStepHandler
    {
        private readonly ISiteService _siteService;
        public MatomoSettingsStep(ISiteService siteService)
        {
            _siteService = siteService;
        }

        public async Task ExecuteAsync(RecipeExecutionContext context)
        {
            if (!string.Equals(context.Name, nameof(MatomoSettings), StringComparison.OrdinalIgnoreCase))
            {
                return;
            }
            var model = context.Step.ToObject<MatomoSettingsViewModel>();
            var container = await _siteService.LoadSiteSettingsAsync();
            container.Alter<MatomoSettings>(nameof(MatomoSettings), aspect =>
            {
                aspect.SiteID = model.SiteID;
                aspect.ServerUri = model.ServerUri;
            });
            await _siteService.UpdateSiteSettingsAsync(container);
        }
    }
}

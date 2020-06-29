using System;
using System.Threading.Tasks;
using StatCan.OrchardCore.GCCollab.Services;
using StatCan.OrchardCore.GCCollab.Settings;
using OrchardCore.Recipes.Models;
using OrchardCore.Recipes.Services;

namespace StatCan.OrchardCore.GCCollab.Recipes
{
    /// <summary>
    /// This recipe step sets GCCollab Account settings.
    /// </summary>
    public class GCCollabAuthenticationSettingsStep : IRecipeStepHandler
    {
        private readonly IGCCollabAuthenticationService _GCCollabAuthenticationService;

        public GCCollabAuthenticationSettingsStep(IGCCollabAuthenticationService GCCollabLoginService)
        {
            _GCCollabAuthenticationService = GCCollabLoginService;
        }

        public async Task ExecuteAsync(RecipeExecutionContext context)
        {
            if (!string.Equals(context.Name, nameof(GCCollabAuthenticationSettings), StringComparison.OrdinalIgnoreCase))
            {
                return;
            }
            var model = context.Step.ToObject<GCCollabLoginSettingsStepModel>();
            var settings = await _GCCollabAuthenticationService.GetSettingsAsync();
            settings.ClientID = model.ConsumerKey;
            settings.ClientSecret = model.ConsumerSecret;
            settings.CallbackPath = model.CallbackPath;
            await _GCCollabAuthenticationService.UpdateSettingsAsync(settings);
        }
    }

    public class GCCollabLoginSettingsStepModel
    {
        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }
        public string CallbackPath { get; set; }
    }
}
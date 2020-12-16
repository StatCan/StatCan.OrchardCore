using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using OrchardCore.Recipes.Models;
using OrchardCore.Recipes.Services;
using StatCan.OrchardCore.EmailTemplates.Models;
using StatCan.OrchardCore.EmailTemplates.Services;

namespace StatCan.OrchardCore.EmailTemplates.Recipes
{
    /// <summary>
    /// This recipe step creates a set of templates.
    /// </summary>
    public class EmailTemplateStep : IRecipeStepHandler
    {
        private readonly EmailTemplatesManager _templatesManager;

        public EmailTemplateStep(EmailTemplatesManager templatesManager)
        {
            _templatesManager = templatesManager;
        }

        public async Task ExecuteAsync(RecipeExecutionContext context)
        {
            if (!String.Equals(context.Name, "EmailTemplates", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            if (context.Step.Property("EmailTemplates").Value is JObject templates)
            {
                foreach (var property in templates.Properties())
                {
                    var name = property.Name;
                    var value = property.Value.ToObject<EmailTemplate>();

                    await _templatesManager.UpdateTemplateAsync(name, value);
                }
            }
        }
    }
}

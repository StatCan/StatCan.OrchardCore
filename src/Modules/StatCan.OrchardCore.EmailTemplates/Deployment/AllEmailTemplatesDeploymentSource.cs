using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using OrchardCore.Deployment;
using StatCan.OrchardCore.EmailTemplates.Services;

namespace StatCan.OrchardCore.EmailTemplates.Deployment
{
    public class AllEmailTemplatesDeploymentSource : IDeploymentSource
    {
        private readonly EmailTemplatesManager _templatesManager;

        public AllEmailTemplatesDeploymentSource(EmailTemplatesManager templatesManager)
        {
            _templatesManager = templatesManager;
        }

        public async Task ProcessDeploymentStepAsync(DeploymentStep step, DeploymentPlanResult result)
        {
            var allEmailTemplatesStep = step as AllEmailTemplatesDeploymentStep;

            if (allEmailTemplatesStep == null)
            {
                return;
            }

            var templateObjects = new JObject();
            var templates = await _templatesManager.GetEmailTemplatesDocumentAsync();

            foreach (var template in templates.Templates)
            {
                templateObjects[template.Key] = JObject.FromObject(template.Value);
            }

            result.Steps.Add(new JObject(
                new JProperty("id", "EmailTemplates"),
                new JProperty("EmailTemplates", templateObjects)
            ));
        }
    }
}

using OrchardCore.Deployment;

namespace StatCan.OrchardCore.EmailTemplates.Deployment
{
    /// <summary>
    /// Adds templates to a <see cref="DeploymentPlanResult"/>.
    /// </summary>
    public class AllEmailTemplatesDeploymentStep : DeploymentStep
    {
        public AllEmailTemplatesDeploymentStep()
        {
            Name = "AllEmailTemplates";
        }
    }
}

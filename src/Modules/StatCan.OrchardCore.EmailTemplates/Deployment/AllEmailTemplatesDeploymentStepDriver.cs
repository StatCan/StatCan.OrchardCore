using OrchardCore.Deployment;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.Views;

namespace StatCan.OrchardCore.EmailTemplates.Deployment
{
    public class AllTemplatesDeploymentStepDriver : DisplayDriver<DeploymentStep, AllEmailTemplatesDeploymentStep>
    {
        public override IDisplayResult Display(AllEmailTemplatesDeploymentStep step)
        {
            return
                Combine(
                    View("AllEmailTemplatesDeploymentStep_Summary", step).Location("Summary", "Content"),
                    View("AllEmailTemplatesDeploymentStep_Thumbnail", step).Location("Thumbnail", "Content")
                );
        }

        public override IDisplayResult Edit(AllEmailTemplatesDeploymentStep step)
        {
            return View("AllEmailTemplatesDeploymentStep_Edit", step).Location("Content");
        }
    }
}

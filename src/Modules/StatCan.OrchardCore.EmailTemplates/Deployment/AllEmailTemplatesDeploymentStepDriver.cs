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
                    View("AllTemplatesDeploymentStep_Summary", step).Location("Summary", "Content"),
                    View("AllTemplatesDeploymentStep_Thumbnail", step).Location("Thumbnail", "Content")
                );
        }

        public override IDisplayResult Edit(AllEmailTemplatesDeploymentStep step)
        {
            return View("AllTemplatesDeploymentStep_Edit", step).Location("Content");
        }
    }
}

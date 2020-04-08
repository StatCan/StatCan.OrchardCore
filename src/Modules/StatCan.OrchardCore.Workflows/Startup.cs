using Fluid;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using OrchardCore.Workflows.Helpers;
using StatCan.OrchardCore.Workflows.Drivers;
using StatCan.OrchardCore.Workflows.Tasks;
using StatCan.OrchardCore.Workflows.ViewModels;

namespace StatCan.OrchardCore.Workflows
{
    public class Startup : StartupBase
    {
        static Startup()
        {
            TemplateContext.GlobalMemberAccessStrategy.Register<ValidateUserTaskPropertyViewModel>();
        }

        public override void ConfigureServices(IServiceCollection services)
        {
           services.AddActivity<ValidateUserTask, ValidateUserTaskDisplay>();
        }
    }
}
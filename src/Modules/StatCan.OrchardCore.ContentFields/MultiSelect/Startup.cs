using StatCan.OrchardCore.ContentFields.MultiSelect.Drivers;
using StatCan.OrchardCore.ContentFields.MultiSelect.Fields;
using StatCan.OrchardCore.ContentFields.MultiSelect.Settings;
using StatCan.OrchardCore.ContentFields.MultiSelect.ViewModels;
using Fluid;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;

namespace StatCan.OrchardCore.ContentFields.MultiSelect
{
    [Feature("StatCan.OrchardCore.ContentFields.MultiSelect")]
    public class Startup : StartupBase
    {
        public Startup()
        {
            TemplateContext.GlobalMemberAccessStrategy.Register<MultiSelectField>();
            TemplateContext.GlobalMemberAccessStrategy.Register<DisplayMultiSelectFieldViewModel>();
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDataMigration, Migrations>();

            services.AddContentField<MultiSelectField>()
                .UseDisplayDriver<MultiSelectFieldDisplayDriver>();

            services.AddScoped<IContentPartFieldDefinitionDisplayDriver, MultiSelectFieldSettingsDriver>();
        }
    }
}

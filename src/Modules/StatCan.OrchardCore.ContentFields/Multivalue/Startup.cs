using Fluid;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Admin;
using OrchardCore.ContentFields.Indexing;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.Indexing;
using OrchardCore.Modules;

using StatCan.OrchardCore.ContentFields.Multivalue.Drivers;
using StatCan.OrchardCore.ContentFields.Multivalue.Fields;
using StatCan.OrchardCore.ContentFields.Multivalue.Settings;
using StatCan.OrchardCore.ContentFields.Multivalue.ViewModels;


namespace StatCan.OrchardCore.ContentFields.Multivalue
{
    [Feature(Constants.Features.Multivalue)]
    public class Startup : StartupBase
    {
        static Startup()
        {
            // Registering both field types and shape types are necessary as they can
            // be accessed from inner properties.

            TemplateContext.GlobalMemberAccessStrategy.Register<MultivalueField>();
            TemplateContext.GlobalMemberAccessStrategy.Register<DisplayMultivalueFieldViewModel>();
        }
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddContentField<MultivalueField>()
                .UseDisplayDriver<MultivalueFieldDisplayDriver>();
            // services.AddScoped<IContentPartFieldDefinitionDisplayDriver, TextFieldSettingsDriver>();
            // services.AddScoped<IContentFieldIndexHandler, TextFieldIndexHandler>();
            services.AddScoped<IContentPartFieldDefinitionDisplayDriver, TextFieldHeaderDisplaySettingsDriver>();
            services.AddScoped<IContentPartFieldDefinitionDisplayDriver, MultivalueFieldEditorSettingsDriver>();
        }
    }
}
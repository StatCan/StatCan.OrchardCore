using Fluid;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.Modules;
using StatCan.OrchardCore.ContentFields.MultiValueTextField.Drivers;
using StatCan.OrchardCore.ContentFields.MultiValueTextField.Settings;
using StatCan.OrchardCore.ContentFields.MultiValueTextField.ViewModels;

namespace StatCan.OrchardCore.ContentFields.MultiValueTextField
{
    [Feature(Constants.Features.MultiValueTextField)]
    public class Startup : StartupBase
    {
        static Startup()
        {
            TemplateContext.GlobalMemberAccessStrategy.Register<Fields.MultiValueTextField>();
            TemplateContext.GlobalMemberAccessStrategy.Register<DisplayMultiValueTextFieldViewModel>();
        }
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddContentField<Fields.MultiValueTextField>()
                .UseDisplayDriver<MultiValueTextFieldDisplayDriver>();
            services.AddScoped<IContentPartFieldDefinitionDisplayDriver, MultiValueTextFieldSettingsDriver>();
            services.AddScoped<IContentPartFieldDefinitionDisplayDriver, TextFieldHeaderDisplaySettingsDriver>();
            services.AddScoped<IContentPartFieldDefinitionDisplayDriver, MultiValueTextFieldEditorSettingsDriver>();
        }
    }
}

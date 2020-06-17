using StatCan.OrchardCore.ContentFields.Settings;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using OrchardCore.ContentTypes.Editors;

namespace StatCan.OrchardCore.ContentFields
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IContentPartFieldDefinitionDisplayDriver, TextFieldPredefinedGroupEditorSettingsDriver>();
        }
    }
}
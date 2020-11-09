using StatCan.OrchardCore.ContentFields.Multivalue.Settings;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using OrchardCore.ContentTypes.Editors;

namespace StatCan.OrchardCore.ContentFields.Multivalue
{
    [Feature(Constants.Features.Multivalue)]
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IContentPartFieldDefinitionDisplayDriver, MultivalueFieldEditorSettingsDriver>();
        }
    }
}
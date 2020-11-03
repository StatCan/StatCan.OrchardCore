using Etch.OrchardCore.ContentPermissions.Drivers;
using Etch.OrchardCore.ContentPermissions.Liquid;
using Etch.OrchardCore.ContentPermissions.Models;
using Etch.OrchardCore.ContentPermissions.Services;
using Etch.OrchardCore.ContentPermissions.Settings;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.Data.Migration;
using OrchardCore.Liquid;
using OrchardCore.Modules;

namespace Etch.OrchardCore.ContentPermissions
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IContentPermissionsService, ContentPermissionsService>();
            
            services.AddContentPart<ContentPermissionsPart>().UseDisplayDriver<ContentPermissionsDisplay>();

            services.AddScoped<IContentTypePartDefinitionDisplayDriver, ContentPermissionsPartSettingsDisplayDriver>();

            services.AddLiquidFilter<UserCanViewFilter>("user_can_view");

            services.AddScoped<IDataMigration, Migrations>();
        }
    }
}

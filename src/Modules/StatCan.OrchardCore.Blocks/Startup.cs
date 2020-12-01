using Etch.OrchardCore.Blocks.Drivers;
using Etch.OrchardCore.Blocks.Fields;
using Etch.OrchardCore.Blocks.Models;
using Etch.OrchardCore.Blocks.Parsers;
using Etch.OrchardCore.Blocks.Services;
using Etch.OrchardCore.Blocks.Settings;
using Etch.OrchardCore.Blocks.ViewModels.Blocks;
using Fluid;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;
using System;

namespace Etch.OrchardCore.Blocks
{
    public class Startup : StartupBase
    {
        static Startup()
        {
            TemplateContext.GlobalMemberAccessStrategy.Register<EmbedBlockViewModel>();
            TemplateContext.GlobalMemberAccessStrategy.Register<HeadingBlockViewModel>();
            TemplateContext.GlobalMemberAccessStrategy.Register<ImageBlockViewModel>();
            TemplateContext.GlobalMemberAccessStrategy.Register<ListBlockViewModel>();
            TemplateContext.GlobalMemberAccessStrategy.Register<ParagraphBlockViewModel>();
            TemplateContext.GlobalMemberAccessStrategy.Register<QuoteBlockViewModel>();
            TemplateContext.GlobalMemberAccessStrategy.Register<RawBlockViewModel>();
        }

        public override void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            routes.MapAreaControllerRoute(
                name: "ContentSearch",
                areaName: "StatCan.OrchardCore.Blocks",
                pattern: "Blocks/SearchContentItems",
                defaults: new { controller = "LinkContentAdmin", action = "SearchContentItems" }
            );
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddContentField<BlockField>()
                .UseDisplayDriver<BlockFieldDriver>();

            services.AddScoped<IContentPartFieldDefinitionDisplayDriver, BlockFieldSettingsDriver>();

            services.AddContentPart<BlockBodyPart>()
                .UseDisplayDriver<BlockBodyPartDisplay>();

            services.AddScoped<IContentTypePartDefinitionDisplayDriver, BlockBodyPartSettingsDriver>();

            services.AddScoped<IContentSearchResultsProvider, DefaultContentSearchResultsProvider>();

            services.AddScoped<IDataMigration, Migrations>();
        }
    }
}

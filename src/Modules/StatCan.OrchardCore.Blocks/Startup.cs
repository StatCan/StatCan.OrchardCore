using StatCan.OrchardCore.Blocks.Drivers;
using StatCan.OrchardCore.Blocks.Fields;
using StatCan.OrchardCore.Blocks.Models;
using StatCan.OrchardCore.Blocks.Parsers;
using StatCan.OrchardCore.Blocks.Services;
using StatCan.OrchardCore.Blocks.Settings;
using StatCan.OrchardCore.Blocks.ViewModels;
using StatCan.OrchardCore.Blocks.ViewModels.Blocks;
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

namespace StatCan.OrchardCore.Blocks
{
    public class Startup : StartupBase
    {
        public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
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

            services.Configure<TemplateOptions>(o =>
            {
                o.MemberAccessStrategy.Register<DisplayBlockFieldViewModel>();
                o.MemberAccessStrategy.Register<EmbedBlockViewModel>();
                o.MemberAccessStrategy.Register<HeadingBlockViewModel>();
                o.MemberAccessStrategy.Register<ImageBlockViewModel>();
                o.MemberAccessStrategy.Register<ListBlockViewModel>();
                o.MemberAccessStrategy.Register<ParagraphBlockViewModel>();
                o.MemberAccessStrategy.Register<QuoteBlockViewModel>();
                o.MemberAccessStrategy.Register<RawBlockViewModel>();
            });
        }
    }
}

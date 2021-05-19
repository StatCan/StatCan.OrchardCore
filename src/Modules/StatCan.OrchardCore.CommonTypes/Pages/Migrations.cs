using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Autoroute.Models;
using StatCan.OrchardCore.Extensions;

namespace StatCan.OrchardCore.CommonTypes.Page
{
    public class PageMigrations : DataMigration
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;
        public PageMigrations(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }

        public int Create()
        {
            _contentDefinitionManager.AlterTypeDefinition("Page", type => type
                .DisplayedAs("Page")
                .Creatable()
                .Listable()
                .Draftable()
                .Versionable()
                .Securable()
                .WithPart("LocalizationPart", part => part
                    .WithPosition("0")
                )
                .WithPart("TitlePart", part => part
                    .WithPosition("1")
                )
                .WithPart("AutoroutePart", part => part
                    .WithPosition("2")
                    .WithSettings(new AutoroutePartSettings
                    {
                        AllowCustomPath = true,
                        Pattern = "{{ ContentItem | display_text | slugify }}",
                        ShowHomepageOption = true,
                    })
                )
                .WithFlow("3")
            );
           return 1;
        }
    }
    public class AdditionalPagesMigrations : DataMigration
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;
        public AdditionalPagesMigrations(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }

        public int Create()
        {
            _contentDefinitionManager.AlterTypeDefinition("HtmlPage", type => type
                .DisplayedAs("Html Page")
                .Creatable()
                .Listable()
                .Draftable()
                .Versionable()
                .Securable()
                .WithPart("LocalizationPart", part => part
                    .WithPosition("0")
                )
                .WithPart("TitlePart", part => part
                    .WithPosition("1")
                )
                .WithPart("AutoroutePart", part => part
                    .WithPosition("2")
                    .WithSettings(new AutoroutePartSettings
                    {
                        AllowCustomPath = true,
                        Pattern = "{{ ContentItem | display_text | slugify }}",
                        ShowHomepageOption = true,
                    })
                )
                .WithHtmlBody("3")
                .WithPart("HtmlPage", part => part
                    .WithPosition("4")
                )
            );
            _contentDefinitionManager.AlterTypeDefinition("LiquidPage", type => type
                .DisplayedAs("Liquid Page")
                .Creatable()
                .Listable()
                .Draftable()
                .Versionable()
                .Securable()
                .WithPart("LocalizationPart", part => part
                    .WithPosition("0")
                )
                .WithPart("TitlePart", part => part
                    .WithPosition("1")
                )
                .WithPart("AutoroutePart", part => part
                    .WithPosition("2")
                    .WithSettings(new AutoroutePartSettings
                    {
                        AllowCustomPath = true,
                        Pattern = "{{ ContentItem | display_text | slugify }}",
                        ShowHomepageOption = true,
                    })
                )
                .WithPart("LiquidPart", part => part
                    .WithPosition("3")
                )
                .WithPart("LiquidPage", part => part
                    .WithPosition("4")
                )
            );
            _contentDefinitionManager.AlterTypeDefinition("MarkdownPage", type => type
                .DisplayedAs("Markdown Page")
                .Creatable()
                .Listable()
                .Draftable()
                .Versionable()
                .Securable()
                .WithPart("LocalizationPart", part => part
                    .WithPosition("0")
                )
                .WithPart("TitlePart", part => part
                    .WithPosition("1")
                )
                .WithPart("AutoroutePart", part => part
                    .WithPosition("2")
                    .WithSettings(new AutoroutePartSettings
                    {
                        AllowCustomPath = true,
                        Pattern = "{{ ContentItem | display_text | slugify }}",
                        ShowHomepageOption = true,
                    })
                )
                .WithMarkdownBody("3")
                .WithPart("MarkdownPage", part => part
                    .WithPosition("4")
                )
            );
            return 1;
        }
    }
}

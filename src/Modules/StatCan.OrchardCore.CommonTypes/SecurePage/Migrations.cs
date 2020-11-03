using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Autoroute.Models;
using StatCan.OrchardCore.Extensions;

namespace StatCan.OrchardCore.CommonTypes.SecurePage
{
    public class Migrations : DataMigration
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;
        public Migrations(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }

        public int Create()
        {
            _contentDefinitionManager.AlterPartDefinition("SecurePage", p => p
                .WithDisplayName("Secure Page")
                .WithHtmlField("UnauthorizedHtml", "Unauthorized Html", "Displayed when no redirect is provided and the user is not authorized", "0")
            );
            _contentDefinitionManager.AlterTypeDefinition("SecurePage", type => type
                .DisplayedAs("Secure Page")
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
                .WithContentPermission("4")
                .WithPart("SecurePage", p => p.WithPosition("5"))
            );
            return 1;
        }
    }
}
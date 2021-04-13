using OrchardCore.Autoroute.Models;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Lists.Models;
using OrchardCore.Title.Models;

namespace StatCan.OrchardCore.Tenant
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
            _contentDefinitionManager.AlterTypeDefinition("Tenant", type => type
                .DisplayedAs("Tenant")
                .WithPart("Tenant", part => part
                    .WithPosition("1")
                )
                .WithPart("TitlePart", part => part
                    .WithPosition("0")
                    .WithSettings(new TitlePartSettings
                    {
                        Options = TitlePartOptions.GeneratedHidden,
                        Pattern = "{{ Model.ContentItem.Content.Tenant.Name.Text }}",
                    })
                )
            );

            _contentDefinitionManager.AlterPartDefinition("Tenant", part => part
                .WithField("Poster", field => field
                    .OfType("MediaField")
                    .WithDisplayName("Poster")
                    .WithPosition("0")
                )
                .WithField("Name", field => field
                    .OfType("TextField")
                    .WithDisplayName("Name")
                    .WithPosition("1")
                )
                .WithField("Description", field => field
                    .OfType("TextField")
                    .WithDisplayName("Short Description")
                    .WithPosition("2")
                )
            );

            _contentDefinitionManager.AlterTypeDefinition("TenantPage", type => type
                .DisplayedAs("Tenant Page")
                .Creatable()
                .Listable()
                .Draftable()
                .Versionable()
                .Securable()
                .WithPart("TenantPage", part => part
                    .WithPosition("0")
                )
                .WithPart("ListPart", part => part
                    .WithPosition("2")
                    .WithSettings(new ListPartSettings
                    {
                        PageSize = 10,
                        ContainedContentTypes = new[] { "Tenant" },
                    })
                )
                .WithPart("AutoroutePart", part => part
                    .WithPosition("1")
                    .WithSettings(new AutoroutePartSettings
                    {
                        AllowCustomPath = true,
                    })
                )
            );

            return 1;
        }
    }
}

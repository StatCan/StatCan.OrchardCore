using OrchardCore.Autoroute.Models;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using static Lombiq.HelpfulExtensions.Extensions.ContentTypes.ContentTypes;

namespace Lombiq.HelpfulExtensions.Extensions.ContentTypes
{
    public class Migrations : DataMigration
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;


        public Migrations(IContentDefinitionManager contentDefinitionManager) =>
            _contentDefinitionManager = contentDefinitionManager;


        public int Create()
        {
            _contentDefinitionManager.AlterTypeDefinition(Page, builder => builder
                .Creatable()
                .Securable()
                .Draftable()
                .Listable()
                .Versionable()
                .WithPart("TitlePart", part => part.WithPosition("0"))
                .WithPart("AutoroutePart", part => part
                    .WithPosition("1")
                    .WithSettings(new AutoroutePartSettings
                    {
                        ShowHomepageOption = true,
                        AllowCustomPath = true,
                    })
                )
                .WithPart("FlowPart", part => part.WithPosition("2"))
            );

            return 2;
        }

        public int UpdateFrom1()
        {
            _contentDefinitionManager.AlterTypeDefinition(Page, builder => builder
                .WithPart("TitlePart", part => part.WithPosition("0"))
                .WithPart("AutoroutePart", part => part.WithPosition("1"))
                .WithPart("FlowPart", part => part.WithPosition("2"))
            );

            return 2;
        }
    }
}

using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;

namespace StatCan.OrchardCore.Radar.Migrations
{
    public class ArtifactMigration: DataMigration
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;

        public ArtifactMigration(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }

        public int Create()
        {
            CreateArtifact();

            return 1;
        }

        private void CreateArtifact()
        {
            _contentDefinitionManager.AlterTypeDefinition(Constants.ContentTypes.Aritifact, type => type
                .DisplayedAs("Artifact")
                .Creatable()
                .Listable()
                .Draftable()
                .Versionable()
                .Securable()
                .WithPart("Artifact", part => part
                    .WithPosition("1")
                )
                .WithPart("TitlePart", part => part
                    .WithPosition("0")
                )
);
        }
    }
}

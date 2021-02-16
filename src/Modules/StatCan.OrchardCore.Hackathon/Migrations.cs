using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;

namespace StatCan.OrchardCore.Hackathon
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
            Fip();
            return 1;
        }

        private void Fip()
        {
            _contentDefinitionManager.AlterTypeDefinition("FIP", type => type
                .DisplayedAs("FIP")
                .Stereotype("Widget")
                .WithPart("FIP", part => part
                    .WithPosition("0")
                )
            );

            _contentDefinitionManager.AlterPartDefinition("FIP", part => part
                .WithField("Dark", field => field
                    .OfType("BooleanField")
                    .WithDisplayName("Dark")
                )
            );
        }
    }
}

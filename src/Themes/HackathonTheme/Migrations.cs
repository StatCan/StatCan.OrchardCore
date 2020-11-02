using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;

namespace StatCan.Themes.HackathonTheme
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
            HackathonThemeSettings();
            return 1;
        }

        private void HackathonThemeSettings()
        {
            _contentDefinitionManager.AlterTypeDefinition("HackathonThemeSettings", type => type
                .DisplayedAs("Hackathon Theme Settings")
                .Stereotype("CustomSettings")
                .WithPart("HackathonThemeSettings", part => part
                    .WithPosition("0")
                )
            );

            _contentDefinitionManager.AlterPartDefinition("HackathonThemeSettings", part => part
                .WithField("Logo", field => field
                    .OfType("MediaField")
                    .WithDisplayName("Logo")
                    .WithPosition("0")
                )
            );
        }
    }
}
using Microsoft.Extensions.Logging;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentLocalization.Models;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Flows.Models;
using OrchardCore.Media.Fields;
using OrchardCore.Title.Models;

namespace StatCan.OrchardCore.Hackathon
{
    public class HackathonMigrations : DataMigration
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly ILogger _logger;
        public HackathonMigrations(IContentDefinitionManager contentDefinitionManager, ILogger<HackathonMigrations> logger)
        {
            _contentDefinitionManager = contentDefinitionManager;
            _logger = logger;
        }

        public int Create()
        {
            CreateHackathonPage();
            CreateHackathonCustomSetings();
            CreateWidgets();

            return 1;
        }

        private void CreateHackathonCustomSetings()
        {
            _logger.LogError("Creating");
            _contentDefinitionManager.AlterPartDefinition("HackathonCustomSettings", part => part
                .WithField("StartDate", f => f
                    .OfType(nameof(DateField))
                    .WithDisplayName("Start Date")
                    .WithPosition("0")
                )
                .WithField("EndDate", f => f
                    .OfType(nameof(DateField))
                    .WithDisplayName("End Date")
                    .WithPosition("1")
                )
                .WithField("TeamSize", f => f
                    .OfType(nameof(NumericField))
                    .WithDisplayName("Team Size")
                    .WithEditor("Slider")
                    .WithPosition("2")
                )
                .WithField("Logo", f => f
                    .OfType(nameof(MediaField))
                    .WithDisplayName("Logo")
                    .WithPosition("3")
                )
                .WithField("SiteTitle", f => f
                    .OfType(nameof(TextField))
                    .WithDisplayName("Site Title")
                    .WithPosition("4")
                )
            );

            _contentDefinitionManager.AlterTypeDefinition("HackathonCustomSettings", type => type
                .WithPart("HackathonCustomSettings", p => p.WithPosition("0"))
                .Stereotype("CustomSettings"));
        }

        private void CreateHackathonPage()
        {
            _contentDefinitionManager.AlterPartDefinition("HackathonPage", p => p
                .WithField("Name", f => f
                    .OfType(nameof(TextField))
                    .WithDisplayName("Name")
                    .WithPosition("0")
                )
            );

            _contentDefinitionManager.AlterTypeDefinition("HackathonPage", t => t.Creatable().Listable().Securable().Draftable()              
                .WithPart(nameof(LocalizationPart), p => p.WithPosition("0"))
                .WithPart(nameof(TitlePart), p => p
                    .WithPosition("1")
                    .WithSettings(new TitlePartSettings()
                    {
                        Pattern = "{{ContentItem.Content.HackathonPage.Name.Text}}"
                    })
                )
                .WithPart("HackathonPage", p => p.WithPosition("2"))
                //.WithPart("AutoroutePart", p => p.WithPosition("3").WithSettings(new AutoroutePartSettings()
                //{
                //    Pattern = "{% assign hackathon = ContentItem.Content.HackathonPage.Hackathon.LocalizationSets | localization_set | first %}\r\n{% if hackathon != null %}\r\n{{ hackathon.Content.AutoroutePart.Path }}/{{ContentItem.Content.HackathonPage.Name.Text}}\r\n{% endif %}",
                //    AllowCustomPath = true,
                //    ShowHomepageOption = true
                //}))
                //.WithAdvancedFlowPart("4")

            );
        }

        private void CreateWidgets()
        {
            CreateBasicWidget("HackathonCalendar");

            CreateBasicWidget("Tabs");
            _contentDefinitionManager.AlterTypeDefinition("Tabs", t => t
                .WithPart(nameof(TitlePart), p => p
                    .WithPosition("0")
                )
                .WithPart("Tabs", nameof(BagPart), p => p
                    .WithDisplayName("Tabs")
                    .WithPosition("1")
                    .WithSettings(new BagPartSettings() { ContainedContentTypes = new string[] { "Tab" } })
                )
            );
            _contentDefinitionManager.AlterPartDefinition("Tab", p => p.WithDisplayName("Tab"));
            _contentDefinitionManager.AlterTypeDefinition("Tab", t => t
               .WithPart(nameof(TitlePart), p => p
                    .WithPosition("0")
                )
            );
        }

        private void CreateBasicWidget(string name)
        {
            _contentDefinitionManager.AlterPartDefinition(name, p => p.WithDisplayName(name));
            _contentDefinitionManager.AlterTypeDefinition(name, t => t.Stereotype("Widget")
               .WithPart(name, p => p.WithPosition("0"))
            );
        }
    }
}

using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Recipes.Services;
using OrchardCore.Title.Models;
using StatCan.OrchardCore.Extensions;
using System.Threading.Tasks;

namespace StatCan.OrchardCore.Hackathon
{
    public class HackathonMigrations : DataMigration
    {
        private readonly IRecipeMigrator _recipeMigrator;
        private readonly IContentDefinitionManager _contentDefinitionManager;

        public HackathonMigrations(IRecipeMigrator recipeMigrator, IContentDefinitionManager contentDefinitionManager)
        {
            _recipeMigrator = recipeMigrator;
            _contentDefinitionManager = contentDefinitionManager;
        }

        public async Task<int> CreateAsync()
        {
            CreateHackathonCustomSetings();
            CreateUserProfiles();
            CreateWidgets();
            CreateChallenge();

            await _recipeMigrator.ExecuteAsync("queries.recipe.json", this);
            await _recipeMigrator.ExecuteAsync("roles.recipe.json", this);

            return 1;
        }

        private void CreateHackathonCustomSetings()
        {
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
                .WithNumericField("Capacity", "2", new NumericFieldSettings() { DefaultValue = "0", Hint = "The maximum number of participants. Set to 0 for unlimited" })
            );

            _contentDefinitionManager.AlterTypeDefinition("HackathonCustomSettings", type => type
                .WithPart("HackathonCustomSettings", p => p.WithPosition("0"))
                .Stereotype("CustomSettings"));
        }

        private void CreateUserProfiles()
        {
            _contentDefinitionManager.AlterPartDefinition("ParticipantProfile", part => part
                .WithTextField("FirstName", "First Name", "0")
                .WithTextField("LastName", "Last Name", "1")
                .WithTextField("Email", "Contact Email", "Email", "2")
                .WithField("Language", f => f
                    .OfType(nameof(TextField))
                    .WithDisplayName("Language")
                    .WithPosition("3")
                    .WithEditor("PredefinedList")
                    .WithSettings(new TextFieldPredefinedListEditorSettings()
                    {
                        Editor = 0,
                        DefaultValue = "en",
                        Options = new ListValueOption[] {
                            new ListValueOption(){Name = "English", Value = "en"},
                            new ListValueOption(){Name = "French", Value = "fr"},
                            new ListValueOption(){Name = "Billingual", Value = "both"}
                        }
                    })
                )              
            );

            _contentDefinitionManager.AlterTypeDefinition("ParticipantProfile", type => type
                .WithPart("ParticipantProfile", p => p.WithPosition("0"))
                .Stereotype("CustomUserSettings")
            );
        }

        private void CreateWidgets()
        {
            _contentDefinitionManager.CreateBasicWidget("HackathonCalendar");
            _contentDefinitionManager.CreateBasicWidget("ChallengeListWidget");
        }

        private void CreateChallenge()
        {
            _contentDefinitionManager.AlterPartDefinition("Challenge", p => p
                .WithTextField("Name", "0")
                .WithTextField("ShortDescription", "Short Description", "1")
            );

            _contentDefinitionManager.AlterTypeDefinition("Challenge", t => t.Creatable().Draftable().Listable().Securable()
                .WithTitlePart("0", TitlePartOptions.GeneratedDisabled, "{{ContentItem.Content.Challenge.Name.Text}}")
                .WithPart("Challenge", p => p.WithPosition("1"))
                .WithMarkdownBody("2")
            );
        }
    }
}

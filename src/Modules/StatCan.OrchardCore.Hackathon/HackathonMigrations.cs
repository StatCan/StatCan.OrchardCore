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
            CreateWidgets();
            CreateHackerVolunteers();

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

        private void CreateWidgets()
        {
            _contentDefinitionManager.CreateBasicWidget("HackathonCalendar");
        }
        private void CreateHackerVolunteers()
        {
            _contentDefinitionManager.AlterPartDefinition("ParticipantPart", p => p
                .WithDescription("Fields common to all participants")
                .Attachable()
                .WithTextField("FirstName", "First Name", "0")
                .WithTextField("LastName", "Last Name", "1")
                .WithTextField("Email", "Email", "Email", "2")
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
                .WithTextField("AdministratorNotes", "Administrator Notes", "TextArea", "5")
            );
            _contentDefinitionManager.AlterPartDefinition("Hacker", p => p
                .WithDisplayName("Hacker")
            );

            _contentDefinitionManager.AlterTypeDefinition("Hacker", t => t.Creatable().Listable().Securable()
                .WithTitlePart("0", TitlePartOptions.GeneratedDisabled, "{{ ContentItem.Content.ParticipantPart.LastName.Text }}, {{ ContentItem.Content.ParticipantPart.FirstName.Text }}")
                .WithPart("ParticipantPart", p => p.WithPosition("1"))
                .WithPart("Hacker", p => p.WithPosition("2"))
            );

            _contentDefinitionManager.AlterPartDefinition("Volunteer", p => p
                .WithTextField("VolunteerTypes", "Volunteer Types", "0", new TextFieldSettings() { Hint = "Comma delimited list of volunteer types selected by the volunteer" })
            );

            _contentDefinitionManager.AlterTypeDefinition("Volunteer", t => t.Creatable().Listable().Securable()
                .WithTitlePart("0", TitlePartOptions.GeneratedDisabled, "{{ ContentItem.Content.ParticipantPart.LastName.Text }}, {{ ContentItem.Content.ParticipantPart.FirstName.Text }}")
                .WithPart("Volunteer", p => p.WithPosition("1"))
                .WithPart("ParticipantPart", p => p.WithPosition("2"))
           );
        }
    }
}

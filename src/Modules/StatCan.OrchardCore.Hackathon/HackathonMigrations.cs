using Inno.Hackathons;
using OrchardCore.Autoroute.Models;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentLocalization.Models;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Flows.Models;
using OrchardCore.Media.Fields;
using OrchardCore.Recipes.Services;
using OrchardCore.Title.Models;
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
            CreateHackathonPage();
            CreateHackathonCustomSetings();
            CreateWidgets();
            CreateHackerVolunteers();
            CreateHackathonFormReference();

            await _recipeMigrator.ExecuteAsync("roles.recipe.json", this);
            await _recipeMigrator.ExecuteAsync("queries.recipe.json", this);

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
                .WithField("Logo", f => f
                    .OfType(nameof(MediaField))
                    .WithDisplayName("Logo")
                    .WithPosition("2")
                )
                .WithField("SiteTitle", f => f
                    .OfType(nameof(TextField))
                    .WithDisplayName("Site Title")
                    .WithPosition("3")
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
                .WithPart("AutoroutePart", p => p.WithPosition("3").WithSettings(new AutoroutePartSettings()
                {
                    Pattern = "{% assign hackathon = ContentItem.Content.HackathonPage.Hackathon.LocalizationSets | localization_set | first %}\r\n{% if hackathon != null %}\r\n{{ hackathon.Content.AutoroutePart.Path }}/{{ContentItem.Content.HackathonPage.Name.Text}}\r\n{% endif %}",
                    AllowCustomPath = true,
                    ShowHomepageOption = true
                }))

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
                .WithTextField("AdditionalInformation", "AdditionalInformation", "TextArea", "4")
                .WithTextField("AdministratorNotes", "Administrator Notes", "TextArea", "5")
                .WithSwitchBooleanField("Selected", "Selected", "6", new BooleanFieldSettings()
                {
                    Hint = "Only selected participants can see the dashboard pages",
                    Label = "Is selected",
                    DefaultValue = false
                })
                .WithSwitchBooleanField("Attending", "Attending", "7", new BooleanFieldSettings()
                {
                    Hint = "Field set by the user when confirming attendance",
                    Label = "Is attending",
                    DefaultValue = false
                })
                 .WithSwitchBooleanField("CheckedIn", "Checked-in", "8", new BooleanFieldSettings()
                 {
                     Hint = "The participant is checked in at the door by the volunteers",
                     Label = "Is checked-in",
                     DefaultValue = false
                 })
            );
            _contentDefinitionManager.AlterPartDefinition("Hacker", p => p
                .WithField("Team", f => f
                    .OfType(nameof(ContentPickerField))
                    .WithDisplayName("Team")
                    .WithPosition("0")
                    .WithSettings(new ContentPickerFieldSettings() { DisplayedContentTypes = new string[] { "Team" } })
                )
                .WithCaseField("1")
            );

            _contentDefinitionManager.AlterTypeDefinition("Hacker", t => t.Creatable().Listable().Securable()
                .WithPart(nameof(TitlePart), p => p
                    .WithPosition("0")
                    .WithSettings(new TitlePartSettings() { Options = TitlePartOptions.GeneratedDisabled, Pattern = "{{ ContentItem.Content.ParticipantPart.LastName.Text }}, {{ ContentItem.Content.ParticipantPart.FirstName.Text }}" })
                )
                .WithPart("ParticipantPart", p => p.WithPosition("1"))
                .WithPart("Hacker", p => p.WithPosition("2"))
            );

            _contentDefinitionManager.AlterPartDefinition("Volunteer", p => p
                .WithTextField("VolunteerTypes", "Volunteer Types", "0", new TextFieldSettings() { Hint = "Comma delimited list of volunteer types selected by the volunteer" })
            );

            _contentDefinitionManager.AlterTypeDefinition("Volunteer", t => t.Creatable().Listable().Securable()
               // Name of this part is used as a magic string to associate with the Judge's type
               .WithPart(nameof(TitlePart), p => p
                   .WithPosition("0")
                   .WithSettings(new TitlePartSettings() { Options = TitlePartOptions.GeneratedDisabled, Pattern = "{{ ContentItem.Content.ParticipantPart.LastName.Text }}, {{ ContentItem.Content.ParticipantPart.FirstName.Text }}" })
               )
               .WithPart("Volunteer", p => p.WithPosition("1"))
               .WithPart("ParticipantPart", p => p.WithPosition("2"))
           );
        }

        private void CreateHackathonFormReference()
        {
            _contentDefinitionManager.AlterPartDefinition("HackathonRegistrationFormRef", p => p.Attachable().Reusable()
                .WithDescription("Fields related to rendering a hackathon registration form")
                .WithSwitchBooleanField("Enabled", "Enabled", "0")
                .WithHtmlField("FormHeader", "Form Header", "Html displayed on top of the form", "1")
                .WithField("Form", f => f
                    .OfType(nameof(ContentPickerField))
                    .WithDisplayName("Form")
                    .WithPosition("2")
                    .WithSettings(new ContentPickerFieldSettings()
                    {
                        DisplayedContentTypes = new string[] { "InnoForm" }
                    })
                )
                .WithHtmlField("DisabledHtml", "Disabled html", "Html to display when the form is disabled", "3")
            );

            _contentDefinitionManager.AlterPartDefinition("RegistrationFormReference", p => p
                .WithField("PartName", f => f
                    .OfType(nameof(TextField))
                    .WithDisplayName("Part Name")
                    .WithPosition("0")
                    .WithEditor("PredefinedList")
                    .WithSettings(new TextFieldPredefinedListEditorSettings()
                    {
                        Editor = 0,
                        Options = new ListValueOption[]{
                            new ListValueOption(){Name = "Participant", Value = "ParticipantForm"},
                            new ListValueOption(){Name = "Volunteer", Value = "VolunteerForm"},
                        },
                        DefaultValue = "ParticipantForm"
                    })
                )
            );

            _contentDefinitionManager.AlterTypeDefinition("RegistrationFormReference", t => t
                .Stereotype("Widget")
                .WithPart(nameof(TitlePart), p => p
                    .WithPosition("0")
                    .WithSettings(new TitlePartSettings()
                    {
                        Options = TitlePartOptions.GeneratedHidden,
                        Pattern = "{{ContentItem.Content.RegistrationFormReference.PartName.Text}} Reference"
                    })
                )
                .WithPart("RegistrationFormReference", p => p.WithPosition("1"))

            );
        }
    }
}

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
            CreateSecurePage();
            CreateHackathonCustomSetings();
            CreateWidgets();
            CreateHackerVolunteers();
            CreateHackathonFormReference();
            //CreateCase();

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
                .WithNumericField("Capacity", "2", new NumericFieldSettings() { DefaultValue = "0", Hint = "The maximum number of participants" })
            );

            _contentDefinitionManager.AlterTypeDefinition("HackathonCustomSettings", type => type
                .WithPart("HackathonCustomSettings", p => p.WithPosition("0"))
                .Stereotype("CustomSettings"));
        }

        private void CreateSecurePage()
        {
            _contentDefinitionManager.AlterPartDefinition("SecurePage", p => p.WithDisplayName("Secure Page"));

            _contentDefinitionManager.AlterTypeDefinition("SecurePage", t => t.Creatable().Listable().Securable().Draftable()
                .WithPart(nameof(LocalizationPart), p => p.WithPosition("0"))
                .WithPart(nameof(TitlePart), p => p
                    .WithPosition("1")
                    .WithSettings(new TitlePartSettings()
                    {
                        RenderTitle = false,
                        Options = TitlePartOptions.EditableRequired
                    })
                )
                .WithPart("SecurePage", p => p.WithPosition("2"))
                .WithPart("AutoroutePart", p => p.WithPosition("3").WithSettings(new AutoroutePartSettings()
                {
                    Pattern = "{{ContentItem | display_text | slugify}}",
                    AllowCustomPath = true,
                    ShowHomepageOption = true
                }))
                .WithFlow("4")
                .WithContentPermission("5")
            );
        }

        private void CreateWidgets()
        {
            _contentDefinitionManager.CreateBasicWidget("HackathonCalendar");

            //CreateBasicWidget("Tabs");
            //_contentDefinitionManager.AlterTypeDefinition("Tabs", t => t
            //    .WithPart(nameof(TitlePart), p => p
            //        .WithPosition("0")
            //    )
            //    .WithFlow("1", new string[] { "Tab" })
            //);
            //_contentDefinitionManager.AlterPartDefinition("Tab", p => p.WithDisplayName("Tab"));
            //_contentDefinitionManager.AlterTypeDefinition("Tab", t => t
            //   .WithPart(nameof(TitlePart), p => p
            //        .WithPosition("0")
            //    )
            //   .WithPart("Tab", p => p.WithPosition("1"))
            //   .WithFlow("2")
            //);
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
            );
            _contentDefinitionManager.AlterPartDefinition("Hacker", p => p
                .WithDisplayName("Hacker")
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

        /*private void CreateCase()
        {
            _contentDefinitionManager.AlterPartDefinition("Case", p => p
                .WithHackathonField("0")
                .WithTextField("Name", "1")
                .WithTextField("ShortDescription", "Short description", "2", new TextFieldSettings()
                {
                    Hint = "Short description that appears in case selection dropdowns"
                })
            );

            _contentDefinitionManager.AlterTypeDefinition("Case", t => t.Creatable().Listable().Securable()
                // Name of this part is used as a magic string to associate with the Judge's type
                .WithPart(nameof(TitlePart), p => p
                    .WithPosition("0")
                    .WithSettings(new TitlePartSettings()
                    {
                        Options = TitlePartOptions.GeneratedHidden
                    })
                )
                .WithPart(nameof(LocalizationPart), p => p.WithPosition("1"))
                .WithPart("Case", p => p.WithPosition("2"))
                .WithPart("HtmlBodyPart", p => p.WithPosition("3").WithEditor("Wysiwyg"))
            );
        }*/
    }
}

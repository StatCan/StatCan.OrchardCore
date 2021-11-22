using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Recipes.Services;
using OrchardCore.Title.Models;
using StatCan.OrchardCore.Extensions;
using System.Threading.Tasks;

namespace StatCan.OrchardCore.Candev
{
    public class Migrations : DataMigration
    {
        private readonly IRecipeMigrator _recipeMigrator;
        private readonly IContentDefinitionManager _contentDefinitionManager;

        public Migrations(IRecipeMigrator recipeMigrator, IContentDefinitionManager contentDefinitionManager)
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
            CreateTeam();
            CreateScore();
            CreateNewsletterSubscriber();
            CreateTopic();

            await _recipeMigrator.ExecuteAsync("queries.recipe.json", this);
            await _recipeMigrator.ExecuteAsync("roles.recipe.json", this);
            await _recipeMigrator.ExecuteAsync("role-judge.recipe.json", this);

            return 1;
        }

        private void CreateHackathonCustomSetings()
        {
            _contentDefinitionManager.AlterPartDefinition("HackathonCustomSettings", part => part
                .WithField("StartDate", f => f
                    .OfType(nameof(DateField))
                    .WithDisplayName("Start Date")
                    .WithPosition("1")
                )
                .WithField("EndDate", f => f
                    .OfType(nameof(DateField))
                    .WithDisplayName("End Date")
                    .WithPosition("2")
                )
                .WithNumericField("Capacity", "3", new NumericFieldSettings() { DefaultValue = "0", Hint = "The maximum number of participants. Set to 0 for unlimited" })
                .WithField("Where", field => field
                    .OfType("TextField")
                    .WithDisplayName("Where")
                    .WithPosition("0")
                )
            );

            _contentDefinitionManager.AlterPartDefinition("TeamCustomSettings", part => part
                .WithField("TeamSize", field => field
                    .OfType("NumericField")
                    .WithDisplayName("Team Size")
                    .WithEditor("Slider")
                    .WithPosition("0")
                )
                .WithField("TeamEditable", field => field
                    .OfType("BooleanField")
                    .WithDisplayName("Teams Editable")
                    .WithEditor("Switch")
                    .WithPosition("1")
                    .WithSettings(new BooleanFieldSettings
                    {
                        Label = "Can hackers create / join / leave teams?",
                    })
                )
            );

            _contentDefinitionManager.AlterPartDefinition("JudgingCustomSettings", part => part
                .WithField("JudgingInProgress", field => field
                    .OfType("BooleanField")
                    .WithDisplayName("Judging in progress")
                    .WithEditor("Switch")
                    .WithPosition("9")
                    .WithSettings(new BooleanFieldSettings
                    {
                        Hint = "True when a judging round is active.",
                    })
                )
            );

            _contentDefinitionManager.AlterTypeDefinition("HackathonCustomSettings", type => type
                .DisplayedAs("Hackathon Custom Settings")
                .Stereotype("CustomSettings")
                .WithPart("HackathonCustomSettings", p => p
                    .WithPosition("0")
                )
                .WithPart("TeamCustomSettings", part => part
                    .WithPosition("1")
                )
                .WithPart("JudgingCustomSettings", part => part
                    .WithPosition("2")
                )
            );
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
                .WithField("TermsAndConditions", field => field
                    .OfType("BooleanField")
                    .WithDisplayName("Terms And Conditions")
                    .WithPosition("4")
                )
                .WithField("Comments", field => field
                    .OfType("TextField")
                    .WithDisplayName("Comments")
                    .WithPosition("5")
                )
            );

            _contentDefinitionManager.AlterTypeDefinition("ParticipantProfile", type => type
                .WithPart("ParticipantProfile", p => p.WithPosition("0"))
                .Stereotype("CustomUserSettings")
            );

            _contentDefinitionManager.AlterPartDefinition("Hacker", part => part
                .WithDisplayName("Hacker")
                .WithField("Team", field => field
                    .OfType("ContentPickerField")
                    .WithDisplayName("Team")
                    .WithPosition("0")
                    .WithSettings(new ContentPickerFieldSettings
                    {
                        DisplayedContentTypes = new[] { "Team" },
                    })
                )
                .WithField("School", field => field
                    .OfType("TextField")
                    .WithDisplayName("School")
                    .WithPosition("1")
                )
                .WithField("FieldOfStudy", field => field
                    .OfType("TextField")
                    .WithDisplayName("Field of Study")
                    .WithPosition("2")
                )
                .WithField("ProgramName", field => field
                    .OfType("TextField")
                    .WithDisplayName("Program Name")
                    .WithPosition("3")
                )
                .WithField("ProgramLevel", field => field
                    .OfType("TextField")
                    .WithDisplayName("ProgramLevel")
                    .WithPosition("4")
                )
                .WithField("ProgramYears", field => field
                    .OfType("TextField")
                    .WithDisplayName("ProgramYears")
                    .WithPosition("5")
                )
                .WithField("Adult", field => field
                    .OfType("BooleanField")
                    .WithDisplayName("Adult")
                    .WithPosition("6")
                )
                .WithBooleanField("Attendance", "Attendance", "7")
            );

            _contentDefinitionManager.AlterTypeDefinition("Hacker", type => type
                .DisplayedAs("Hacker")
                .Stereotype("CustomUserSettings")
                .WithPart("Hacker", part => part
                    .WithPosition("0")
                )
            );
        }

        private void CreateWidgets()
        {
            _contentDefinitionManager.CreateBasicWidget("HackathonCalendar");
            _contentDefinitionManager.CreateBasicWidget("ChallengeListWidget");
            _contentDefinitionManager.CreateBasicWidget("ChallengeSubmission");
            _contentDefinitionManager.CreateBasicWidget("TeamDashboardWidget");
            _contentDefinitionManager.CreateBasicWidget("JudgingListWidget");
            _contentDefinitionManager.CreateBasicWidget("ArchivedSolutions");
            _contentDefinitionManager.CreateBasicWidget("UserProfile");
        }

        private void CreateChallenge()
        {
            _contentDefinitionManager.AlterPartDefinition("Challenge", part => part
                .WithField("Title", field => field
                    .OfType("TextField")
                    .WithDisplayName("Title")
                    .WithPosition("0")
                )
                .WithField("DatasetsFiles", field => field
                    .OfType("MediaField")
                    .WithDisplayName("Datasets Files")
                    .WithPosition("6")
                )
                .WithField("BackgroundInformation", field => field
                    .OfType("TextField")
                    .WithDisplayName("Background Information")
                    .WithPosition("7")
                )
                .WithField("ContactPersonName", field => field
                    .OfType("TextField")
                    .WithDisplayName("Contact Person Name")
                    .WithPosition("8")
                )
                .WithField("ContactPersonEmail", field => field
                    .OfType("TextField")
                    .WithDisplayName("Contact Person Email")
                    .WithPosition("9")
                )
                .WithField("TechnicalMentorName", field => field
                    .OfType("TextField")
                    .WithDisplayName("Technical Mentor Name")
                    .WithPosition("10")
                )
                .WithField("TechnicalMentorEmail", field => field
                    .OfType("TextField")
                    .WithDisplayName("Technical Mentor Email")
                    .WithPosition("11")
                )
                .WithField("CaseSpecialistName", field => field
                    .OfType("TextField")
                    .WithDisplayName("Case Specialist Name")
                    .WithPosition("12")
                )
                .WithField("CaseSpecialistEmail", field => field
                    .OfType("TextField")
                    .WithDisplayName("Case Specialist Email")
                    .WithPosition("13")
                )
                .WithField("Comments", field => field
                    .OfType("TextField")
                    .WithDisplayName("Comments")
                    .WithPosition("14")
                )
                .WithField("Statement", field => field
                    .OfType("TextField")
                    .WithDisplayName("Statement")
                    .WithEditor("TextArea")
                    .WithPosition("15")
                )
                .WithField("Datasets", field => field
                    .OfType("TextField")
                    .WithDisplayName("Datasets")
                    .WithEditor("TextArea")
                    .WithPosition("16")
                )
                .WithField("OrganizationNameEn", field => field
                    .OfType("TextField")
                    .WithDisplayName("Organization Name En")
                    .WithPosition("2")
                )
                .WithField("OrganizationAcronymEn", field => field
                    .OfType("TextField")
                    .WithDisplayName("Organization Acronym En")
                    .WithPosition("3")
                )
                .WithField("Keywords", field => field
                    .OfType("TextField")
                    .WithDisplayName("Keywords")
                    .WithPosition("1")
                )
            );

            _contentDefinitionManager.AlterTypeDefinition("Challenge", t => t
                .DisplayedAs("Challenge")
                .Creatable()
                .Listable()
                .Draftable()
                .Securable()
                .WithPart("TitlePart", part => part
                    .WithPosition("0")
                    .WithSettings(new TitlePartSettings
                    {
                        Options = TitlePartOptions.GeneratedHidden,
                        Pattern = "{{ContentItem.Content.Challenge.Title.Text}}",
                    })
                )
                .WithPart("Challenge", part => part
                    .WithPosition("1")
                )
                .WithPart("MarkdownBodyPart", part => part
                    .WithDisplayName("Markdown Body")
                    .WithPosition("2")
                    .WithEditor("Wysiwyg")
                )
                .WithPart("LocalizationPart", part => part
                    .WithPosition("3")
                )
            );

            _contentDefinitionManager.AlterPartDefinition("Volunteer", part => part
                .WithField("Mentor", field => field
                    .OfType("BooleanField")
                    .WithDisplayName("Mentor")
                    .WithPosition("0")
                )
                .WithField("Judge", field => field
                    .OfType("BooleanField")
                    .WithDisplayName("Judge")
                    .WithPosition("7")
                )
                .WithField("WorshopPresenter", field => field
                    .OfType("BooleanField")
                    .WithDisplayName("Worshop Presenter")
                    .WithPosition("2")
                )
                .WithField("TechnicalAdvisor", field => field
                    .OfType("BooleanField")
                    .WithDisplayName("Technical Advisor")
                    .WithPosition("3")
                )
                .WithField("KeynoteSpeaker", field => field
                    .OfType("BooleanField")
                    .WithDisplayName("Keynote Speaker")
                    .WithPosition("4")
                )
                .WithField("Observer", field => field
                    .OfType("BooleanField")
                    .WithDisplayName("Observer")
                    .WithPosition("5")
                )
                .WithField("Organizer", field => field
                    .OfType("BooleanField")
                    .WithDisplayName("Organizer")
                    .WithPosition("6")
                )
                .WithField("Department", field => field
                    .OfType("TextField")
                    .WithDisplayName("Department")
                    .WithPosition("8")
                )
                .WithField("AgencyRepresentative", field => field
                    .OfType("BooleanField")
                    .WithDisplayName("Agency Representative")
                    .WithPosition("1")
                )
            );

            _contentDefinitionManager.AlterTypeDefinition("Volunteer", type => type
                .DisplayedAs("Volunteer")
                .Securable()
                .Stereotype("CustomUserSettings")
                .WithPart("Volunteer", part => part
                    .WithPosition("0")
                )
            );

            _contentDefinitionManager.AlterPartDefinition("JudgeType", part => part
                .Attachable()
                .WithDisplayName("Judge Type")
                .WithDescription("Provides a picker to choose a type of judge")
                .WithField("Type", field => field
                    .OfType("MultiTextField")
                    .WithDisplayName("Type")
                    .WithEditor("Picker")
                    .WithPosition("0")
                    .WithSettings(new MultiTextFieldSettings
                    {
                        Options = new MultiTextFieldValueOption[] { new MultiTextFieldValueOption() {
                            Name = "Technical",
                            Value = "Technical"
                        }, new MultiTextFieldValueOption() {
                            Name = "Subject Matter",
                            Value = "SubjectMatter"
                        } },
                    })
                )
            );

            _contentDefinitionManager.AlterPartDefinition("Judge", part => part
                .WithField("Challenge", field => field
                    .OfType("ContentPickerField")
                    .WithDisplayName("Challenge")
                    .WithPosition("0")
                    .WithSettings(new ContentPickerFieldSettings
                    {
                        DisplayedContentTypes = new[] { "Challenge" },
                    })
                )
            );

            _contentDefinitionManager.AlterTypeDefinition("Judge", type => type
                .DisplayedAs("Judge")
                .Stereotype("CustomUserSettings")
                .WithPart("Judge", part => part
                    .WithPosition("0")
                )
                .WithPart("JudgeType", part => part
                    .WithPosition("1")
                )
            );
        }

        private void CreateTeam()
        {
            _contentDefinitionManager.AlterPartDefinition("TeamSolutionPart", p => p
                .WithDisplayName("Team Solution")
                .WithDescription("Fields related to the team's solution to the challenge")
                .Attachable()
                .WithTextField("Name", "Name of solution", "0")
                .WithTextField("Description", "Solution description", "TextArea", "1")
                .WithTextField("RepositoryUrl", "Solution repository url", "2", new TextFieldSettings() { Hint = "Url of where we can get solution artifacts. Typically a git repository" })
            );

            _contentDefinitionManager.AlterPartDefinition("Team", p => p
                .WithTextField("Name", "Team Name", "0")
                .WithTextField("Description", "Team Description", "TextArea", "1")
                .WithChallengeField("3")
                .WithTeamCaptainField("4")
                .WithField("Topics", field => field
                    .OfType("ContentPickerField")
                    .WithDisplayName("Topics")
                    .WithPosition("2")
                    .WithSettings(new ContentPickerFieldSettings
                    {
                        Multiple = true,
                        DisplayedContentTypes = new[] { "Topic" },
                    })
                )
                .WithField("InTheRunning", field => field
                    .OfType("BooleanField")
                    .WithDisplayName("In The Running")
                    .WithEditor("Switch")
                    .WithPosition("1")
                    .WithSettings(new BooleanFieldSettings
                    {
                        DefaultValue = true,
                    })
                )
            );

            _contentDefinitionManager.AlterTypeDefinition("Team", t => t
                .Creatable().Listable().Securable()
                .WithTitlePart("0", TitlePartOptions.GeneratedDisabled, "{% assign case = ContentItem.Content.Team.Challenge.LocalizationSets | localization_set: 'en' | first %}\r\n{{ case | display_text | slugify }}-{{ContentItem.Content.Team.Name.Text}}-{{ ContentItem.Id }}")
                .WithPart("Team", p => p.WithPosition("1"))
                .WithPart("TeamSolutionPart", p => p.WithPosition("2"))
                .WithPart("EmailTemplatePart", p => p.WithPosition("3"))
            );
        }

        private void CreateScore()
        {
            _contentDefinitionManager.AlterPartDefinition("Score", p => p
                .Attachable()
                .WithNumericField("Score", "0", new NumericFieldSettings() { Required = true, DefaultValue = "0" })
                .WithNumericField("Round", "1", new NumericFieldSettings() { Required = true, DefaultValue = "0" })
                .WithField("Judge", f => f
                    .OfType(nameof(UserPickerField))
                    .WithDisplayName("Judge")
                    .WithPosition("2")
                    .WithSettings(new UserPickerFieldSettings() { DisplayedRoles = new string[] { "Volunteer" } })
                )
                .WithTeamField("3")
            );

            _contentDefinitionManager.AlterTypeDefinition("Score", t => t
                .WithPart("Score", p => p.WithPosition("0"))
                .WithPart("JudgeType", p => p.WithPosition("1"))
            );
        }

        private void CreateNewsletterSubscriber()
        {
            _contentDefinitionManager.AlterPartDefinition("NewsletterSubscriber", part => part
                .WithField("Email", field => field
                    .OfType("TextField")
                    .WithDisplayName("Email")
                    .WithEditor("Email")
                    .WithPosition("0")
                )
                .WithField("Name", field => field
                    .OfType("TextField")
                    .WithDisplayName("Name")
                    .WithPosition("1")
                )
            );

            _contentDefinitionManager.AlterTypeDefinition("NewsletterSubscriber", type => type
                .DisplayedAs("Newsletter Subscriber")
                .Creatable()
                .Draftable()
                .Versionable()
                .Securable()
                .WithPart("NewsletterSubscriber", part => part
                    .WithPosition("0")
                )
            );
        }

        private void CreateTopic()
        {
            _contentDefinitionManager.AlterPartDefinition("Topic", part => part
                .WithField("NameEn", field => field
                    .OfType("TextField")
                    .WithDisplayName("Name")
                    .WithPosition("0")
                )
                .WithField("NameFr", field => field
                    .OfType("TextField")
                    .WithDisplayName("Name")
                    .WithPosition("1")
                )
                .WithField("Challenge", field => field
                    .OfType("ContentPickerField")
                    .WithDisplayName("Challenge")
                    .WithPosition("2")
                    .WithSettings(new ContentPickerFieldSettings
                    {
                        DisplayedContentTypes = new[] { "Challenge" },
                    })
                )
            );

            _contentDefinitionManager.AlterTypeDefinition("Topic", type => type
                .DisplayedAs("Topic")
                .Creatable()
                .Listable()
                .Draftable()
                .Securable()
                .WithPart("Topic", part => part
                    .WithPosition("0")
                )
                .WithPart("TitlePart", part => part
                    .WithPosition("1")
                    .WithSettings(new TitlePartSettings
                    {
                        Options = TitlePartOptions.GeneratedDisabled,
                        Pattern = "{{ContentItem.Content.Topic.NameEn.Text}}"
                    })
                )
            );
        }
    }
}

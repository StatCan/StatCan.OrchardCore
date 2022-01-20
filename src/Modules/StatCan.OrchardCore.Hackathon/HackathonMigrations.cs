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
            CreateSkillsInterestForm();

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

        private void CreateSkillsInterestForm() {
            _contentDefinitionManager.AlterTypeDefinition("SkillsInterest", type => type
                .DisplayedAs("SkillsInterest")
                .Listable()
                .Stereotype("Widget")
                .WithPart("SkillsInterest", part => part
                    .WithPosition("0")
                )
            );

            _contentDefinitionManager.AlterPartDefinition("SkillsInterest", part => part
                .WithField("Consent", field => field
                    .OfType("TextField")
                    .WithDisplayName("Consent")
                    .WithPosition("0")
                )
                .WithField("DegreesCompleted", field => field
                    .OfType("TextField")
                    .WithDisplayName("DegreesCompleted")
                    .WithPosition("2")
                )
                .WithField("PhoneNumber", field => field
                    .OfType("TextField")
                    .WithDisplayName("PhoneNumber")
                    .WithPosition("3")
                )
                .WithField("GcJobInterest", field => field
                    .OfType("TextField")
                    .WithDisplayName("GcJobInterest")
                    .WithPosition("4")
                )
                .WithField("JobType", field => field
                    .OfType("TextField")
                    .WithDisplayName("JobType")
                    .WithPosition("5")
                )
                .WithField("Coop", field => field
                    .OfType("TextField")
                    .WithDisplayName("Coop")
                    .WithPosition("6")
                )
                .WithField("Fswep", field => field
                    .OfType("TextField")
                    .WithDisplayName("Fswep")
                    .WithPosition("7")
                )
                .WithField("Location", field => field
                    .OfType("TextField")
                    .WithDisplayName("Location")
                    .WithPosition("8")
                )
                .WithField("Citizenship", field => field
                    .OfType("TextField")
                    .WithDisplayName("Citizenship")
                    .WithPosition("9")
                )
                .WithField("CitizenshipStatus", field => field
                    .OfType("TextField")
                    .WithDisplayName("CitizenshipStatus")
                    .WithPosition("10")
                )
                .WithField("FieldsOfInterest", field => field
                    .OfType("TextField")
                    .WithDisplayName("FieldsOfInterest")
                    .WithPosition("11")
                )
                .WithField("OtherFieldsOfInterest", field => field
                    .OfType("TextField")
                    .WithDisplayName("OtherFieldsOfInterest")
                    .WithPosition("12")
                )
                .WithField("OtherInterest", field => field
                    .OfType("TextField")
                    .WithDisplayName("OtherInterest")
                    .WithPosition("13")
                )
                .WithField("FieldsOfExpertise", field => field
                    .OfType("TextField")
                    .WithDisplayName("FieldsOfExpertise")
                    .WithPosition("14")
                )
                .WithField("OtherFieldsOfExpertise", field => field
                    .OfType("TextField")
                    .WithDisplayName("OtherFieldsOfExpertise")
                    .WithPosition("15")
                )
                .WithField("OtherExpertise", field => field
                    .OfType("TextField")
                    .WithDisplayName("OtherExpertise")
                    .WithPosition("16")
                )
                .WithField("Sql", field => field
                    .OfType("NumericField")
                    .WithDisplayName("Sql")
                    .WithPosition("17")
                )
                .WithField("Sas", field => field
                    .OfType("NumericField")
                    .WithDisplayName("Sas")
                    .WithPosition("18")
                )
                .WithField("R", field => field
                    .OfType("NumericField")
                    .WithDisplayName("R")
                    .WithPosition("19")
                )
                .WithField("Python", field => field
                    .OfType("NumericField")
                    .WithDisplayName("Python")
                    .WithPosition("20")
                )
                .WithField("Java", field => field
                    .OfType("NumericField")
                    .WithDisplayName("Java")
                    .WithPosition("21")
                )
                .WithField("Perl", field => field
                    .OfType("NumericField")
                    .WithDisplayName("Perl")
                    .WithPosition("22")
                )
                .WithField("Scala", field => field
                    .OfType("NumericField")
                    .WithDisplayName("Scala")
                    .WithPosition("23")
                )
                .WithField("Julia", field => field
                    .OfType("NumericField")
                    .WithDisplayName("Julia")
                    .WithPosition("24")
                )
                .WithField("Matlab", field => field
                    .OfType("NumericField")
                    .WithDisplayName("Matlab")
                    .WithPosition("25")
                )
                .WithField("Stata", field => field
                    .OfType("NumericField")
                    .WithDisplayName("Stata")
                    .WithPosition("26")
                )
                .WithField("PowerBi", field => field
                    .OfType("NumericField")
                    .WithDisplayName("PowerBi")
                    .WithPosition("27")
                )
                .WithField("Qgis", field => field
                    .OfType("NumericField")
                    .WithDisplayName("Qgis")
                    .WithPosition("28")
                )
                .WithField("Argis", field => field
                    .OfType("NumericField")
                    .WithDisplayName("Arcgis")
                    .WithPosition("29")
                )
                .WithField("Grass", field => field
                    .OfType("NumericField")
                    .WithDisplayName("Grass")
                    .WithPosition("30")
                )
                .WithField("CPlusPlus", field => field
                    .OfType("NumericField")
                    .WithDisplayName("CPlusPlus")
                    .WithPosition("31")
                )
                .WithField("CSharp", field => field
                    .OfType("NumericField")
                    .WithDisplayName("CSharp")
                    .WithPosition("32")
                )
                .WithField("Javascript", field => field
                    .OfType("NumericField")
                    .WithDisplayName("Javascript")
                    .WithPosition("33")
                )
                .WithField("Html", field => field
                    .OfType("NumericField")
                    .WithDisplayName("Html")
                    .WithPosition("34")
                )
                .WithField("Css", field => field
                    .OfType("NumericField")
                    .WithDisplayName("Css")
                    .WithPosition("35")
                )
                .WithField("OtherLanguage", field => field
                    .OfType("TextField")
                    .WithDisplayName("OtherLanguage")
                    .WithPosition("36")
                )
                .WithField("OtherLanguageRating", field => field
                    .OfType("NumericField")
                    .WithDisplayName("OtherLanguageRating")
                    .WithPosition("37")
                )
                .WithField("PublicProfilesLink", field => field
                    .OfType("TextField")
                    .WithDisplayName("PublicProfilesLink")
                    .WithPosition("38")
                )
                .WithField("GitHubLink", field => field
                    .OfType("TextField")
                    .WithDisplayName("GitHubLink")
                    .WithPosition("39")
                )
                .WithField("Experience", field => field
                    .OfType("TextField")
                    .WithDisplayName("Experience")
                    .WithPosition("40")
                )
                .WithField("Cv", field => field
                    .OfType("TextField")
                    .WithDisplayName("Cv")
                    .WithEditor("TextArea")
                    .WithPosition("41")
                )
                .WithField("Language", field => field
                    .OfType("TextField")
                    .WithDisplayName("Language")
                    .WithPosition("1")
                )
            );
        }
    }
}

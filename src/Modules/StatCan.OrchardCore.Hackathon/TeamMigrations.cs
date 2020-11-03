using OrchardCore.ContentManagement.Metadata;
using OrchardCore.Data.Migration;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.ContentFields.Settings;
using OrchardCore.Title.Models;
using OrchardCore.ContentFields.Fields;
using OrchardCore.Modules;
using StatCan.OrchardCore.Extensions;

namespace StatCan.OrchardCore.Hackathon
{
    [RequireFeatures("StatCan.OrchardCore.Hackathon.Team")]
    public class TeamMigrations : DataMigration
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;

        public TeamMigrations(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }

        public int Create()
        {
            CreateTeam();
            CreateWidgets();
            CreateTeamCustomSettings();

            _contentDefinitionManager.AlterPartDefinition("Hacker", p => p
                .WithField("Team", f => f
                    .OfType(nameof(ContentPickerField))
                    .WithDisplayName("Team")
                    .WithPosition("0")
                    .WithSettings(new ContentPickerFieldSettings() { DisplayedContentTypes = new string[] { "Team" } })
                )
            );

            return 1;
        }

        private void CreateTeam()
        {
            _contentDefinitionManager.AlterPartDefinition("TeamSubmissionPart", p => p
                .WithDisplayName("Team Submission")
                .WithDescription("Collection of fields related to Team Submission")
                .Attachable()
                .WithTextField("Name", "Submission name", "0", new TextFieldSettings() { Hint = "Comma delimited list of volunteer types selected by the volunteer" })
                .WithTextField("Description", "Submission description", "TextArea", "1")
                .WithTextField("RepositoryUrl", "Submission repository url", "2", new TextFieldSettings() { Hint = "Repository url submitted by the participant." })
                .WithTextField("ForkedRepositoryUrl", "Forked repository url", "3", new TextFieldSettings() { Hint = "System forked url of the repository" })
            );

            _contentDefinitionManager.AlterPartDefinition("Team", p => p
                .WithCaseField("0")
            );

            _contentDefinitionManager.AlterTypeDefinition("Team", t => t
                .Creatable().Listable().Securable()
                // Name of this part is used as a magic string to associate with the Judge's type
                .WithPart(nameof(TitlePart), p => p
                    .WithPosition("0")
                    .WithSettings(new TitlePartSettings() { Options = TitlePartOptions.GeneratedDisabled, Pattern = "{% assign hackathon = ContentItem.Content.Team.Hackathon.LocalizationSets | localization_set: 'en' | first %}\r\n{% assign case = ContentItem.Content.Team.Case.LocalizationSets | localization_set: 'en' | first %}\r\n{{ hackathon | display_text | slugify }}-{{ case | display_text | slugify }}-{{ ContentItem.Id }}" })
                )
                .WithPart("Team", p => p.WithPosition("1"))
                .WithPart("TeamSubmissionPart", p => p.WithPosition("2"))
            );
        }

        private void CreateWidgets()
        {
            _contentDefinitionManager.CreateBasicWidget("TeamDashboardWidget");
            _contentDefinitionManager.AlterPartDefinition("TeamDashboardWidget", t => t
                .WithHtmlField("SoloMessage", "Solo message", "The html displayed when the participant is not part of a team", "0")
                .WithHtmlField("TeamMessage", "Team message", "The html displayed when the participant is part of a team", "1")
            );

            _contentDefinitionManager.CreateBasicWidget("TeamJoinListWidget");

            _contentDefinitionManager.CreateBasicWidget("TeamFlowDashboardWidget");
            _contentDefinitionManager.AlterTypeDefinition("TeamFlowDashboardWidget", t => t
                .WithPart(nameof(TitlePart), p => p
                    .WithPosition("1")
                )
                .WithFlow("2")
            );
            _contentDefinitionManager.AlterPartDefinition("TeamFlowDashboardWidget", t => t
                .WithHtmlField("NoTeamHtml", "Solo message", "The html displayed when the participant is not part of a team", "0")
            );
        }

        private void CreateTeamCustomSettings()
        {
            _contentDefinitionManager.AlterPartDefinition("TeamCustomSettings", part => part
                .WithField("TeamSize", f => f
                    .OfType(nameof(NumericField))
                    .WithDisplayName("Team Size")
                    .WithEditor("Slider")
                    .WithPosition("0")
                )
                .WithSwitchBooleanField("TeamEditable", "Teams Editable", "1",
                    new BooleanFieldSettings() { Label = "Can hackers create / join / leave teams?" }));

            _contentDefinitionManager.AlterTypeDefinition("HackathonCustomSettings", type => type
                .WithPart("TeamCustomSettings", p => p.WithPosition("1")));
        }


    }
}

using OrchardCore.ContentManagement.Metadata;
using OrchardCore.Data.Migration;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.ContentFields.Settings;
using OrchardCore.Title.Models;
using OrchardCore.ContentFields.Fields;
using StatCan.OrchardCore.Extensions;

namespace StatCan.OrchardCore.Hackathon
{
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

            return 1;
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
                .WithChallengeField("2")
                .WithTeamCaptainField("3")
            );

            _contentDefinitionManager.AlterTypeDefinition("Team", t => t
                .Creatable().Listable().Securable()
                .WithTitlePart("0", TitlePartOptions.GeneratedDisabled, "{% assign case = ContentItem.Content.Team.Case.LocalizationSets | localization_set: 'en' | first %}\r\n{{ case | display_text | slugify }}-{{ContentItem.Content.Team.Name.Text}}-{{ ContentItem.Id }}")
                .WithPart("Team", p => p.WithPosition("1"))
                .WithPart("TeamSolutionPart", p => p.WithPosition("2"))
                .WithPart("EmailTemplatePart", p => p.WithPosition("3"))
            );

            _contentDefinitionManager.AlterPartDefinition("Hacker", p => p
                .WithDisplayName("Hacker")
                .WithTeamField("0")
                .WithBooleanField("Attendance", "Attendance", "1")
            );

            _contentDefinitionManager.AlterTypeDefinition("Hacker", type => type
                .WithPart("Hacker", p => p.WithPosition("0"))
                .Stereotype("CustomUserSettings")
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

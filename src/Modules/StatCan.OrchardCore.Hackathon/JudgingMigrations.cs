using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using StatCan.OrchardCore.Extensions;
using System.Threading.Tasks;
using OrchardCore.Recipes.Services;

namespace StatCan.OrchardCore.Hackathon
{
    public class JudgingMigrations : DataMigration
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly IRecipeMigrator _recipeMigrator;

        public JudgingMigrations(IRecipeMigrator recipeMigrator, IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
            _recipeMigrator = recipeMigrator;
        }

        public async Task<int> CreateAsync()
        {
            CreateJudgingCustomSettings();

            // create new Score part
            _contentDefinitionManager.AlterPartDefinition("Score", p => p
                .Attachable()
                .WithNumericField("Score", "0", new NumericFieldSettings() { Required = true, Scale = 1, Minimum =(decimal?)0.0, DefaultValue = "0" })
                .WithTextField("Comment", "1")
                .WithField("Judge", f => f
                    .OfType(nameof(ContentPickerField))
                    .WithDisplayName("Judge")
                    .WithPosition("2")
                    .WithSettings(new ContentPickerFieldSettings() { DisplayedContentTypes = new[] { "Volunteer" }, Required = true })
                )
                .WithTeamField("2")
            );

            _contentDefinitionManager.AlterTypeDefinition("ScoreEntry", t => t
                .WithPart("Score", p => p.WithPosition("0"))
                .WithPart("ScoreEntry", p => p.WithPosition("1"))
            );

            // Judging List Widget
            _contentDefinitionManager.AlterPartDefinition("JudgingListWidget", p => p.WithDisplayName("Judging List Widget"));
            _contentDefinitionManager.AlterTypeDefinition("JudgingListWidget", t => t.Stereotype("Widget")
               .WithPart("JudgingListWidget", p => p.WithPosition("0"))
            );

            _contentDefinitionManager.AlterPartDefinition("JudgingResultWidget", p => p.WithDisplayName("Judging Result Widget"));
            _contentDefinitionManager.AlterTypeDefinition("JudgingResultWidget", t => t.Stereotype("Widget")
                .WithPart("JudgingResultWidget", p => p.WithPosition("0"))
            );

            await _recipeMigrator.ExecuteAsync("role-judge.recipe.json", this);
            return 1;
        }

        private void CreateJudgingCustomSettings()
        {
            _contentDefinitionManager.AlterPartDefinition("JudgingCustomSettings", part => part
                .WithSwitchBooleanField("JudgingInProgress", "Judging in progress", "9",
                    new BooleanFieldSettings()
                    {
                        Hint = "True when a judging round is active."
                    }
                )
            );

            _contentDefinitionManager.AlterTypeDefinition("HackathonCustomSettings", type => type
                .WithPart("JudgingCustomSettings", p => p.WithPosition("2"))
                .Stereotype("CustomSettings"));
        }
    }
}
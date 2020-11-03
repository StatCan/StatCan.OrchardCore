using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Indexing;
using OrchardCore.Flows.Models;
using System.Collections.Generic;
using OrchardCore.Title.Models;
using StatCan.OrchardCore.Hackathon;
using OrchardCore.Modules;
using OrchardCore.Autoroute.Models;
using StatCan.OrchardCore.Extensions;

namespace StatCan.OrchardCore.Hackathon
{
    [RequireFeatures("StatCan.OrchardCore.Hackathon.Judging")]
    public class JudgingMigrations : DataMigration
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;

        public JudgingMigrations(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }

        public int Create()
        {
            CreateJudgingCustomSettings();

            // create new HackathonJudgingFormRef type
            /*_contentDefinitionManager.AlterPartDefinition("HackathonJudgingFormRef", p => p
            .Attachable().Reusable().WithDescription("Reusable part that holds a reference to a Judging form")
                .WithField("Form", f => f
                    .OfType(nameof(ContentPickerField))
                    .WithDisplayName("Form")
                    .WithPosition("0")
                    .WithSettings(new ContentPickerFieldSettings() { DisplayedContentTypes = new[] { "InnoForm" } })
                )
            );*/

            /*var judgingTypes = new List<ListValueOption>
            {
                new ListValueOption() { Name = "Technical", Value = "Technical" },
                new ListValueOption() { Name = "Subject Matter", Value = "SubjectMatter" },
                new ListValueOption() { Name = "Bonus", Value = "Bonus" }
            };*/

            // create new Score part
            _contentDefinitionManager.AlterPartDefinition("Score", p => p
                .Attachable()
                .WithNumericField("Score", "0", new NumericFieldSettings() { Required = true, Scale = 1, Minimum = (decimal?)0.0, DefaultValue = "0" })
                .WithTextField("Comment", "1")
                .WithField("Judge", f => f
                    .OfType(nameof(ContentPickerField))
                    .WithDisplayName("Judge")
                    .WithPosition("2")
                    .WithSettings(new ContentPickerFieldSettings() { DisplayedContentTypes = new[] { "Volunteer" }, Required = true })
                )
                .WithField("Team", f => f
                    .OfType(nameof(ContentPickerField))
                    .WithDisplayName("Team")
                    .WithPosition("3")
                    .WithSettings(new ContentPickerFieldSettings() { DisplayedContentTypes = new[] { "Team" }, Required = true })
                )
            );

            _contentDefinitionManager.AlterPartDefinition("ScoringPage", p => p
                .WithField("VueForm", f => f
                    .OfType(nameof(ContentPickerField))
                    .WithDisplayName("VueForm")
                    .WithPosition("0")
                    .WithSettings(new ContentPickerFieldSettings() { DisplayedContentTypes = new string[] { "VueForm" } })
                )
            );

            _contentDefinitionManager.AlterTypeDefinition("ScoringPage", t => t.Creatable().Listable().Securable().Draftable()
                .WithPart("ScoringPage", p => p.WithPosition("0"))
                .WithPart("AutoroutePart", p => p.WithPosition("1").WithSettings(new AutoroutePartSettings() { Pattern = "ScoringPage"}))
            );



            //.WithNumericField("Round", "0", new NumericFieldSettings() { Required = true })
            /*.WithField("Type", f => f
                .OfType(nameof(TextField))
                .WithPosition("1")
                .WithDisplayName("Type")
                .WithEditor("PredefinedList")
                .WithSettings(new TextFieldPredefinedListEditorSettings() { Options = judgingTypes.ToArray(), DefaultValue = "Technical" })
            )*/

            // modify Hackathon type
            /*_contentDefinitionManager.AlterPartDefinition("Hackathon", p => p
                .WithSwitchBooleanField("HackingConcluded", "Hacking concluded", "8",
                    new BooleanFieldSettings()
                    {
                        Hint = "When hacking is concluded, the judging can start"
                    }
                )
                .WithSwitchBooleanField("JudgingInProgress", "Judging in progress", "9",
                    new BooleanFieldSettings()
                    {
                        Hint = "When a judging round is active."
                    }
                )
                 .WithField("JudgingRound", f => f
                    .OfType(nameof(NumericField))
                    .WithDisplayName("Judging Round")
                    .WithDescription("The current or upcoming judging round.")
                    .WithEditor("Slider")
                    .WithPosition("10")
                   .WithSettings(new NumericFieldSettings() { Required = true, Scale = 0, Minimum = 0.0m, Maximum = 5m, DefaultValue = "0" })
                )
            );*/

            /*_contentDefinitionManager.AlterTypeDefinition("Hackathon", t => t
                // Name of this part is used as a magic string to associate with the Judge's type
                .WithPart("JudgingFormTechnical", "HackathonJudgingFormRef", p => p
                  .WithDisplayName("Technical Judging form reference")
                  .WithPosition("9")
                )
                .WithPart("JudgingFormSubjectMatter", "HackathonJudgingFormRef", p => p
                  .WithDisplayName("Subject matter Judging form reference")
                  .WithPosition("10")
                )
            );*/

            // Add Scores bag to team type
            _contentDefinitionManager.AlterTypeDefinition("ScoreEntry", t => t
                .WithPart("Score", p => p.WithPosition("0"))
                .WithPart("ScoreEntry", p => p.WithPosition("1"))
            );

            /*_contentDefinitionManager.AlterPartDefinition("Team", p => p
                .WithSwitchBooleanField("InTheRunning", "In the running", "3", new BooleanFieldSettings()
                {
                    DefaultValue = true
                })
            );*/

            // Add JudgingEntry
            /*_contentDefinitionManager.AlterPartDefinition("JudgingEntry", p => p
                .WithHackathonField("0")
                .WithField("Judge", f => f
                    .OfType(nameof(ContentPickerField))
                    .WithDisplayName("Judge")
                    .WithPosition("1")
                    .WithSettings(new ContentPickerFieldSettings() { DisplayedContentTypes = new[] { "Volunteer" }, Required = true })
                )
                .WithField("Team", f => f
                    .OfType(nameof(ContentPickerField))
                    .WithDisplayName("Team")
                    .WithPosition("2")
                    .WithSettings(new ContentPickerFieldSettings() { DisplayedContentTypes = new[] { "Team" }, Required = true })
                )
                .WithTextField("FormJson", "3")
            );

            _contentDefinitionManager.AlterTypeDefinition("JudgingEntry", t => t
                .Creatable().Listable().Draftable().Securable()
                .WithPart(nameof(TitlePart), p => p.WithPosition("0").WithSettings(new TitlePartSettings() { Options = TitlePartOptions.GeneratedHidden, Pattern = "JudgingEntry Round {{ContentItem.Content.Score.Round.Value}}" }))
                .WithPart("JudgingEntry", p => p.WithPosition("1"))
                .WithPart("Score", p => p.WithPosition("2"))
            );*/

            // Add VolunteerJudgePart
            /*_contentDefinitionManager.AlterPartDefinition("VolunteerJudge", p => p
                .Attachable().WithDescription("Field related to Judges")
                .WithField("IsJudge", f => f
                    .OfType(nameof(BooleanField))
                    .WithDisplayName("Is Judge")
                    .WithEditor("Switch")
                    .WithPosition("0")
                    .WithSettings(new BooleanFieldSettings() { Hint = "Enable for volunteers that will be judging at the event", Label = "Is Judge" })
                )
                .WithField("Type", f => f
                    .OfType(nameof(TextField))
                    .WithDisplayName("Type")
                    .WithPosition("1")
                    .WithEditor("PredefinedList")
                    .WithSettings(new TextFieldPredefinedListEditorSettings() { Options = judgingTypes.ToArray(), DefaultValue = "Technical" })
                )
                .WithField("AssignedCase", f => f
                    .OfType(nameof(LocalizationSetContentPickerField))
                    .WithDisplayName("Assigned Case")
                    .WithPosition("2")
                    .WithSettings(new LocalizationSetContentPickerFieldSettings()
                    {
                        Hint = "Case assigned to this judge, will be used to assign Judging Entries",
                        DisplayedContentTypes = new string[] { "Case" }
                    })
                )
            );*/

            /*_contentDefinitionManager.AlterTypeDefinition("Volunteer", t => t
                .RemovePart("TitlePart")
                .WithPart("TitlePart", p => p.WithPosition("0").WithSettings(new TitlePartSettings() { Options = TitlePartOptions.GeneratedDisabled, Pattern = "{%- assign case = ContentItem.Content.VolunteerJudge.AssignedCase.LocalizationSets | localization_set: 'en' | first -%}\r\n{%- if ContentItem.Content.VolunteerJudge.IsJudge.Value == true -%}\r\nJudge - \r\n{% if case != null -%}\r\n{{case|display_text}} - \r\n{% endif -%}\r\n{% endif -%}\r\n{{ ContentItem.Content.ParticipantPart.LastName.Text }}, {{ ContentItem.Content.ParticipantPart.FirstName.Text }}" }))
                .WithPart("VolunteerJudge", p => p.WithPosition("3"))
            );*/

            // Judging List Widget
            _contentDefinitionManager.AlterPartDefinition("JudgingListWidget", p => p.WithDisplayName("Judging List Widget"));
            _contentDefinitionManager.AlterTypeDefinition("JudgingListWidget", t => t.Stereotype("Widget")
               .WithPart("JudgingListWidget", p => p.WithPosition("0"))
            );

            _contentDefinitionManager.AlterPartDefinition("JudgingResultWidget", p => p.WithDisplayName("Judging Result Widget"));
            _contentDefinitionManager.AlterTypeDefinition("JudgingResultWidget", t => t.Stereotype("Widget")
                .WithPart("JudgingResultWidget", p => p.WithPosition("0"))
            );

            return 1;
        }

        public int UpdateFrom1()
        {
            // Judging Result Widget
            /*_contentDefinitionManager.AlterPartDefinition("JudgingResultWidget", p => p.WithDisplayName("Judging Result Widget"));
            _contentDefinitionManager.AlterTypeDefinition("JudgingResultWidget", t => t.Stereotype("Widget"));
            */
            // Judging Question 
            /*_contentDefinitionManager.AlterPartDefinition("JudgingQuestion", p => p.WithDisplayName("Judging Question")
                .WithTextField("Question", "Question", "1", new TextFieldSettings()
                {
                    Hint = "The criterion to judge.",
                    Required = true
                })
                .WithField("Type", f => f
                    .OfType(nameof(TextField))
                    .WithDisplayName("ParticipantType")
                    .WithPosition("2")
                    .WithEditor("PredefinedList")
                    .WithSettings(new TextFieldSettings()
                    {
                        Hint = "The type of criterion."
                    })
                    .WithSettings(new TextFieldPredefinedListEditorSettings()
                    {
                        Editor = 0,
                        DefaultValue = "Technical",
                        Options = new ListValueOption[] {
                            new ListValueOption(){Name = "Technical", Value = "Technical"},
                            new ListValueOption(){Name = "Subject Matter", Value = "SubjectMatter"}
                        }
                    })
                )
            );
            _contentDefinitionManager.AlterTypeDefinition("JudgingQuestion", t => t
               .WithPart("JudgingQuestion", p => p.WithPosition("0"))
            );*/

            // Add Judging Questions bag to Hackathon
            /*_contentDefinitionManager.AlterTypeDefinition("Hackathon", t => t
                .WithPart("JudgingQuestions", "BagPart", p => p
                    .WithDisplayName("Judging Questions")
                    .WithDescription("List of criteria to judge.")
                    .WithPosition("11")
                    .WithSettings(new BagPartSettings() { ContainedContentTypes = new string[] { "JudgingQuestion" } })
                )
            );*/
            return 2;
        }

        private void CreateJudgingCustomSettings()
        {
            _contentDefinitionManager.AlterPartDefinition("JudgingCustomSettings", part => part
                .WithSwitchBooleanField("JudgingInProgress", "Judging in progress", "9",
                    new BooleanFieldSettings()
                    {
                        Hint = "When a judging round is active."
                    }
                )
            );

            _contentDefinitionManager.AlterTypeDefinition("HackathonCustomSettings", type => type
                .WithPart("JudgingCustomSettings", p => p.WithPosition("2"))
                .Stereotype("CustomSettings"));
        }
    }
}
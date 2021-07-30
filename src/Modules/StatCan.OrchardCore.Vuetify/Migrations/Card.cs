using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Flows.Models;
using OrchardCore.Modules;
using OrchardCore.Title.Models;
using StatCan.OrchardCore.Extensions;

namespace StatCan.OrchardCore.Vuetify.Migrations
{
    [Feature(Constants.Features.Card)]
    public class CardMigrations : DataMigration
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;
        public CardMigrations(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }

        public int Create()
        {
            VCard();

            return 1;
        }

        private void VCard()
        {
            _contentDefinitionManager.AlterTypeDefinition("VCard", type => type
                .DisplayedAs("VCard")
                .Stereotype("Widget")
                .WithPart("VCard", part => part
                    .WithPosition("1")
                )
                .WithTitlePart("0", TitlePartOptions.GeneratedDisabled, "{{ Model.ContentItem.Content.VCard.Title.Text }}")
                .WithPart("FlowPart", part => part
                    .WithPosition("2")
                )
                .WithPart("Actions", part => part
                    .WithDisplayName("Actions")
                    .WithDescription("Used for placing actions for a card.")
                    .WithPosition("3")
                    .WithSettings(new BagPartSettings
                    {
                        ContainedContentTypes = new[] { "ContentMenuItem", "LinkMenuItem", "Menu" },
                    })
                )
            );

            _contentDefinitionManager.AlterPartDefinition("VCard", part => part
                .WithField("Title", field => field
                    .OfType("TextField")
                    .WithDisplayName("Title")
                    .WithPosition("0")
                )
                .WithField("Subtitle", field => field
                    .OfType("TextField")
                    .WithDisplayName("Subtitle")
                    .WithPosition("1")
                )
            );
        }
    }
}

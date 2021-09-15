using System.Threading.Tasks;

using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Title.Models;
using OrchardCore.Taxonomies.Models;

namespace StatCan.OrchardCore.Radar.Migrations
{
    public class TaxonomyMigrations : DataMigration
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly IContentManager _contentManager;

        public TaxonomyMigrations(IContentDefinitionManager contentDefinitionManager, IContentManager contentManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
            _contentManager = contentManager;
        }

        public async Task<int> CreateAsync()
        {
            Topic();
            ProposalType();
            ProjectType();
            CommunityType();

            await CreateTopicsTaxonomy();
            await CreateProposalTypesTaxonomy();
            await CreateProjectTypesTaxonomy();
            await CreateCommunityTypesTaxonoy();

            return 1;
        }

        private void Topic()
        {
            _contentDefinitionManager.AlterTypeDefinition(Constants.ContentTypes.Topic, type => type
                .DisplayedAs("Topic")
                .Listable()
                .Draftable()
                .Versionable()
                .Securable()
                .WithPart("Topic", part => part
                    .WithPosition("2")
                )
                .WithPart("TitlePart", part => part
                    .WithPosition("0")
                    .WithSettings(new TitlePartSettings
                    {
                        Options = TitlePartOptions.GeneratedHidden,
                        Pattern = "{{ ContentItem.Content.Topic.Name.Text }}",
                    })
                )
            );

            _contentDefinitionManager.AlterPartDefinition(Constants.ContentTypes.Topic, part => part
                .WithField("Name", field => field
                    .OfType("TextField")
                    .WithDisplayName("Name")
                )
                .WithField("Description", field => field
                    .OfType("TextField")
                    .WithDisplayName("Description")
                )
            );
        }

        private void ProposalType()
        {
            _contentDefinitionManager.AlterTypeDefinition(Constants.ContentTypes.ProposalType, type => type
                .DisplayedAs("Proposal Type")
                .Listable()
                .Draftable()
                .Versionable()
                .Securable()
                .WithPart("ProposalType", part => part
                    .WithPosition("1")
                )
                .WithPart("TitlePart", part => part
                    .WithPosition("0")
                    .WithSettings(new TitlePartSettings
                    {
                        Options = TitlePartOptions.GeneratedHidden,
                        Pattern = "{{ ContentItem.Content.ProposalType.Name.Text }}",
                    })
                )
            );

            _contentDefinitionManager.AlterPartDefinition(Constants.ContentTypes.ProposalType, part => part
                .WithField("Name", field => field
                    .OfType("TextField")
                    .WithDisplayName("Name")
                    .WithPosition("0")
                )
                .WithField("Description", field => field
                    .OfType("TextField")
                    .WithDisplayName("Description")
                    .WithPosition("1")
                )
            );
        }

        private void ProjectType()
        {
            _contentDefinitionManager.AlterTypeDefinition("ProjectType", type => type
                .DisplayedAs("Project Type")
                .Listable()
                .Draftable()
                .Versionable()
                .Securable()
                .WithPart("ProjectType", part => part
                    .WithPosition("1")
                )
                .WithPart("TitlePart", part => part
                    .WithPosition("0")
                    .WithSettings(new TitlePartSettings
                    {
                        Options = TitlePartOptions.GeneratedHidden,
                        Pattern = "{{ ContentItem.Content.ProjectType.Name.Text }}",
                    })
                )
            );

            _contentDefinitionManager.AlterPartDefinition("ProjectType", part => part
                .WithField("Name", field => field
                    .OfType("TextField")
                    .WithDisplayName("Name")
                    .WithPosition("0")
                )
                .WithField("Description", field => field
                    .OfType("TextField")
                    .WithDisplayName("Description")
                    .WithPosition("1")
                )
            );
        }

        private void CommunityType()
        {
            _contentDefinitionManager.AlterTypeDefinition(Constants.ContentTypes.CommunityType, type => type
                .DisplayedAs("Community Type")
                .Listable()
                .Draftable()
                .Versionable()
                .Securable()
                .WithPart("CommunityType", part => part
                    .WithPosition("1")
                )
                .WithPart("TitlePart", part => part
                    .WithPosition("0")
                    .WithSettings(new TitlePartSettings
                    {
                        Options = TitlePartOptions.GeneratedHidden,
                        Pattern = "{{ ContentItem.Content.CommunityType.Name.Text }}",
                    })
                )
            );

            _contentDefinitionManager.AlterPartDefinition(Constants.ContentTypes.CommunityType, part => part
                .WithField("Name", field => field
                    .OfType("TextField")
                    .WithDisplayName("Name")
                    .WithPosition("0")
                )
                .WithField("Description", field => field
                    .OfType("TextField")
                    .WithDisplayName("Description")
                    .WithPosition("1")
                )
            );

        }

        private async Task CreateTopicsTaxonomy()
        {
            var topics = await _contentManager.NewAsync("Taxonomy");

            topics.DisplayText = "Topics"; // Instead of TitlePart.Title
            topics.Alter<TaxonomyPart>(part =>
            {
                part.TermContentType = Constants.ContentTypes.Topic;
            });

            await _contentManager.CreateAsync(topics, VersionOptions.Published);
            await _contentManager.PublishAsync(topics);
        }

        private async Task CreateProposalTypesTaxonomy()
        {
            var proposalTypes = await _contentManager.NewAsync("Taxonomy");

            proposalTypes.DisplayText = "Proposal Types"; // Instead of TitlePart.Title
            proposalTypes.Alter<TaxonomyPart>(part =>
            {
                part.TermContentType = Constants.ContentTypes.ProposalType;
            });

            await _contentManager.CreateAsync(proposalTypes, VersionOptions.Published);
            await _contentManager.PublishAsync(proposalTypes);
        }

        private async Task CreateProjectTypesTaxonomy()
        {
            var projectTypes = await _contentManager.NewAsync("Taxonomy");

            projectTypes.DisplayText = "Project Types"; // Instead of TitlePart.Title
            projectTypes.Alter<TaxonomyPart>(part =>
            {
                part.TermContentType = Constants.ContentTypes.ProjectType;
            });

            await _contentManager.CreateAsync(projectTypes, VersionOptions.Published);
            await _contentManager.PublishAsync(projectTypes);
        }

        private async Task CreateCommunityTypesTaxonoy()
        {
            var communityTypes = await _contentManager.NewAsync("Taxonomy");

            communityTypes.DisplayText = "Communities Types"; // Instead of TitlePart.Title
            communityTypes.Alter<TaxonomyPart>(part =>
            {
                part.TermContentType = Constants.ContentTypes.CommunityType;
            });

            await _contentManager.CreateAsync(communityTypes, VersionOptions.Published);
            await _contentManager.PublishAsync(communityTypes);
        }
    }
}

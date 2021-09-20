using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Taxonomies.Models;
using OrchardCore.Title.Models;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.Settings;
using OrchardCore.Taxonomies.Fields;
using OrchardCore.Taxonomies.Settings;
using OrchardCore.Flows.Models;

namespace StatCan.OrchardCore.Radar
{
    public class Migrations : DataMigration
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly IContentManager _contentManager;

        private readonly Dictionary<string, string> _taxonomyIds;

        public Migrations(IContentDefinitionManager contentDefinitionManager, IContentManager contentManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
            _contentManager = contentManager;

            _taxonomyIds = new Dictionary<string, string>();
        }

        public async Task<int> CreateAsync()
        {
            CreateTaxonomies();
            await CreateTaxonomyItems();
            CreateArtifact();
            CreateRadarEntityPart();
            CreateProposal();
            CreateProject();
            CreateEvent();
            CreateCommunity();

            CreateLandingPage();

            return 1;
        }

        private void CreateTaxonomies()
        {
            // Topic
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

            // Proposal Type
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

            // Project Type
            _contentDefinitionManager.AlterTypeDefinition(Constants.ContentTypes.ProjectType, type => type
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

            _contentDefinitionManager.AlterPartDefinition(Constants.ContentTypes.ProjectType, part => part
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

            // Community Type
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

        private async Task CreateTaxonomyItems()
        {
            // Topics
            var topics = await _contentManager.NewAsync("Taxonomy");
            _taxonomyIds.Add("Topics", topics.ContentItemId);

            topics.DisplayText = "Topics"; // Instead of TitlePart.Title
            topics.Alter<TaxonomyPart>(part =>
            {
                part.TermContentType = Constants.ContentTypes.Topic;
            });

            await _contentManager.CreateAsync(topics, VersionOptions.Published);
            await _contentManager.PublishAsync(topics);

            // Proposal Type
            var proposalTypes = await _contentManager.NewAsync("Taxonomy");
            _taxonomyIds.Add("Proposal Types", proposalTypes.ContentItemId);

            proposalTypes.DisplayText = "Proposal Types"; // Instead of TitlePart.Title
            proposalTypes.Alter<TaxonomyPart>(part =>
            {
                part.TermContentType = Constants.ContentTypes.ProposalType;
            });

            await _contentManager.CreateAsync(proposalTypes, VersionOptions.Published);
            await _contentManager.PublishAsync(proposalTypes);

            // Project Type
            var projectTypes = await _contentManager.NewAsync("Taxonomy");
            _taxonomyIds.Add("Project Types", proposalTypes.ContentItemId);

            projectTypes.DisplayText = "Project Types"; // Instead of TitlePart.Title
            projectTypes.Alter<TaxonomyPart>(part =>
            {
                part.TermContentType = Constants.ContentTypes.ProjectType;
            });

            await _contentManager.CreateAsync(projectTypes, VersionOptions.Published);
            await _contentManager.PublishAsync(projectTypes);

            // Community Type
            var communityTypes = await _contentManager.NewAsync("Taxonomy");
            _taxonomyIds.Add("Community Types", communityTypes.ContentItemId);

            communityTypes.DisplayText = "Community Types"; // Instead of TitlePart.Title
            communityTypes.Alter<TaxonomyPart>(part =>
            {
                part.TermContentType = Constants.ContentTypes.CommunityType;
            });

            await _contentManager.CreateAsync(communityTypes, VersionOptions.Published);
            await _contentManager.PublishAsync(communityTypes);
        }

        private void CreateArtifact()
        {
            _contentDefinitionManager.AlterTypeDefinition(Constants.ContentTypes.Aritifact, type => type
                .DisplayedAs("Artifact")
                .Creatable()
                .Listable()
                .Draftable()
                .Versionable()
                .Securable()
                .WithPart("Artifact", part => part
                    .WithPosition("1")
                )
                .WithPart("TitlePart", part => part
                    .WithPosition("0")
                )
            );
        }

        private void CreateRadarEntityPart()
        {
            _contentDefinitionManager.AlterPartDefinition(Constants.ContentTypes.RadarEntity, part => part
                .Attachable()
                .WithDescription("Provides fields for an entity in Radar")
                .WithField("Name", field => field
                    .OfType(nameof(TextField))
                    .WithDisplayName("Name")
                )
                .WithField("Description", field => field
                    .OfType(nameof(TextField))
                    .WithDisplayName("Description")
                )
                .WithField("Topics", field => field
                    .OfType(nameof(TaxonomyField))
                    .WithDisplayName("Topics")
                    .WithEditor("Tags")
                    .WithDisplayMode("Tags")
                    .WithSettings(new TaxonomyFieldSettings
                    {
                        Unique = false,
                        TaxonomyContentItemId = _taxonomyIds.GetValueOrDefault("Topics")
                    })
                    .WithSettings(new TaxonomyFieldTagsEditorSettings
                    {
                        Open = false,
                    })

                )
                .WithField("RelatedEntity", field => field
                    .OfType(nameof(ContentPickerField))
                    .WithDisplayName("Related Entity")
                    .WithSettings(new ContentPickerFieldSettings
                    {
                        Multiple = true,
                        Required = false
                    })
                )
            );
        }

        private void CreateProposal()
        {
            _contentDefinitionManager.AlterTypeDefinition(Constants.ContentTypes.Proposal, type => type
                .DisplayedAs("Proposal")
                .Creatable()
                .Listable()
                .Draftable()
                .Versionable()
                .Securable()
                .WithPart(Constants.ContentTypes.Proposal, part => part
                    .WithPosition("2")
                )
                .WithPart(Constants.ContentTypes.RadarEntity, part => part
                    .WithPosition("1")
                )
                .WithPart("TitlePart", part => part
                    .WithPosition("0")
                    .WithSettings(new TitlePartSettings
                    {
                        Options = TitlePartOptions.GeneratedHidden,
                        Pattern = "{{ ContentItem.Content.RadarEntity.Name.Text }}",
                    })
                )
                .WithPart("ContentPermissionsPart", part => part
                    .WithPosition("3")
                )
                .WithPart("Workspace", part => part
                    .WithDisplayName("Workspace")
                    .WithDescription("Add an Artifact to your workspace of this proposal")
                    .WithPosition("4")
                    .WithSettings(new BagPartSettings
                    {
                        ContainedContentTypes = new[] { Constants.ContentTypes.Aritifact },
                    })
                )
            );

            _contentDefinitionManager.AlterPartDefinition(Constants.ContentTypes.Proposal, part => part
                .WithField("Type", field => field
                    .OfType("TaxonomyField")
                    .WithDisplayName("Type")
                    .WithEditor("Tags")
                    .WithDisplayMode("Tags")
                    .WithPosition("0")
                    .WithSettings(new TaxonomyFieldSettings
                    {
                        Required = true,
                        TaxonomyContentItemId = _taxonomyIds.GetValueOrDefault("Proposal Types"),
                        Unique = true,
                    })
                    .WithSettings(new TaxonomyFieldTagsEditorSettings
                    {
                        Open = false,
                    })
                )
            );
        }

        private void CreateProject()
        {
            // Project Member
            _contentDefinitionManager.AlterTypeDefinition(Constants.ContentTypes.ProjectMember, type => type
                .DisplayedAs("ProjectMember")
                .Creatable()
                .Listable()
                .Draftable()
                .Versionable()
                .Securable()
                .WithPart(Constants.ContentTypes.ProjectMember, part => part
                    .WithPosition("1")
                )
                .WithPart("TitlePart", part => part
                    .WithPosition("0")
                    .WithSettings(new TitlePartSettings
                    {
                        Options = TitlePartOptions.GeneratedHidden,
                        Pattern = @"{% assign users = ContentItem.Content.EventOrganier.UserPicker.UserIds | users_by_id %}
                                    {% for user in users %}
                                        {{ user.UserName }} - {{ user.Email }}
                                    {% endfor %}",
                    })
                )
            );

            _contentDefinitionManager.AlterPartDefinition(Constants.ContentTypes.ProjectMember, part => part
                .WithField("Member", field => field
                    .OfType("UserPickerField")
                    .WithDisplayName("Member")
                    .WithPosition("0")
                    .WithSettings(new UserPickerFieldSettings
                    {
                        Required = true,
                        DisplayAllUsers = true,
                        DisplayedRoles = Array.Empty<string>(),
                    })
                )
            );

            // Project
            _contentDefinitionManager.AlterTypeDefinition(Constants.ContentTypes.Project, type => type
                        .DisplayedAs("Project")
                        .Creatable()
                        .Listable()
                        .Draftable()
                        .Versionable()
                        .Securable()
                        .WithPart(Constants.ContentTypes.Project, part => part
                            .WithPosition("4")
                        )
                        .WithPart(Constants.ContentTypes.RadarEntity, part => part
                            .WithPosition("1")
                        )
                        .WithPart("TitlePart", part => part
                            .WithPosition("0")
                            .WithSettings(new TitlePartSettings
                            {
                                Options = TitlePartOptions.GeneratedHidden,
                                Pattern = "{{ ContentItem.Content.RadarEntity.Name.Text }}",
                            })
                        )
                        .WithPart("ContentPermissionsPart", part => part
                            .WithPosition("5")
                        )
                        .WithPart("Workspace", part => part
                            .WithDisplayName("Workspace")
                            .WithDescription("Add an Artifact to your workspace of this project")
                            .WithPosition("2")
                            .WithSettings(new BagPartSettings
                            {
                                ContainedContentTypes = new[] { Constants.ContentTypes.Aritifact },
                            })
                        )
                        .WithPart(Constants.ContentTypes.ProjectMember, part => part
                            .WithDisplayName("Project Member")
                            .WithDescription("Add a member to this project")
                            .WithPosition("3")
                            .WithSettings(new BagPartSettings
                            {
                                ContainedContentTypes = new[] { Constants.ContentTypes.ProjectMember },
                            })
                        )
                    );

            _contentDefinitionManager.AlterPartDefinition(Constants.ContentTypes.Project, part => part
                .WithField("Type", field => field
                    .OfType("TaxonomyField")
                    .WithEditor("Tags")
                    .WithDisplayMode("Tags")
                    .WithDisplayName("Type")
                    .WithPosition("0")
                    .WithSettings(new TaxonomyFieldSettings
                    {
                        Required = true,
                        TaxonomyContentItemId = _taxonomyIds.GetValueOrDefault("Project Types"),
                        Unique = true,
                    })
                    .WithSettings(new TaxonomyFieldTagsEditorSettings
                    {
                        Open = false,
                    })
                )
            );
        }

        private void CreateEvent()
        {
            // Event Organizer
            _contentDefinitionManager.AlterTypeDefinition(Constants.ContentTypes.EventOrganizer, type => type
                .DisplayedAs("Event Organizer")
                .Creatable()
                .Listable()
                .Draftable()
                .Versionable()
                .Securable()
                .WithPart(Constants.ContentTypes.EventOrganizer, part => part
                    .WithPosition("1")
                )
                .WithPart("TitlePart", part => part
                    .WithPosition("0")
                    .WithSettings(new TitlePartSettings
                    {
                        Options = TitlePartOptions.GeneratedHidden,
                        Pattern = @"{% assign users = ContentItem.Content.EventOrganier.UserPicker.UserIds | users_by_id %}
                                    {% for user in users %}
                                        {{ user.UserName }} - {{ user.Email }}
                                    {% endfor %}",
                    })
                )
            );

            _contentDefinitionManager.AlterPartDefinition(Constants.ContentTypes.EventOrganizer, part => part
                .WithField("Organizer", field => field
                    .OfType("UserPickerField")
                    .WithDisplayName("Organizer")
                    .WithPosition("0")
                    .WithSettings(new UserPickerFieldSettings
                    {
                        Required = true,
                        DisplayAllUsers = true,
                        DisplayedRoles = Array.Empty<string>(),
                    })
                )
                .WithField("Role", field => field
                    .OfType("TextField")
                    .WithDisplayName("Role")
                    .WithPosition("1")
                )
            );

            // Event
            _contentDefinitionManager.AlterTypeDefinition(Constants.ContentTypes.Event, type => type
                .DisplayedAs("Event")
                .Creatable()
                .Listable()
                .Draftable()
                .Versionable()
                .Securable()
                .WithPart(Constants.ContentTypes.Event, part => part
                    .WithPosition("1")
                )
                .WithPart("ContentPermissionsPart", part => part
                    .WithPosition("5")
                )
                .WithPart(Constants.ContentTypes.RadarEntity, part => part
                    .WithPosition("2")
                )
                .WithPart("TitlePart", part => part
                    .WithPosition("0")
                    .WithSettings(new TitlePartSettings
                    {
                        Options = TitlePartOptions.GeneratedDisabled,
                        Pattern = "{{ ContentItem.Content.RadarEntity.Name.Text }}",
                    })
                )
                .WithPart(Constants.ContentTypes.EventOrganizer, part => part
                    .WithDisplayName("Event Organizer")
                    .WithDescription("Event Organizer")
                    .WithPosition("3")
                    .WithSettings(new BagPartSettings
                    {
                        ContainedContentTypes = new[] { Constants.ContentTypes.EventOrganizer },
                    })
                )
                .WithPart("Workspace", part => part
                    .WithDisplayName("Workspace")
                    .WithDescription("Add an Artifact to your workspace of this event")
                    .WithPosition("4")
                    .WithSettings(new BagPartSettings
                    {
                        ContainedContentTypes = new[] { Constants.ContentTypes.Aritifact },
                    })
                )
            );

            _contentDefinitionManager.AlterPartDefinition(Constants.ContentTypes.Event, part => part
                .WithField("Attendees", field => field
                    .OfType("UserPickerField")
                    .WithDisplayName("Attendees")
                    .WithPosition("0")
                )
                .WithField("StartDate", field => field
                    .OfType("DateTimeField")
                    .WithDisplayName("Start Date")
                    .WithPosition("1")
                )
                .WithField("EndDate", field => field
                    .OfType("DateTimeField")
                    .WithDisplayName("End Date")
                    .WithPosition("2")
                )
            );
        }

        private void CreateCommunity()
        {
            // Community Member
            _contentDefinitionManager.AlterTypeDefinition(Constants.ContentTypes.CommunityMember, type => type
                .DisplayedAs("Community Member")
                .Creatable()
                .Listable()
                .Draftable()
                .Versionable()
                .Securable()
                .WithPart(Constants.ContentTypes.CommunityMember, part => part
                    .WithPosition("1")
                )
                .WithPart("TitlePart", part => part
                    .WithPosition("0")
                    .WithSettings(new TitlePartSettings
                    {
                        Options = TitlePartOptions.GeneratedHidden,
                        Pattern = @"{% assign users = ContentItem.Content.EventOrganier.UserPicker.UserIds | users_by_id %}
                                    {% for user in users %}
                                        {{ user.UserName }} - {{ user.Email }}
                                    {% endfor %}",
                    })
                )
            );

            _contentDefinitionManager.AlterPartDefinition(Constants.ContentTypes.CommunityMember, part => part
                .WithField("Member", field => field
                    .OfType("UserPickerField")
                    .WithDisplayName("Member")
                    .WithPosition("0")
                    .WithSettings(new UserPickerFieldSettings
                    {
                        Required = true,
                        DisplayAllUsers = true,
                        DisplayedRoles = Array.Empty<string>(),
                    })
                )
            );

            // Community
            _contentDefinitionManager.AlterTypeDefinition(Constants.ContentTypes.Community, type => type
                .DisplayedAs("Community")
                .Creatable()
                .Listable()
                .Draftable()
                .Versionable()
                .Securable()
                .WithPart(Constants.ContentTypes.Community, part => part
                    .WithPosition("4")
                )
                .WithPart("TitlePart", part => part
                    .WithPosition("0")
                    .WithSettings(new TitlePartSettings
                    {
                        Options = TitlePartOptions.GeneratedHidden,
                        Pattern = "{{ ContentItem.Content.RadarEntity.Name.Text }}",
                    })
                )
                .WithPart(Constants.ContentTypes.RadarEntity, part => part
                    .WithPosition("1")
                )
                .WithPart("ContentPermissionsPart", part => part
                    .WithPosition("5")
                )
                .WithPart("Workspace", part => part
                    .WithDisplayName("Workspace")
                    .WithDescription("Add an Artifact to your workspace of this community")
                    .WithPosition("2")
                    .WithSettings(new BagPartSettings
                    {
                        ContainedContentTypes = new[] { Constants.ContentTypes.Aritifact },
                    })
                )
                .WithPart(Constants.ContentTypes.CommunityMember, part => part
                    .WithDisplayName("Community Member")
                    .WithDescription("Add a member to this community")
                    .WithPosition("3")
                    .WithSettings(new BagPartSettings
                    {
                        ContainedContentTypes = new[] { Constants.ContentTypes.CommunityMember },
                    })
                )
            );

            _contentDefinitionManager.AlterPartDefinition(Constants.ContentTypes.Community, part => part
                .WithField("Type", field => field
                    .OfType("TaxonomyField")
                    .WithDisplayName("Type")
                    .WithEditor("Tags")
                    .WithDisplayMode("Tags")
                    .WithPosition("0")
                    .WithSettings(new TaxonomyFieldSettings
                    {
                        Required = true,
                        TaxonomyContentItemId = _taxonomyIds.GetValueOrDefault("Community Types"),
                        Unique = true,
                    })
                    .WithSettings(new TaxonomyFieldTagsEditorSettings
                    {
                        Open = false,
                    })
                )
            );
        }

        private void CreateLandingPage()
        {
            _contentDefinitionManager.AlterTypeDefinition(Constants.ContentTypes.LandingPage, type => type
                .DisplayedAs("Landing Page")
                .Creatable()
                .Listable()
                .Draftable()
                .Versionable()
                .Securable()
                .WithPart(Constants.ContentTypes.LandingPage, part => part
                    .WithPosition("3")
                )
                .WithPart("AutoroutePart", part => part
                    .WithPosition("2")
                )
                .WithPart("LocalizationPart", part => part
                    .WithPosition("0")
                )
                .WithPart("TitlePart", part => part
                    .WithPosition("1")
                )
                .WithPart("FlowPart", part => part
                    .WithPosition("5")
                )
                .WithPart("HtmlBodyPart", part => part
                    .WithPosition("4")
                )
            );

            _contentDefinitionManager.AlterPartDefinition(Constants.ContentTypes.LandingPage, part => part
                .WithField("Description", field => field
                    .OfType("TextField")
                    .WithDisplayName("Description")
                    .WithPosition("1")
                )
            );
        }
    }
}

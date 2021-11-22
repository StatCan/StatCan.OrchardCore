using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Taxonomies.Models;
using OrchardCore.Title.Models;
using OrchardCore.ContentFields.Settings;
using OrchardCore.Taxonomies.Settings;
using OrchardCore.Flows.Models;
using OrchardCore.Contents.Models;
using OrchardCore.Autoroute.Models;
using OrchardCore.Media.Settings;
using Etch.OrchardCore.ContentPermissions.Models;

namespace StatCan.OrchardCore.Radar.Migrations
{
    public class ContentTypeMigrations : DataMigration
    {

        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly IContentManager _contentManager;

        private readonly Dictionary<string, string> _taxonomyIds;

        public ContentTypeMigrations(IContentDefinitionManager contentDefinitionManager, IContentManager contentManager)
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
            CreateRadarFormPart();
            CreateProposal();
            CreateProject();
            CreateEvent();
            CreateCommunity();

            CreateLandingPage();
            CreateAppBar();
            CreateNavigationDrawer();
            CreateFooter();

            CreateUserProfile();

            return 1;
        }

        private void CreateTaxonomies()
        {
            // Add content permission to Taxonomy
            _contentDefinitionManager.AlterTypeDefinition("Taxonomy", type => type
                .WithPart("ContentPermissionsPart", part => part
                    .WithPosition("5")
                    .WithSettings(new ContentPermissionsPartSettings
                    {
                        RedirectUrl = "not-found"
                    })
                )
            );

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
                .WithPart("AutoroutePart", part => part
                    .WithPosition("2")
                    .WithSettings(new AutoroutePartSettings
                    {
                        Pattern = "{{ ContentItem.ContentItemId }}",
                        ManageContainedItemRoutes = true,
                    })
                )
                .WithPart("ContentPermissionsPart", part => part
                    .WithPosition("4")
                    .WithSettings(new ContentPermissionsPartSettings
                    {
                        RedirectUrl = "not-found",
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
                .WithPart("AutoroutePart", part => part
                    .WithPosition("2")
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
                .WithPart("AutoroutePart", part => part
                    .WithPosition("2")
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
                .WithPart("AutoroutePart", part => part
                    .WithPosition("2")
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
            topics.Alter<AutoroutePart>(part =>
            {
                part.Path = "topics";
                part.RouteContainedItems = true;
            });
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
            proposalTypes.Alter<AutoroutePart>(part =>
            {
                part.Path = "proposal-types";
                part.RouteContainedItems = true;
            });
            proposalTypes.Alter<TaxonomyPart>(part =>
            {
                part.TermContentType = Constants.ContentTypes.ProposalType;
            });

            await _contentManager.CreateAsync(proposalTypes, VersionOptions.Published);
            await _contentManager.PublishAsync(proposalTypes);

            // Project Type
            var projectTypes = await _contentManager.NewAsync("Taxonomy");
            _taxonomyIds.Add("Project Types", projectTypes.ContentItemId);

            projectTypes.DisplayText = "Project Types"; // Instead of TitlePart.Title
            projectTypes.Alter<AutoroutePart>(part =>
            {
                part.Path = "project-types";
                part.RouteContainedItems = true;
            });
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
            communityTypes.Alter<AutoroutePart>(part =>
            {
                part.Path = "community-types";
                part.RouteContainedItems = true;
            });
            communityTypes.Alter<TaxonomyPart>(part =>
            {
                part.TermContentType = Constants.ContentTypes.CommunityType;
            });

            await _contentManager.CreateAsync(communityTypes, VersionOptions.Published);
            await _contentManager.PublishAsync(communityTypes);
        }

        private void CreateArtifact()
        {
            _contentDefinitionManager.AlterTypeDefinition(Constants.ContentTypes.Artifact, type => type
                .DisplayedAs("Artifact")
                .Draftable()
                .Versionable()
                .Securable()
                .WithPart("Artifact", part => part
                    .WithPosition("2")
                )
                .WithPart("TitlePart", part => part
                    .WithPosition("1")
                )
                .WithPart("ContentPermissionsPart", part => part
                    .WithPosition("3")
                    .WithSettings(new ContentPermissionsPartSettings
                    {
                        RedirectUrl = "not-found",
                    })
                )
                .WithPart("AutoroutePart", part => part
                    .WithPosition("2")
                    .WithSettings(new AutoroutePartSettings
                    {
                        Pattern = "{{ ContentItem.ContentItemId }}",
                        ManageContainedItemRoutes = true,
                    })
                )
            );

            _contentDefinitionManager.AlterPartDefinition(Constants.ContentTypes.Artifact, part => part
                .Attachable()
                .WithField("URL", field => field
                    .OfType("TextField")
                    .WithDisplayName("URL")
                )
                .WithField("LocalizationSet", field => field
                    .OfType("TextField")
                    .WithDisplayName("LocalizationSet")
                    .WithPosition("1")
                )
            );
        }

        private void CreateRadarEntityPart()
        {
            _contentDefinitionManager.AlterPartDefinition(Constants.ContentTypes.RadarEntityPart, part => part
                .Attachable()
                .WithDescription("Provides fields for an entity in Radar")
                .WithField("Name", field => field
                    .OfType("TextField")
                    .WithDisplayName("Name")
                )
                .WithField("Description", field => field
                    .OfType("TextField")
                    .WithDisplayName("Description")
                )
                .WithField("Topics", field => field
                    .OfType("TaxonomyField")
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
                    .OfType("LocalizationSetContentPickerField")
                    .WithDisplayName("Related Entity")
                    .WithSettings(new LocalizationSetContentPickerFieldSettings
                    {
                        Multiple = true,
                        Required = false,
                        DisplayedContentTypes = new[] { Constants.ContentTypes.Project, Constants.ContentTypes.Proposal, Constants.ContentTypes.Community, Constants.ContentTypes.Event },
                    })
                )
            );
        }

        private void CreateRadarFormPart()
        {
            _contentDefinitionManager.AlterPartDefinition(Constants.ContentTypes.RadarFormPart, part => part
                .Attachable()
                .WithDescription("Holds the initial value for a form")
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
                .WithPart("LocalizationPart", part => part
                    .WithPosition("0")
                )
                .WithPart(Constants.ContentTypes.Proposal, part => part
                    .WithPosition("2")
                )
                .WithPart(Constants.ContentTypes.RadarEntityPart, part => part
                    .WithPosition("1")
                )
                .WithPart("TitlePart", part => part
                    .WithPosition("0")
                    .WithSettings(new TitlePartSettings
                    {
                        Options = TitlePartOptions.GeneratedHidden,
                        Pattern = "{{ ContentItem.Content.RadarEntityPart.Name.Text }}",
                    })
                )
                .WithPart("ContentPermissionsPart", part => part
                    .WithPosition("3")
                    .WithSettings(new ContentPermissionsPartSettings
                    {
                        RedirectUrl = "not-found",
                    })
                )
                .WithPart("Workspace", "BagPart", part => part
                    .WithDisplayName("Workspace")
                    .WithDescription("Add an Artifact to your workspace of this proposal")
                    .WithPosition("4")
                    .WithSettings(new BagPartSettings
                    {
                        ContainedContentTypes = new[] { Constants.ContentTypes.Artifact },
                    })
                )
                .WithPart("AutoroutePart", part => part
                    .WithPosition("2")
                    .WithSettings(new AutoroutePartSettings
                    {
                        AllowRouteContainedItems = true,
                        Pattern = "{{ \"proposals\" | t | append: \"/\" | append: ContentItem.ContentItemId }}",
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
                .DisplayedAs("Project Member")
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
                        Pattern = @"{% assign user = ContentItem.Content.ProjectMember.Member.UserIds | first | users_by_id %}
                                    {{ user.Properties.UserProfile.UserProfile.FirstName.Text }} {{ user.Properties.UserProfile.UserProfile.LastName.Text }}                                ",
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
                .WithField("Role", field => field
                    .OfType("TextField")
                    .WithDisplayName("Role")
                    .WithPosition("1")
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
                .WithPart("LocalizationPart", part => part
                    .WithPosition("0")
                )
                .WithPart(Constants.ContentTypes.Project, part => part
                    .WithPosition("2")
                )
                .WithPart(Constants.ContentTypes.RadarEntityPart, part => part
                    .WithPosition("1")
                )
                .WithPart("TitlePart", part => part
                    .WithPosition("0")
                    .WithSettings(new TitlePartSettings
                    {
                        Options = TitlePartOptions.GeneratedHidden,
                        Pattern = "{{ ContentItem.Content.RadarEntityPart.Name.Text }}",
                    })
                )
                .WithPart("ContentPermissionsPart", part => part
                    .WithPosition("3")
                    .WithSettings(new ContentPermissionsPartSettings
                    {
                        RedirectUrl = "not-found",
                    })
                )
                .WithPart("Workspace", "BagPart", part => part
                    .WithDisplayName("Workspace")
                    .WithDescription("Add an Artifact to your workspace of this project")
                    .WithPosition("5")
                    .WithSettings(new BagPartSettings
                    {
                        ContainedContentTypes = new[] { Constants.ContentTypes.Artifact },
                    })
                )
                .WithPart(Constants.ContentTypes.ProjectMember, "BagPart", part => part
                    .WithDisplayName("Project Member")
                    .WithDescription("Add a member to this project")
                    .WithPosition("4")
                    .WithSettings(new BagPartSettings
                    {
                        ContainedContentTypes = new[] { Constants.ContentTypes.ProjectMember },
                    })
                )
                .WithPart("AutoroutePart", part => part
                    .WithPosition("2")
                    .WithSettings(new AutoroutePartSettings
                    {
                        AllowRouteContainedItems = true,
                        Pattern = "{{ \"projects\" | t | append: \"/\" | append: ContentItem.ContentItemId }}",
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
                        Pattern = @"{% assign user = ContentItem.Content.EventOrganizer.Organizer.UserIds | first | users_by_id %}
                                    {{ user.Properties.UserProfile.UserProfile.FirstName.Text }} {{ user.Properties.UserProfile.UserProfile.LastName.Text }}",
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
                .WithPart("LocalizationPart", part => part
                    .WithPosition("0")
                )
                .WithPart(Constants.ContentTypes.Event, part => part
                    .WithPosition("2")
                )
                .WithPart("ContentPermissionsPart", part => part
                    .WithPosition("3")
                    .WithSettings(new ContentPermissionsPartSettings
                    {
                        RedirectUrl = "not-found",
                    })
                )
                .WithPart(Constants.ContentTypes.RadarEntityPart, part => part
                    .WithPosition("1")
                )
                .WithPart("TitlePart", part => part
                    .WithPosition("0")
                    .WithSettings(new TitlePartSettings
                    {
                        Options = TitlePartOptions.GeneratedDisabled,
                        Pattern = "{{ ContentItem.Content.RadarEntityPart.Name.Text }}",
                    })
                )
                .WithPart(Constants.ContentTypes.EventOrganizer, "BagPart", part => part
                    .WithDisplayName("Event Organizer")
                    .WithDescription("Event Organizer")
                    .WithPosition("4")
                    .WithSettings(new BagPartSettings
                    {
                        ContainedContentTypes = new[] { Constants.ContentTypes.EventOrganizer },
                    })
                )
                .WithPart("Workspace", "BagPart", part => part
                    .WithDisplayName("Workspace")
                    .WithDescription("Add an Artifact to your workspace of this event")
                    .WithPosition("5")
                    .WithSettings(new BagPartSettings
                    {
                        ContainedContentTypes = new[] { Constants.ContentTypes.Artifact },
                    })
                )
                .WithPart("AutoroutePart", part => part
                    .WithPosition("2")
                    .WithSettings(new AutoroutePartSettings
                    {
                        AllowRouteContainedItems = true,
                        Pattern = "{{ \"events\" | t | append: \"/\" | append: ContentItem.ContentItemId }}",
                    })
                )
            );

            _contentDefinitionManager.AlterPartDefinition(Constants.ContentTypes.Event, part => part
                .WithField("Attendees", field => field
                    .OfType("UserPickerField")
                    .WithDisplayName("Attendees")
                    .WithPosition("0")
                    .WithSettings(new UserPickerFieldSettings
                    {
                        Multiple = true,
                        DisplayAllUsers = true,
                        DisplayedRoles = Array.Empty<string>(),
                    })
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
                        Pattern = @"{% assign user = ContentItem.Content.CommunityMember.Member.UserIds | first | users_by_id %}
                                    {{ user.Properties.UserProfile.UserProfile.FirstName.Text }} {{ user.Properties.UserProfile.UserProfile.LastName.Text }}",
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
                .WithField("Role", field => field
                    .OfType("TextField")
                    .WithDisplayName("Role")
                    .WithPosition("1")
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
                .WithPart("LocalizationPart", part => part
                    .WithPosition("0")
                )
                .WithPart(Constants.ContentTypes.Community, part => part
                    .WithPosition("2")
                )
                .WithPart("TitlePart", part => part
                    .WithPosition("0")
                    .WithSettings(new TitlePartSettings
                    {
                        Options = TitlePartOptions.GeneratedHidden,
                        Pattern = "{{ ContentItem.Content.RadarEntityPart.Name.Text }}",
                    })
                )
                .WithPart(Constants.ContentTypes.RadarEntityPart, part => part
                    .WithPosition("1")
                )
                .WithPart("ContentPermissionsPart", part => part
                    .WithPosition("3")
                    .WithSettings(new ContentPermissionsPartSettings
                    {
                        RedirectUrl = "not-found",
                    })
                )
                .WithPart("Workspace", "BagPart", part => part
                    .WithDisplayName("Workspace")
                    .WithDescription("Add an Artifact to your workspace of this community")
                    .WithPosition("5")
                    .WithSettings(new BagPartSettings
                    {
                        ContainedContentTypes = new[] { Constants.ContentTypes.Artifact },
                    })
                )
                .WithPart(Constants.ContentTypes.CommunityMember, "BagPart", part => part
                    .WithDisplayName("Community Member")
                    .WithDescription("Add a member to this community")
                    .WithPosition("4")
                    .WithSettings(new BagPartSettings
                    {
                        ContainedContentTypes = new[] { Constants.ContentTypes.CommunityMember },
                    })
                )
                .WithPart("AutoroutePart", part => part
                    .WithPosition("2")
                    .WithSettings(new AutoroutePartSettings
                    {
                        AllowRouteContainedItems = true,
                        Pattern = "{{ \"communities\" | t | append: \"/\" | append: ContentItem.ContentItemId }}",
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
            // Entity Card
            _contentDefinitionManager.AlterTypeDefinition(Constants.ContentTypes.EntityCard, type => type
                 .DisplayedAs("Entity Card")
                 .Versionable()
                 .Securable()
                 .WithPart("EntityCard", part => part
                     .WithPosition("2")
                 )
                 .WithPart("LocalizationPart", part => part
                     .WithPosition("0")
                 )
                 .WithPart("TitlePart", part => part
                     .WithPosition("1")
                 )
             );

            _contentDefinitionManager.AlterPartDefinition(Constants.ContentTypes.EntityCard, part => part
                .WithField("Type", field => field
                    .OfType("TextField")
                    .WithDisplayName("Type")
                    .WithPosition("2")
                )
                .WithField("Limit", field => field
                    .OfType("NumericField")
                    .WithDisplayName("Limit")
                    .WithPosition("3")
                )
                .WithField("Caption", field => field
                    .OfType("TextField")
                    .WithDisplayName("Caption")
                    .WithPosition("0")
                )
                .WithField("Icon", field => field
                    .OfType("TextField")
                    .WithDisplayName("Icon")
                    .WithPosition("1")
                )
                .WithField("ButtonText", field => field
                    .OfType("TextField")
                    .WithDisplayName("Button Text")
                    .WithPosition("5")
                )
                .WithField("ButtonLink", field => field
                    .OfType("TextField")
                    .WithDisplayName("Button Link")
                    .WithPosition("4")
                )
            );

            // Trending Card
            _contentDefinitionManager.AlterTypeDefinition(Constants.ContentTypes.TrendingCard, type => type
                .DisplayedAs("Trending Card")
                .Draftable()
                .Versionable()
                .Securable()
                .WithPart("TrendingCard", part => part
                    .WithPosition("2")
                )
                .WithPart("LocalizationPart", part => part
                    .WithPosition("0")
                )
                .WithPart("TitlePart", part => part
                    .WithPosition("1")
                )
            );

            _contentDefinitionManager.AlterPartDefinition(Constants.ContentTypes.TrendingCard, part => part
                .WithField("Caption", field => field
                    .OfType("TextField")
                    .WithDisplayName("Caption")
                    .WithPosition("0")
                )
                .WithField("Type", field => field
                    .OfType("TextField")
                    .WithDisplayName("Type")
                    .WithPosition("2")
                )
                .WithField("Icon", field => field
                    .OfType("TextField")
                    .WithDisplayName("Icon")
                    .WithPosition("1")
                )
                .WithField("ButtonText", field => field
                    .OfType("TextField")
                    .WithDisplayName("Button Text")
                    .WithPosition("3")
                )
            );

            // Header list
            _contentDefinitionManager.AlterTypeDefinition(Constants.ContentTypes.LandingPageHeaderList, type => type
                .DisplayedAs("Landing Page Header List")
                .Draftable()
                .Versionable()
                .WithPart("LandingPageHeaderList", part => part
                    .WithPosition("0")
                )
            );

            _contentDefinitionManager.AlterPartDefinition("LandingPageHeaderList", part => part
                .WithField("Caption", field => field
                    .OfType("TextField")
                    .WithDisplayName("Caption")
                    .WithPosition("0")
                )
            );

            // Footer card
            _contentDefinitionManager.AlterTypeDefinition(Constants.ContentTypes.LandingPageFooterCard, type => type
                 .DisplayedAs("Landing Page Footer Card")
                 .WithSettings(new FullTextAspectSettings
                 {
                     IncludeBodyAspect = false,
                     IncludeDisplayText = false,
                 })
                 .WithPart("LandingPageFooterCard", part => part
                     .WithPosition("1")
                 )
                 .WithPart("TitlePart", part => part
                     .WithPosition("0")
                 )
             );

            _contentDefinitionManager.AlterPartDefinition(Constants.ContentTypes.LandingPageFooterCard, part => part
                .WithField("Icon", field => field
                    .OfType("TextField")
                    .WithDisplayName("Icon")
                    .WithPosition("0")
                    .WithSettings(new TextFieldSettings
                    {
                        Hint = "MD Icon name",
                        Required = true,
                    })
                )
                .WithField("Link", field => field
                    .OfType("TextField")
                    .WithDisplayName("Link")
                    .WithPosition("2")
                    .WithSettings(new TextFieldSettings
                    {
                        Hint = "A link that the card will go to",
                    })
                )
                .WithField("Caption", field => field
                    .OfType("TextField")
                    .WithDisplayName("Caption")
                    .WithPosition("1")
                )
            );


            // Landing Page
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
                .WithPart("Activities", "BagPart", part => part
                    .WithDisplayName("Activities")
                    .WithDescription("Add the activity cards here")
                    .WithPosition("5")
                    .WithSettings(new BagPartSettings
                    {
                        ContainedContentTypes = new[] { Constants.ContentTypes.EntityCard },
                    })
                )
                .WithPart("Trends", "BagPart", part => part
                    .WithDisplayName("Trends")
                    .WithDescription("Add the trend card here")
                    .WithPosition("6")
                    .WithSettings(new BagPartSettings
                    {
                        ContainedContentTypes = new[] { Constants.ContentTypes.TrendingCard },
                    })
                )
                .WithPart("Footer", "BagPart", part => part
                    .WithDisplayName("Footer")
                    .WithDescription("Add cards to the landing page footer")
                    .WithPosition("7")
                    .WithSettings(new BagPartSettings
                    {
                        ContainedContentTypes = new[] { Constants.ContentTypes.LandingPageFooterCard },
                    })
                )
                .WithPart("HeaderList", part => part
                    .WithDisplayName("Header List")
                    .WithDescription("Provides a collection behavior for your content item where you can place other content items.")
                    .WithPosition("4")
                    .WithSettings(new BagPartSettings
                    {
                        ContainedContentTypes = new[] { Constants.ContentTypes.LandingPageHeaderList },
                    })
                )
            );

            _contentDefinitionManager.AlterPartDefinition("LandingPage", part => part
                .WithField("Description", field => field
                    .OfType("TextField")
                    .WithDisplayName("Description")
                    .WithPosition("0")
                )
            );
        }

        private void CreateUserProfile()
        {
            _contentDefinitionManager.AlterTypeDefinition(Constants.ContentTypes.UserProfile, type => type
                .DisplayedAs("User Profile")
                .Versionable()
                .Stereotype("CustomUserSettings")
                .WithPart(Constants.ContentTypes.UserProfile, part => part
                    .WithPosition("0")
                )
            );

            _contentDefinitionManager.AlterPartDefinition(Constants.ContentTypes.UserProfile, part => part
                .WithField("FirstName", field => field
                    .OfType("TextField")
                    .WithDisplayName("First Name")
                    .WithPosition("0")
                )
                .WithField("LastName", field => field
                    .OfType("TextField")
                    .WithDisplayName("Last Name")
                    .WithPosition("1")
                )
            );

        }

        private void CreateAppBar()
        {
            _contentDefinitionManager.AlterTypeDefinition(Constants.ContentTypes.AppBar, type => type
                .DisplayedAs("App Bar")
                .Creatable()
                .Listable()
                .Draftable()
                .Versionable()
                .Securable()
                .Stereotype("Widget")
                .WithPart("AppBar", part => part
                    .WithPosition("1")
                )
                .WithPart("TitlePart", part => part
                    .WithPosition("0")
                )
            );

            _contentDefinitionManager.AlterPartDefinition(Constants.ContentTypes.AppBar, part => part
                .WithField("Height", field => field
                    .OfType("NumericField")
                    .WithDisplayName("Height")
                    .WithPosition("1")
                    .WithSettings(new NumericFieldSettings
                    {
                        Minimum = 0,
                        Maximum = 9999,
                    })
                )
                .WithField("Width", field => field
                    .OfType("NumericField")
                    .WithDisplayName("Width")
                    .WithPosition("2")
                    .WithSettings(new NumericFieldSettings
                    {
                        Minimum = 0,
                        Maximum = 9999,
                    })
                )
                .WithField("Elevation", field => field
                    .OfType("NumericField")
                    .WithDisplayName("Elevation")
                    .WithEditor("Slider")
                    .WithPosition("3")
                    .WithSettings(new NumericFieldSettings
                    {
                        Minimum = 0,
                        Maximum = 24,
                    })
                )
                .WithField("ExtensionHeight", field => field
                    .OfType("NumericField")
                    .WithDisplayName("Extension Height")
                    .WithPosition("4")
                    .WithSettings(new NumericFieldSettings
                    {
                        Minimum = 0,
                        Maximum = 9999,
                        DefaultValue = "48",
                    })
                )
                .WithField("ScrollThreshold", field => field
                    .OfType("NumericField")
                    .WithDisplayName("Scroll Threshold")
                    .WithPosition("5")
                    .WithSettings(new NumericFieldSettings
                    {
                        Minimum = 0,
                        Maximum = 9999,
                    })
                )
                .WithField("Logo", field => field
                    .OfType("MediaField")
                    .WithDisplayName("Logo")
                    .WithSettings(new MediaFieldSettings
                    {
                        Multiple = false,
                    })
                )
            );
        }

        private void CreateNavigationDrawer()
        {
            _contentDefinitionManager.AlterTypeDefinition(Constants.ContentTypes.NavigationDrawer, type => type
                .DisplayedAs("Navigation Drawer")
                .Creatable()
                .Listable()
                .Draftable()
                .Versionable()
                .Securable()
                .Stereotype("Widget")
                .WithPart("NavigationDrawer", part => part
                    .WithPosition("0")
                )
            );

            _contentDefinitionManager.AlterPartDefinition(Constants.ContentTypes.NavigationDrawer, part => part
                .WithField("Color", field => field
                    .OfType("TextField")
                    .WithDisplayName("Color")
                    .WithPosition("0")
                )
                .WithField("MobileBreakpoint", field => field
                    .OfType("NumericField")
                    .WithDisplayName("Mobile Breakpoint")
                    .WithPosition("4")
                    .WithSettings(new NumericFieldSettings
                    {
                        Minimum = 0,
                        Maximum = 9999,
                    })
                )
                .WithField("OverlayColor", field => field
                    .OfType("TextField")
                    .WithDisplayName("Overlay Color")
                    .WithPosition("5")
                )
                .WithField("OverlayOpacity", field => field
                    .OfType("NumericField")
                    .WithDisplayName("Overlay Opacity")
                    .WithEditor("Slider")
                    .WithPosition("6")
                    .WithSettings(new NumericFieldSettings
                    {
                        Scale = 4,
                        Minimum = 0,
                        Maximum = 1,
                    })
                )
                .WithField("Height", field => field
                    .OfType("NumericField")
                    .WithDisplayName("Height")
                    .WithPosition("3")
                )
                .WithField("Width", field => field
                    .OfType("NumericField")
                    .WithDisplayName("Width")
                    .WithPosition("1")
                )
                .WithField("MiniVariantWidth", field => field
                    .OfType("NumericField")
                    .WithDisplayName("Mini Variant Width")
                    .WithPosition("2")
                    .WithSettings(new NumericFieldSettings
                    {
                        DefaultValue = "56",
                    })
                )
            );
        }

        private void CreateFooter()
        {
            _contentDefinitionManager.AlterTypeDefinition(Constants.ContentTypes.Footer, type => type
                .DisplayedAs("Footer")
                .Creatable()
                .Listable()
                .Draftable()
                .Versionable()
                .Securable()
                .Stereotype("Widget")
                .WithPart("Footer", part => part
                    .WithPosition("0")
                )
            );

            _contentDefinitionManager.AlterPartDefinition(Constants.ContentTypes.Footer, part => part
                .WithField("Version", field => field
                    .OfType("TextField")
                    .WithDisplayName("Version")
                    .WithPosition("0")
                )
                .WithField("Caption", field => field
                    .OfType("TextField")
                    .WithDisplayName("Caption")
                    .WithPosition("1")
                )
                .WithField("Logo", field => field
                    .OfType("MediaField")
                    .WithDisplayName("Logo")
                    .WithPosition("2")
                    .WithSettings(new MediaFieldSettings
                    {
                        Multiple = false,
                    })
                )
            );
        }
    }
}

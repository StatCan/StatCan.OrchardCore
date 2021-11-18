using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using OrchardCore.ContentManagement;
using OrchardCore.ContentLocalization;
using OrchardCore.Queries;
using OrchardCore.Shortcodes.Services;
using OrchardCore.Taxonomies.Models;
using StatCan.OrchardCore.Radar.FormModels;

namespace StatCan.OrchardCore.Radar.Services
{
    public class FormValueProvider
    {
        private readonly IContentManager _contentManager;

        private readonly IQueryManager _queryManager;

        private readonly IContentLocalizationManager _contentLocalizationManager;

        private readonly IShortcodeService _shortcodeService;

        public FormValueProvider(IContentManager contentManager, IQueryManager queryManager, IContentLocalizationManager contentLocalizationManager, IShortcodeService shortcodeService)
        {
            _contentManager = contentManager;
            _queryManager = queryManager;
            _contentLocalizationManager = contentLocalizationManager;
            _shortcodeService = shortcodeService;
        }

        public async Task<FormModel> GetInitialValuesAsync(string entityType, string id)
        {
            if (entityType == "topics")
            {
                return await GetTopicInitialValuesAsync(id);
            }
            else if (entityType == "projects")
            {
                return await GetProjectInitialValuesAsync(id);
            }
            else if (entityType == "communities")
            {
                return await GetCommunityInitialValuesAsync(id);
            }
            else if (entityType == "events")
            {
                return await GetEventInitialValuesAsync(id);
            }
            else if (entityType == "proposals")
            {
                return await GetProposalInitialValuesAsync(id);
            }

            return null;
        }

        public async Task<FormModel> GetInitialValuesAsync(string entityType, string parentId, string childId)
        {
            if (entityType == "artifacts")
            {
                return await GetArtifactInitialValuesAsync(parentId, childId);
            }

            return null;
        }

        public async Task<ArtifactModel> GetArtifactInitialValuesAsync(string parentId, string childId)
        {
            var artifactModel = new ArtifactModel();

            artifactModel = new ArtifactModel()
            {
                Id = "",
                Name = "",
                Url = "",
            };

            if (!(string.IsNullOrEmpty(parentId) || string.IsNullOrEmpty(childId)))
            {
                var parentContentItem = await GetLocalizedContentAsync(parentId);

                var artifacts = parentContentItem.Content.Workspace.ContentItems;

                foreach (var artifact in artifacts)
                {
                    if (childId == artifact.ContentItemId.ToString())
                    {
                        artifactModel.Id = artifact.ContentItemId.ToString();
                        artifactModel.Name = artifact.DisplayText.ToString();
                        artifactModel.Url = artifact.Artifact.URL.Text.ToString();

                        return artifactModel;
                    }
                }

                return null;
            }

            return artifactModel;
        }

        private async Task<TopicFormModel> GetTopicInitialValuesAsync(string id)
        {
            // Initialize a new form model
            var topicFormModel = new TopicFormModel();

            topicFormModel = new TopicFormModel()
            {
                Id = "",
                Name = "",
                Description = "",
                Roles = Array.Empty<string>()
            };

            if (!string.IsNullOrEmpty(id))
            {
                // Each topic needs to be retrived from the taxonomy term
                var topicQuery = await _queryManager.GetQueryAsync("AllTaxonomiesSQL");
                var topicResult = await _queryManager.ExecuteQueryAsync(topicQuery, new Dictionary<string, object> { { "type", "Topics" } });

                if (topicResult != null)
                {
                    var topicPart = (topicResult.Items.First() as ContentItem).As<TaxonomyPart>();

                    foreach (var topic in topicPart.Terms)
                    {
                        if (id.Equals(topic.ContentItemId))
                        {
                            topicFormModel.Id = topic.ContentItemId;
                            topicFormModel.Name = await _shortcodeService.ProcessAsync(topic.DisplayText);
                            topicFormModel.Description = await _shortcodeService.ProcessAsync(topic.Content.Topic.Description.Text.ToString());
                            topicFormModel.Roles = topic.Content.ContentPermissionsPart.Roles.ToObject<string[]>();

                            return topicFormModel;
                        }
                    }
                }

                return null; // Here means that a topic with the given id does not exist
            }

            return topicFormModel; // Lack of id means the form is for creation
        }

        private async Task<ProjectFormModel> GetProjectInitialValuesAsync(string id)
        {
            var projectFormModel = new ProjectFormModel
            {
                Id = "",
                Name = "",
                Description = "",
                Roles = Array.Empty<string>(),
                Topics = new LinkedList<IDictionary<string, string>>(),
                Type = new Dictionary<string, string>(),
                ProjectMembers = new LinkedList<IDictionary<string, object>>(),
                RelatedEntities = new LinkedList<string>(),
                PublishStatus = "",
            };

            if (!string.IsNullOrEmpty(id))
            {
                var contentItem = await GetLocalizedContentAsync(id);

                if (contentItem == null)
                {
                    return null;
                }

                await GetValuesFromRadarEntityPartAsync(projectFormModel, contentItem);
                projectFormModel.Type = new Dictionary<string, string>()
                {
                    {"value", contentItem.Content.Project.Type.TermContentItemIds.ToObject<string[]>()[0]},
                    {"label", contentItem.Content.Project.Type.TagNames.ToObject<string[]>()[0]}
                };

                var projectMembers = contentItem.Content.ProjectMember.ContentItems;

                foreach (var member in projectMembers)
                {
                    var user = new Dictionary<string, object>()
                    {
                        {"user", new Dictionary<string, string>()
                        {
                            {"value", member.ProjectMember.Member.UserIds.ToObject<string[]>()[0]},
                            {"label", member.ProjectMember.Member.UserNames.ToObject<string[]>()[0]}
                        }},
                        {"role", await _shortcodeService.ProcessAsync(member.ProjectMember.Role.Text.ToString())}
                    };

                    projectFormModel.ProjectMembers.Add(user);
                }
            }

            return projectFormModel;
        }

        private async Task<CommunityFormModel> GetCommunityInitialValuesAsync(string id)
        {
            var communityFormModel = new CommunityFormModel
            {
                Id = "",
                Name = "",
                Description = "",
                Roles = Array.Empty<string>(),
                Topics = new LinkedList<IDictionary<string, string>>(),
                Type = "",
                RelatedEntities = new LinkedList<string>(),
                CommunityMembers = new LinkedList<IDictionary<string, string>>(),
                PublishStatus = "",
            };

            if (!string.IsNullOrEmpty(id))
            {
                var contentItem = await GetLocalizedContentAsync(id);

                if (contentItem == null)
                {
                    return null;
                }

                await GetValuesFromRadarEntityPartAsync(communityFormModel, contentItem);
                communityFormModel.Type = contentItem.Content.Community.Type.TermContentItemIds.ToObject<string[]>()[0];

                var communityMembers = contentItem.Content.CommunityMember.ContentItems;

                foreach (var member in communityMembers)
                {
                    var user = new Dictionary<string, string>()
                    {
                        {"userId", member.CommunityMember.Member.UserIds.ToObject<string[]>()[0]},
                        {"role", await _shortcodeService.ProcessAsync(member.CommunityMember.Role.Text.ToString())}
                    };

                    communityFormModel.CommunityMembers.Add(user);
                }
            }

            return communityFormModel;
        }

        private async Task<EventFormModel> GetEventInitialValuesAsync(string id)
        {
            var eventFormModel = new EventFormModel
            {
                Id = "",
                Name = "",
                Description = "",
                Roles = Array.Empty<string>(),
                Topics = new LinkedList<IDictionary<string, string>>(),
                StartDate = DateTime.Today,
                EndDate = DateTime.Today,
                Attendees = new LinkedList<string>(),
                EventOrganizers = new LinkedList<IDictionary<string, string>>(),
                RelatedEntities = new LinkedList<string>(),
                PublishStatus = "",
            };

            if (!string.IsNullOrEmpty(id))
            {
                var contentItem = await GetLocalizedContentAsync(id);

                if (contentItem == null)
                {
                    return null;
                }

                await GetValuesFromRadarEntityPartAsync(eventFormModel, contentItem);
                eventFormModel.StartDate = DateTime.Parse(contentItem.Content.Event.StartDate.Value.ToString(), CultureInfo.CurrentCulture);
                eventFormModel.EndDate = DateTime.Parse(contentItem.Content.Event.StartDate.Value.ToString(), CultureInfo.CurrentCulture);
                eventFormModel.Attendees = contentItem.Content.Event.Attendees.UserIds.ToObject<string[]>();

                var eventOrganizers = contentItem.Content.EventOrganizer.ContentItems;

                foreach (var organizer in eventOrganizers)
                {
                    var user = new Dictionary<string, string>()
                    {
                        {"userId", organizer.EventOrganizer.Organizer.UserIds.ToObject<string[]>()[0]},
                        {"role", await _shortcodeService.ProcessAsync(organizer.EventOrganizer.Role.Text.ToString())}
                    };

                    eventFormModel.EventOrganizers.Add(user);
                }
            }

            return eventFormModel;
        }

        private async Task<ProposalFormModel> GetProposalInitialValuesAsync(string id)
        {
            var proposalFormModel = new ProposalFormModel
            {
                Id = "",
                Name = "",
                Description = "",
                Roles = Array.Empty<string>(),
                Topics = new LinkedList<IDictionary<string, string>>(),
                Type = "",
                RelatedEntities = new LinkedList<string>(),
                PublishStatus = "",
            };

            if (!string.IsNullOrEmpty(id))
            {
                var contentItem = await GetLocalizedContentAsync(id);

                if (contentItem == null)
                {
                    return null;
                }

                await GetValuesFromRadarEntityPartAsync(proposalFormModel, contentItem);
                proposalFormModel.Type = contentItem.Content.Proposal.Type.TermContentItemIds.ToObject<string[]>()[0];
            }

            return proposalFormModel;
        }

        private async Task<ContentItem> GetLocalizedContentAsync(string id)
        {
            var contentItem = await _contentManager.GetAsync(id);

            if (contentItem == null)
            {
                return null;
            }

            var localizationSet = contentItem.Content.LocalizationPart.LocalizationSet.ToString();
            var localizedContent = await _contentLocalizationManager.GetContentItemAsync(localizationSet, CultureInfo.CurrentCulture.Name);

            return localizedContent;
        }

        // Hepler method to get the values from RadarEntityPart
        private async Task GetValuesFromRadarEntityPartAsync(EntityFormModel entityFormModel, ContentItem contentItem)
        {
            entityFormModel.Id = contentItem.ContentItemId;
            entityFormModel.Name = contentItem.DisplayText;
            entityFormModel.Description = contentItem.Content.RadarEntityPart.Description.Text.ToString();
            entityFormModel.Roles = contentItem.Content.ContentPermissionsPart.Roles.ToObject<string[]>();
            entityFormModel.PublishStatus = await GetPublishStatusAsync(contentItem.Published);

            var topicIds = contentItem.Content.RadarEntityPart.Topics.TermContentItemIds.ToObject<string[]>();
            var topicNames = contentItem.Content.RadarEntityPart.Topics.TagNames.ToObject<string[]>();

            for (var i = 0; i < topicIds.Length; i++)
            {
                var optionPair = new Dictionary<string, string>()
                {
                    {"value", topicIds[i]},
                    {"label", await _shortcodeService.ProcessAsync(topicNames[i])}
                };

                entityFormModel.Topics.Add(optionPair);
            }

            var localizationSets = contentItem.Content.RadarEntityPart.RelatedEntity.LocalizationSets.ToObject<string[]>();

            foreach (var localizationSet in localizationSets)
            {
                contentItem = await _contentLocalizationManager.GetContentItemAsync(localizationSet, CultureInfo.CurrentCulture.Name);

                entityFormModel.RelatedEntities.Add(contentItem.ContentItemId);
            }
        }

        private async Task<string> GetPublishStatusAsync(bool isPublished)
        {
            if (isPublished)
            {
                return await _shortcodeService.ProcessAsync("[locale en]Publish[/locale][locale fr]Publier[/locale]"); ;
            }
            else
            {
                return await _shortcodeService.ProcessAsync("[locale en]Draft[/locale]Brouillon[/locale]");
            }
        }
    }
}

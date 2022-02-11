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
using OrchardCore.ContentLocalization.Models;
using OrchardCore.Flows.Models;
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

        public async Task<ArtifactFormModel> GetArtifactInitialValuesAsync(string parentId, string childId)
        {
            var artifactModel = new ArtifactFormModel();

            artifactModel = new ArtifactFormModel()
            {
                Id = "",
                ParentId = "",
                Name = "",
                Url = "",
                Roles = Array.Empty<string>(),
                PublishStatus = "",
            };

            if (!string.IsNullOrEmpty(parentId))
            {
                var parentContentItem = await GetLocalizedContentAsync(parentId);

                artifactModel.ParentId = parentContentItem.ContentItemId;

                if(!string.IsNullOrEmpty(childId))
                {
                    var artifacts = parentContentItem.Get<BagPart>("Workspace").ContentItems;

                    foreach (var artifact in artifacts)
                    {
                        if (childId == artifact.ContentItemId)
                        {
                            artifactModel.Id = artifact.ContentItemId;
                            artifactModel.Name = await _shortcodeService.ProcessAsync(artifact.DisplayText);
                            artifactModel.Url = artifact.Content.Artifact.URL.Text.ToObject<string>();
                            artifactModel.Roles = artifact.Content.ContentPermissionsPart.Roles.ToObject<string[]>();
                            artifactModel.PublishStatus = await GetPublishStatusAsync(artifact.Published);

                            return artifactModel;
                        }
                    }
                }

                return artifactModel;
            }

            return null;
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
                RelatedEntities = new LinkedList<IDictionary<string, string>>(),
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
                Type = new Dictionary<string, string>(),
                RelatedEntities = new LinkedList<IDictionary<string, string>>(),
                CommunityMembers = new LinkedList<IDictionary<string, object>>(),
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
                communityFormModel.Type = new Dictionary<string, string>()
                {
                    {"value", contentItem.Content.Community.Type.TermContentItemIds.ToObject<string[]>()[0]},
                    {"label", contentItem.Content.Community.Type.TagNames.ToObject<string[]>()[0]}
                };

                var communityMembers = contentItem.Content.CommunityMember.ContentItems;

                foreach (var member in communityMembers)
                {
                    var user = new Dictionary<string, object>()
                    {
                        {"user", new Dictionary<string, string>()
                        {
                            {"value", member.CommunityMember.Member.UserIds.ToObject<string[]>()[0]},
                            {"label", member.CommunityMember.Member.UserNames.ToObject<string[]>()[0]}
                        }},
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
                StartDate = GetDateFromDateTime(DateTime.Now),
                EndDate = GetDateFromDateTime(DateTime.Now),
                StartTime = GetTimeFromDateTime(DateTime.Now),
                EndTime = GetTimeFromDateTime(DateTime.Now),
                Attendees = new LinkedList<IDictionary<string, string>>(),
                EventOrganizers = new LinkedList<IDictionary<string, string>>(),
                RelatedEntities = new LinkedList<IDictionary<string, string>>(),
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
                eventFormModel.StartDate = GetDateFromDateTime(DateTime.Parse(contentItem.Content.Event.StartDate.Value.ToString()));
                eventFormModel.StartTime = GetTimeFromDateTime(DateTime.Parse(contentItem.Content.Event.StartDate.Value.ToString()));
                eventFormModel.EndDate = GetDateFromDateTime(DateTime.Parse(contentItem.Content.Event.EndDate.Value.ToString()));
                eventFormModel.EndTime = GetTimeFromDateTime(DateTime.Parse(contentItem.Content.Event.EndDate.Value.ToString()));

                string[] attendeeIds = contentItem.Content.Event.Attendees.UserIds.ToObject<string[]>();
                string[] attendeeNames = contentItem.Content.Event.Attendees.UserNames.ToObject<string[]>();

                for (var i = 0; i < attendeeIds.Length; i++)
                {
                    var user = new Dictionary<string, string>()
                    {
                        {"value", attendeeIds[i]},
                         {"label", attendeeNames[i]}
                    };

                    eventFormModel.Attendees.Add(user);
                }

                var eventOrganizers = contentItem.Content.EventOrganizer.ContentItems;

                foreach (var organizer in eventOrganizers)
                {
                    var user = new Dictionary<string, string>()
                    {
                        {"value", organizer.EventOrganizer.Organizer.UserIds.ToObject<string[]>()[0]},
                        {"label", organizer.EventOrganizer.Organizer.UserNames.ToObject<string[]>()[0]}
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
                Type = new Dictionary<string, string>(),
                RelatedEntities = new LinkedList<IDictionary<string, string>>(),
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
                proposalFormModel.Type = new Dictionary<string, string>()
                {
                    {"value", contentItem.Content.Proposal.Type.TermContentItemIds.ToObject<string[]>()[0]},
                    {"label", contentItem.Content.Proposal.Type.TagNames.ToObject<string[]>()[0]}
                };
            }

            return proposalFormModel;
        }

        private string GetDateFromDateTime(DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd");
        }

        private string GetTimeFromDateTime(DateTime dt)
        {
            return dt.ToString("hh:mm");
        }

        private async Task<ContentItem> GetLocalizedContentAsync(string id)
        {
            var contentItem = await _contentManager.GetAsync(id, VersionOptions.Latest);

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
            entityFormModel.PublishStatus = await GetPublishStatusAsync(contentItem.Content.RadarEntityPart.Publish.Value.ToObject<bool>());

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
                var part = contentItem.As<LocalizationPart>();

                var optionPair = new Dictionary<string, string>(){
                    {"value", part.LocalizationSet},
                    {"label", contentItem.DisplayText}
                };

                entityFormModel.RelatedEntities.Add(optionPair);
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
                return await _shortcodeService.ProcessAsync("[locale en]Draft[/locale][locale fr]Brouillon[/locale]");
            }
        }
    }
}

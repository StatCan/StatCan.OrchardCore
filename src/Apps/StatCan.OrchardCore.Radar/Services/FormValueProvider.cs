using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using OrchardCore.ContentManagement;
using OrchardCore.Queries;
using OrchardCore.ContentManagement.Display;
using OrchardCore.DisplayManagement.ModelBinding;
using Etch.OrchardCore.ContentPermissions.Services;
using OrchardCore.ContentLocalization.Models;
using OrchardCore.Contents;
using OrchardCore.Taxonomies.Models;
using StatCan.OrchardCore.Radar.FormModels;

/*
    Note: use LocalizationEntries to retrive localized content for form values
*/
namespace StatCan.OrchardCore.Radar.Services
{
    public class FormValueProvider
    {
        private IContentManager _contentManager;
        private readonly IQueryManager _queryManager;

        public FormValueProvider(IContentManager contentManager, IQueryManager queryManager)
        {
            _contentManager = contentManager;
            _queryManager = queryManager;
        }

        public async Task<FormModel> GetInitialValues(string entityType, string id)
        {
            if (entityType == "topics")
            {
                return await GetTopicInitialValues(id);
            }
            else if (entityType == "projects")
            {
                return await GetProjectInitialValues(id);
            }
            else if (entityType == "communities")
            {
                return await GetCommunityInitialValues(id);
            }
            else if (entityType == "events")
            {
                return await GetEventInitialValues(id);
            }
            else if (entityType == "proposals")
            {
                return await GetProposalInitialValues(id);
            }

            return null;
        }

        private async Task<TopicFormModel> GetTopicInitialValues(string id)
        {
            // Initialize a new form model
            var topicFormModel = new TopicFormModel();

            topicFormModel = new TopicFormModel()
            {
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
                            topicFormModel.Name = topic.DisplayText;
                            topicFormModel.Description = topic.Content.Topic.Description.Text.ToString();
                            topicFormModel.Roles = topic.Content.ContentPermissionsPart.Roles.ToObject<string[]>();

                            break;
                        }
                    }
                }
            }

            return topicFormModel;
        }

        private async Task<ProjectFormModel> GetProjectInitialValues(string id)
        {
            var projectFormModel = new ProjectFormModel
            {
                Name = "",
                Description = "",
                Roles = Array.Empty<string>(),
                Topics = Array.Empty<string>(),
                Type = "",
                ProjectMembers = new Dictionary<string, string>()
            };

            // Retrive existing content based on localization

            return projectFormModel;
        }

        private async Task<CommunityFormModel> GetCommunityInitialValues(string id)
        {
            var communityFormModel = new CommunityFormModel
            {
                Name = "",
                Description = "",
                Roles = Array.Empty<string>(),
                Topics = Array.Empty<string>(),
                Type = "",
                CommunityMembers = new Dictionary<string, string>()
            };

            return communityFormModel;
        }

        private async Task<EventFormModel> GetEventInitialValues(string id)
        {
            var EventFormModel = new EventFormModel
            {
                Name = "",
                Description = "",
                Roles = Array.Empty<string>(),
                Topics = Array.Empty<string>(),
                StartDate = DateTime.Today,
                EndDate = DateTime.Today,
                EventOrganizer = new Dictionary<string, string>()
            };

            return EventFormModel;
        }

        private async Task<ProposalFormModel> GetProposalInitialValues(string id)
        {
            var proposalFormModel = new ProposalFormModel
            {
                Name = "",
                Description = "",
                Roles = Array.Empty<string>(),
                Topics = Array.Empty<string>(),
                Type = "",
            };

            // Retrive existing content based on localization

            return proposalFormModel;
        }
    }
}

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
    }
}

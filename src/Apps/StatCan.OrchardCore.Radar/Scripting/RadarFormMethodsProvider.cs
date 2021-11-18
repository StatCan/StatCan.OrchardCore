using System.Collections;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using OrchardCore.ContentManagement;
using OrchardCore.Queries;
using OrchardCore.Shortcodes.Services;
using OrchardCore.Scripting;
using OrchardCore.Taxonomies.Models;
using OrchardCore.Autoroute.Models;
using StatCan.OrchardCore.Radar.FormModels;
using StatCan.OrchardCore.Radar.Services.ValueConverters;
using StatCan.OrchardCore.Radar.Services.ContentConverters;

namespace StatCan.OrchardCore.Radar.Scripting
{
    public class RadarFormMethodsProvider : IGlobalMethodProvider
    {
        private readonly GlobalMethod _createOrUpdateTopic;
        private readonly GlobalMethod _convertToFormModel;
        private readonly GlobalMethod _convertToContentDocument;
        // private readonly GlobalMethod _createProject;
        // private readonly GlobalMethod _createEvent;
        // private readonly GlobalMethod _createProposal;
        // private readonly GlobalMethod _createCommunity;

        public RadarFormMethodsProvider()
        {
            {
                _createOrUpdateTopic = new GlobalMethod
                {
                    Name = "createOrUpdateTopic",
                    Method = serviceProvider => (Func<string, JObject, string>)((id, values) =>
                    {
                        var rawValueConverter = serviceProvider.GetRequiredService<TopicRawValueConverter>();
                        var topicFormModel = (TopicFormModel)rawValueConverter.ConvertAsync(values).GetAwaiter().GetResult();

                        var contentConverter = serviceProvider.GetRequiredService<TopicContentConverter>();

                        var queryManager = serviceProvider.GetRequiredService<IQueryManager>();
                        var shortcodeService = serviceProvider.GetRequiredService<IShortcodeService>();
                        var contentManager = serviceProvider.GetRequiredService<IContentManager>();

                        // Each topic needs to be retrived from the taxonomy term
                        var topicQuery = queryManager.GetQueryAsync("AllTaxonomiesSQL").GetAwaiter().GetResult();
                        var topicResult = queryManager.ExecuteQueryAsync(topicQuery, new Dictionary<string, object> { { "type", "Topics" } }).GetAwaiter().GetResult();

                        // Updating existing topic
                        if (topicResult != null)
                        {
                            var topicTaxonomy = topicResult.Items.First() as ContentItem;
                            var topicPart = topicTaxonomy.As<TaxonomyPart>().Content;

                            foreach (JObject existingTopic in (JArray)topicPart.Terms)
                            {
                                if (id.Equals(existingTopic["ContentItemId"].Value<string>()))
                                {
                                    var topic = contentManager.NewAsync("Topic").GetAwaiter().GetResult();
                                    var existing = existingTopic.ToObject<ContentItem>();

                                    // Converts form model into content item document
                                    var topicUpdateObject = contentConverter.ConvertAsync(topicFormModel, new { Existing = existing }).GetAwaiter().GetResult();

                                    topic.ContentItemId = existing.ContentItemId;
                                    topic.Merge(existing);
                                    topic.Merge(topicUpdateObject, new JsonMergeSettings
                                    {
                                        MergeArrayHandling = MergeArrayHandling.Replace,
                                        MergeNullValueHandling = MergeNullValueHandling.Merge
                                    });
                                    topic.Weld<TermPart>();
                                    topic.Alter<TermPart>(t => t.TaxonomyContentItemId = topicTaxonomy.ContentItemId);

                                    existingTopic.Merge(topic.Content, new JsonMergeSettings
                                    {
                                        MergeArrayHandling = MergeArrayHandling.Replace,
                                        MergeNullValueHandling = MergeNullValueHandling.Merge
                                    });

                                    existingTopic["DisplayText"] = topicUpdateObject["Topic"]["Name"]["Text"].Value<string>();

                                    contentManager.UpdateAsync(topicTaxonomy).GetAwaiter().GetResult();

                                    return existing.ContentItemId;
                                }
                            }

                            // Creating new topic
                            var newTopic = contentManager.NewAsync("Topic").GetAwaiter().GetResult();
                            newTopic.Weld<TermPart>();
                            newTopic.Alter<TermPart>(t => t.TaxonomyContentItemId = topicTaxonomy.ContentItemId);
                            newTopic.Alter<AutoroutePart>(part =>
                            {
                                part.Path = newTopic.ContentItemId;
                            });

                            // Converts form model into content item document
                            var topicCreateObject = contentConverter.ConvertAsync(topicFormModel, null).GetAwaiter().GetResult();

                            newTopic.Merge(topicCreateObject);
                            newTopic.DisplayText = topicCreateObject["Topic"]["Name"]["Text"].Value<string>();

                            topicTaxonomy.Alter<TaxonomyPart>(part => part.Terms.Add(newTopic));

                            contentManager.UpdateAsync(topicTaxonomy).GetAwaiter().GetResult();
                            contentManager.UnpublishAsync(topicTaxonomy).GetAwaiter().GetResult(); // Needs to unpublish it first otherwise publish has no effect
                            contentManager.PublishAsync(topicTaxonomy).GetAwaiter().GetResult();

                            return newTopic.ContentItemId;
                        }

                        return null;
                    })
                };

                _convertToFormModel = new GlobalMethod()
                {
                    Name = "convertToFormModel",
                    Method = serviceProvider => (Func<string, JObject, FormModel>)((type, rawValues) =>
                    {
                        IRawValueConverter converter = null;

                        if (type == "Project")
                        {
                            converter = serviceProvider.GetRequiredService<ProjectRawValueConverter>();
                        }
                        else if (type == "Proposal")
                        {
                            converter = serviceProvider.GetRequiredService<ProposalRawValueConverter>();
                        }

                        return converter.ConvertAsync(rawValues).GetAwaiter().GetResult();
                    })
                };

                _convertToContentDocument = new GlobalMethod()
                {
                    Name = "convertToContentDocument",
                    Method = serviceProvider => (Func<string, FormModel, JObject>)((type, formModel) =>
                    {
                        IContentConverter converter = null;

                        if (type == "Project")
                        {
                            converter = serviceProvider.GetRequiredService<ProjectContentConverter>();
                        }
                        else if (type == "Proposal")
                        {
                            converter = serviceProvider.GetRequiredService<ProposalContentConverter>();
                        }

                        return converter.ConvertAsync(formModel, null).GetAwaiter().GetResult();
                    })
                };
            }
        }

        public IEnumerable<GlobalMethod> GetMethods()
        {
            return new[] { _createOrUpdateTopic, _convertToFormModel, _convertToContentDocument };
        }
    }
}


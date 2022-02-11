using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using OrchardCore.ContentManagement;
using OrchardCore.Queries;
using OrchardCore.Shortcodes.Services;
using OrchardCore.Flows.Models;
using OrchardCore.Scripting;
using OrchardCore.Taxonomies.Models;
using OrchardCore.Autoroute.Models;
using OrchardCore.ContentLocalization;
using OrchardCore.Localization;
using StatCan.OrchardCore.Radar.FormModels;
using StatCan.OrchardCore.Radar.Services.ValueConverters;
using StatCan.OrchardCore.Radar.Services.ContentConverters;
using StatCan.OrchardCore.Radar.Services;

namespace StatCan.OrchardCore.Radar.Scripting
{
    public class RadarFormMethodsProvider : IGlobalMethodProvider
    {
        private readonly GlobalMethod _createOrUpdateTopic;
        private readonly GlobalMethod _convertToFormModel;
        private readonly GlobalMethod _convertToContentDocument;
        private readonly GlobalMethod _createOrUpdateArtifact;
        private readonly GlobalMethod _getTaxonomyByTypeAndId;

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

                _createOrUpdateArtifact = new GlobalMethod()
                {
                    Name = "createOrUpdateArtifact",
                    Method = serviceProvider => (Func<string, string, FormModel, string>)((parentId, id, formModel) =>
                    {
                        var shortcodeService = serviceProvider.GetRequiredService<IShortcodeService>();
                        var contentManager = serviceProvider.GetRequiredService<IContentManager>();
                        var contentConverter = serviceProvider.GetRequiredService<ArtifactContentConverter>();

                        var contentLocalizationManager = serviceProvider.GetRequiredService<IContentLocalizationManager>();
                        var localizationService = serviceProvider.GetRequiredService<ILocalizationService>();

                        var parentContentItem = contentManager.GetAsync(parentId).GetAwaiter().GetResult();

                        var supportedCultures = localizationService.GetSupportedCulturesAsync().GetAwaiter().GetResult();

                        if (parentContentItem != null)
                        {
                            var localizationSet = parentContentItem.Content.LocalizationPart.LocalizationSet.ToString();
                            var isUpdate = false;
                            var artifactId = "";

                            var workspace = parentContentItem.Get<BagPart>("Workspace");
                            var artifactLocalizationSet = "";

                            foreach (var artifact in workspace.ContentItems)
                            {
                                if (id.Equals(artifact.ContentItemId))
                                {
                                    isUpdate = true;
                                    artifactId = artifact.ContentItemId;
                                    artifactLocalizationSet = artifact.Content.Artifact.LocalizationSet.Text.ToObject<string>();
                                }
                            }

                            if (isUpdate)
                            {
                                // Update the artifact in other localized version of parent content items
                                foreach (var supportedCulture in supportedCultures)
                                {
                                    var localizedVersion = contentLocalizationManager.GetContentItemAsync(localizationSet, supportedCulture).GetAwaiter().GetResult() as ContentItem;

                                    var localizedWorkspace = localizedVersion.Get<BagPart>("Workspace");

                                    foreach (var artifact in localizedWorkspace.ContentItems)
                                    {
                                        if (artifact.Content.Artifact.LocalizationSet.Text.ToObject<string>() == artifactLocalizationSet)
                                        {
                                            var tempArtifact = contentManager.NewAsync("Artifact").GetAwaiter().GetResult();
                                            tempArtifact.Merge(artifact);

                                            // Converts form model into content item document
                                            var artifactUpdateObject = contentConverter.ConvertAsync(formModel, new { Existing = artifact }).GetAwaiter().GetResult();
                                            tempArtifact.Merge(artifactUpdateObject, new JsonMergeSettings
                                            {
                                                MergeArrayHandling = MergeArrayHandling.Replace,
                                                MergeNullValueHandling = MergeNullValueHandling.Merge
                                            });

                                            artifact.Merge(tempArtifact, new JsonMergeSettings
                                            {
                                                MergeArrayHandling = MergeArrayHandling.Replace,
                                                MergeNullValueHandling = MergeNullValueHandling.Merge
                                            });

                                            artifact.DisplayText = artifactUpdateObject["TitlePart"]["Title"].Value<string>();

                                            if (artifactUpdateObject["Published"].Value<bool>())
                                            {
                                                contentManager.PublishAsync(artifact).GetAwaiter().GetResult();
                                            }
                                            else
                                            {
                                                contentManager.UnpublishAsync(artifact).GetAwaiter().GetResult();
                                            }

                                            contentManager.UpdateAsync(artifact).GetAwaiter().GetResult();

                                            localizedVersion.Apply("Workspace", localizedWorkspace);

                                            contentManager.UpdateAsync(localizedVersion).GetAwaiter().GetResult();
                                            contentManager.UnpublishAsync(localizedVersion).GetAwaiter().GetResult();
                                            contentManager.PublishAsync(localizedVersion).GetAwaiter().GetResult();
                                        }
                                    }
                                }

                                return artifactId;
                            }

                            var artifactCreateObject = contentConverter.ConvertAsync(formModel, null).GetAwaiter().GetResult();

                            // Create the artifact in all localized version also
                            foreach (var supportedCulture in supportedCultures)
                            {
                                var localizedVersion = contentLocalizationManager.GetContentItemAsync(localizationSet, supportedCulture).GetAwaiter().GetResult() as ContentItem;

                                var newArtifact = contentManager.NewAsync("Artifact").GetAwaiter().GetResult();
                                newArtifact.Merge(artifactCreateObject);
                                newArtifact.DisplayText = artifactCreateObject["TitlePart"]["Title"].Value<string>();
                                newArtifact.Alter<AutoroutePart>(part => part.Path = "artifacts/" + newArtifact.ContentItemId);

                                if(artifactCreateObject["Published"].Value<bool>())
                                {
                                    contentManager.PublishAsync(newArtifact).GetAwaiter().GetResult();
                                }

                                contentManager.UpdateAsync(newArtifact).GetAwaiter().GetResult();

                                newArtifact.Latest = true;

                                var localizedWorkspace = localizedVersion.Get<BagPart>("Workspace");
                                localizedWorkspace.ContentItems.Add(JObject.FromObject(newArtifact).ToObject<ContentItem>());
                                localizedVersion.Apply("Workspace", localizedWorkspace);

                                contentManager.UpdateAsync(localizedVersion).GetAwaiter().GetResult();
                                contentManager.UnpublishAsync(localizedVersion).GetAwaiter().GetResult();
                                contentManager.PublishAsync(localizedVersion).GetAwaiter().GetResult();

                                if (CultureInfo.CurrentCulture.Name == supportedCulture)
                                {
                                    artifactId = newArtifact.ContentItemId;
                                }
                            }

                            return artifactId;
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
                        else if (type == "Community")
                        {
                            converter = serviceProvider.GetRequiredService<CommunityRawValueConverter>();
                        }
                        else if (type == "Event")
                        {
                            converter = serviceProvider.GetRequiredService<EventRawValueConverter>();
                        }
                        else if (type == "Artifact")
                        {
                            converter = serviceProvider.GetRequiredService<ArtifactRawValueConverter>();
                        }
                        else if (type == "Topic")
                        {
                            converter = serviceProvider.GetRequiredService<TopicRawValueConverter>();
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
                        else if (type == "Community")
                        {
                            converter = serviceProvider.GetRequiredService<CommunityContentConverter>();
                        }
                        else if (type == "Event")
                        {
                            converter = serviceProvider.GetRequiredService<EventContentConverter>();
                        }
                        else if (type == "Artifact")
                        {
                            converter = serviceProvider.GetRequiredService<ArtifactContentConverter>();
                        }
                        else if (type == "Topic")
                        {
                            converter = serviceProvider.GetRequiredService<TopicContentConverter>();
                        }

                        return converter.ConvertAsync(formModel, null).GetAwaiter().GetResult();
                    })
                };

                _getTaxonomyByTypeAndId = new GlobalMethod()
                {
                    Name = "getTaxonomyByTypeAndId",
                    Method = serviceProvider => (Func<string, string, ContentItem>)((type, id) =>
                    {
                        if (string.IsNullOrEmpty(id))
                        {
                            return null;
                        }

                        var taxonomyManager = serviceProvider.GetRequiredService<TaxonomyManager>();

                        return taxonomyManager.GetTaxonomyTermByIdAsync(type, id).GetAwaiter().GetResult();
                    })
                };
            }
        }

        public IEnumerable<GlobalMethod> GetMethods()
        {
            return new[] { _createOrUpdateTopic, _convertToFormModel, _convertToContentDocument, _createOrUpdateArtifact, _getTaxonomyByTypeAndId };
        }
    }
}


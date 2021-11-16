using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
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

namespace StatCan.OrchardCore.Radar.Scripting
{
    public class RadarFormMethodsProvider : IGlobalMethodProvider
    {
        private readonly GlobalMethod _createOrUpdateTopic;
        // private readonly GlobalMethod _createProject;
        // private readonly GlobalMethod _createEvent;
        // private readonly GlobalMethod _createProposal;
        // private readonly GlobalMethod _createCommunity;

        public RadarFormMethodsProvider()
        {
            _createOrUpdateTopic = new GlobalMethod
            {
                Name = "createOrUpdateTopic",
                Method = serviceProvider => (Func<string, JObject, string>)((id, values) =>
                {
                    var rawValues = values;

                    rawValues.Remove("roleOptions");
                    rawValues.Remove("__RequestVerificationToken");

                    // Array having a single value gets converted to JValue instead of JArray so we need to convert it back
                    if(rawValues["roles"] is JValue)
                    {
                        var roleArray = new JArray();
                        roleArray.Add(rawValues["roles"]);
                        rawValues["roles"] = roleArray;
                    }

                    var topicFormModel = JsonConvert.DeserializeObject<TopicFormModel>(rawValues.ToString());

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
                                var topicUpdateObject = new
                                {
                                    Topic = new
                                    {
                                        Name = new { Text = UpdateLocalizedString(existing.Content.Topic.Name.Text.ToString(), topicFormModel.Name, CultureInfo.CurrentCulture.Name) },
                                        Description = new { Text = UpdateLocalizedString(existing.Content.Topic.Description.Text.ToString(), topicFormModel.Description, CultureInfo.CurrentCulture.Name) }
                                    },
                                    ContentPermissionsPart = new
                                    {
                                        Enabled = true,
                                        Roles = topicFormModel.Roles,
                                    }
                                };

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

                                existingTopic["DisplayText"] = topicUpdateObject.Topic.Name.Text;

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
                        var topicCreateObject = new
                        {
                            Topic = new
                            {
                                Name = new { Text = CreateLocalizedString(topicFormModel.Name, CultureInfo.CurrentCulture.Name) },
                                Description = new { Text = CreateLocalizedString(topicFormModel.Description, CultureInfo.CurrentCulture.Name) }
                            },
                            ContentPermissionsPart = new
                            {
                                Enabled = true,
                                Roles = topicFormModel.Roles,
                            }
                        };

                        newTopic.Merge(topicCreateObject);
                        newTopic.DisplayText = topicCreateObject.Topic.Name.Text;

                        topicTaxonomy.Alter<TaxonomyPart>(part => part.Terms.Add(newTopic));

                        contentManager.UpdateAsync(topicTaxonomy).GetAwaiter().GetResult();
                        contentManager.UnpublishAsync(topicTaxonomy).GetAwaiter().GetResult(); // Needs to unpublish it first otherwise publish has no effect
                        contentManager.PublishAsync(topicTaxonomy).GetAwaiter().GetResult();

                        return newTopic.ContentItemId;
                    }

                    return null;
                })
            };
        }

        public IEnumerable<GlobalMethod> GetMethods()
        {
            return new[] { _createOrUpdateTopic };
        }

        private string CreateLocalizedString(string content, string currentCulture)
        {
            var supportedCultures = new string[] { "en", "fr" }; // Assumming only en and fr
            StringBuilder sb = new StringBuilder();

            foreach (var supportedCulture in supportedCultures)
            {
                sb.Append($"[locale {supportedCulture}]");
                sb.Append(content);
                sb.Append("[/locale]");
            }

            return sb.ToString();
        }

        private IDictionary<string, string> ExtractLocalizedString(string localizedString)
        {
            var supportedCultures = new string[] { "en", "fr" }; // Assumming only en and fr
            var localizedStrings = new Dictionary<string, string>();

            foreach (var supportedCulture in supportedCultures)
            {
                var leftTag = $"[locale {supportedCulture}]";
                var rightTag = "[/locale]";

                var startingIndex = localizedString.IndexOf(leftTag) + leftTag.Length;
                var endingIndex = localizedString.IndexOf(rightTag, startingIndex);

                var content = localizedString.Substring(startingIndex, endingIndex - startingIndex);

                localizedStrings.Add(supportedCulture, content);
            }

            return localizedStrings;
        }

        private string UpdateLocalizedString(string orginal, string insert, string currentCulture)
        {
            IDictionary<string, string> localizedStrings = ExtractLocalizedString(orginal);
            var supportedCultures = new string[] { "en", "fr" }; // Assumming only en and fr
            StringBuilder sb = new StringBuilder();

            foreach (var supportedCulture in supportedCultures)
            {
                sb.Append($"[locale {supportedCulture}]");
                if (currentCulture == supportedCulture)
                {
                    sb.Append(insert);
                }
                else
                {
                    sb.Append(localizedStrings[supportedCulture]);
                }
                sb.Append("[/locale]");
            }

            return sb.ToString();
        }
    }
}

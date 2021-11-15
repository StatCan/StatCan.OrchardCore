using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using YesSql;
using OrchardCore.ContentManagement;
using OrchardCore.Queries;
using OrchardCore.Shortcodes.Services;
using OrchardCore.Scripting;
using OrchardCore.Taxonomies.Models;
using OrchardCore.ContentFields.Fields;
using Etch.OrchardCore.ContentPermissions.Models;
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
                Method = serviceProvider => (Func<string, JObject, bool>)((id, values) =>
                {
                    var rawValues = values;

                    rawValues.Remove("roleOptions");
                    rawValues.Remove("__RequestVerificationToken");

                    var topicFormModel = JsonConvert.DeserializeObject<TopicFormModel>(rawValues.ToString());

                    var queryManager = serviceProvider.GetRequiredService<IQueryManager>();
                    var shortcodeService = serviceProvider.GetRequiredService<IShortcodeService>();
                    var contentManager = serviceProvider.GetRequiredService<IContentManager>();
                    var session = serviceProvider.GetRequiredService<ISession>();

                    // Each topic needs to be retrived from the taxonomy term
                    var topicQuery = queryManager.GetQueryAsync("AllTaxonomiesSQL").GetAwaiter().GetResult();
                    var topicResult = queryManager.ExecuteQueryAsync(topicQuery, new Dictionary<string, object> { { "type", "Topics" } }).GetAwaiter().GetResult();

                    if (topicResult != null)
                    {
                        var topicTaxonomy = topicResult.Items.First() as ContentItem;
                        var topicPart = topicTaxonomy.As<TaxonomyPart>();

                        foreach (var topic in topicPart.Terms)
                        {
                            if (id.Equals(topic.ContentItemId))
                            {
                                return true;
                            }
                        }

                        var newTopic = contentManager.NewAsync("Topic").GetAwaiter().GetResult();
                        newTopic.Weld<TermPart>();
                        newTopic.Alter<TermPart>(t => t.TaxonomyContentItemId = topicTaxonomy.ContentItemId);

                        var topicObject = new
                        {
                            Topic = new
                            {
                                Name = new {Text = topicFormModel.Name},
                                Description = new {Text = topicFormModel.Description}
                            },
                            ContentPermissionsPart = new
                            {
                                Enabled = true,
                                Roles = topicFormModel.Roles,
                            }
                        };

                        newTopic.Merge(topicObject);
                        newTopic.DisplayText = topicFormModel.Name;
                        topicTaxonomy.Alter<TaxonomyPart>(part => part.Terms.Add(newTopic));

                        session.Save(topicTaxonomy);

                        return true;
                    }

                    return true;
                })
            };
        }

        public IEnumerable<GlobalMethod> GetMethods()
        {
            return new[] { _createOrUpdateTopic };
        }
    }
}

using System.Linq;
using System;
using System.Globalization;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Scripting;
using OrchardCore.ContentManagement;
using OrchardCore.Queries;
using OrchardCore.ContentLocalization.Models;
using OrchardCore.Shortcodes.Services;
using OrchardCore.Security.Services;
using StatCan.OrchardCore.Radar.Services;

namespace StatCan.OrchardCore.Radar.Scripting
{
    public class RadarFormValidationMethodsProvider : IGlobalMethodProvider
    {
        private readonly GlobalMethod _validateRadarEntityName;
        private readonly GlobalMethod _validateMaxStringLength;
        private readonly GlobalMethod _validateString;
        private readonly GlobalMethod _validateMemberWithoutRole;
        private readonly GlobalMethod _validateMemberWithRole;
        private readonly GlobalMethod _validateRelatedEntities;
        private readonly GlobalMethod _validateEventDateTime;
        private readonly GlobalMethod _validateTaxonomyName;
        private readonly GlobalMethod _validateTopics;
        private readonly GlobalMethod _validateEntityType; // This is not Content Type
        private readonly GlobalMethod _validateUrl;
        private readonly GlobalMethod _validateRoles;

        public RadarFormValidationMethodsProvider()
        {
            _validateRadarEntityName = new GlobalMethod()
            {
                Name = "validateRadarEntityName",
                Method = serviceProvider => (Func<string, string, ContentItem, bool>)((contentType, name, existingContent) =>
                {
                    var queryManager = serviceProvider.GetRequiredService<IQueryManager>();

                    var nameQuery = queryManager.GetQueryAsync("EntityByTypeAndNameSQL").GetAwaiter().GetResult();
                    var nameResult = queryManager.ExecuteQueryAsync(nameQuery, new Dictionary<string, object> { { "type", contentType }, { "name", name } }).GetAwaiter().GetResult();

                    var contentItems = nameResult.Items.Cast<ContentItem>();

                    if (!nameResult.Items.Any())
                    {
                        return true;
                    }

                    // Vadaliation is slightly different if updating a content item
                    if (existingContent != null)
                    {
                        var existingLocalizationPart = existingContent.As<LocalizationPart>();

                        foreach (var contentItem in contentItems)
                        {
                            var localizationPart = contentItem.As<LocalizationPart>();

                            if (contentItem.ContentItemId != existingContent.ContentItemId && existingLocalizationPart.LocalizationSet != localizationPart.LocalizationSet && existingLocalizationPart.Culture == localizationPart.Culture)
                            {
                                return false;
                            }
                        }

                        return true; // Here means the name is unique
                    }

                    foreach (var contentItem in contentItems)
                    {
                        var localizationPart = contentItem.As<LocalizationPart>();

                        // It's not ok to have non-unique name across different localization
                        if (CultureInfo.CurrentCulture.Name == localizationPart.Culture)
                        {
                            return false;
                        }
                    }

                    return true;
                })
            };

            _validateMaxStringLength = new GlobalMethod()
            {
                Name = "validateMaxStringLength",
                Method = serviceProvider => (Func<string, int, bool>)((str, targetLength) =>
                {
                    return str.Length <= targetLength;
                })
            };

            _validateString = new GlobalMethod()
            {
                Name = "validateString",
                Method = serviceProvider => (Func<string, bool>)((str) =>
                {
                    return !string.IsNullOrWhiteSpace(str);
                })
            };

            _validateMemberWithoutRole = new GlobalMethod()
            {
                Name = "validateMemberWithoutRole",
                Method = serviceProvider => (Func<ICollection<IDictionary<string, string>>, bool>)((members) =>
                 {
                     var queryManager = serviceProvider.GetRequiredService<IQueryManager>();

                     var userIds = new HashSet<string>();

                     foreach (var member in members)
                     {
                         userIds.Add(member["value"]);
                     }

                     // Check if they are duplicated user
                     if (userIds.Count() != members.Count())
                     {
                         return false;
                     }

                     // Check if there are any non-existent user id
                     var userQuery = queryManager.GetQueryAsync("UserByIdSQL").GetAwaiter().GetResult();

                     foreach (var userId in userIds)
                     {
                         var result = queryManager.ExecuteQueryAsync(userQuery, new Dictionary<string, object> { { "userId", userId } }).GetAwaiter().GetResult();

                         if (!result.Items.Any())
                         {
                             return false;
                         }
                     }

                     return true;
                 })
            };

            _validateMemberWithRole = new GlobalMethod()
            {
                Name = "validateMemberWithRole",
                Method = serviceProvider => (Func<ICollection<IDictionary<string, object>>, bool>)((members) =>
                {
                    var queryManager = serviceProvider.GetRequiredService<IQueryManager>();

                    var userIds = new HashSet<string>();
                    var roles = new LinkedList<string>();

                    foreach (var member in members)
                    {
                        var memberObj = (JObject) member["user"];

                        userIds.Add(memberObj["value"].Value<string>());
                        roles.AddLast((string)member["role"]);
                    }

                    // Check if they are duplicated user
                    if (userIds.Count() != members.Count())
                    {
                        return false;
                    }


                    // Check if there are empty roles
                    if (roles.Where(role => string.IsNullOrWhiteSpace(role)).Any())
                    {
                        return false;
                    }

                    // Check if there are any non-existent user id
                    var userQuery = queryManager.GetQueryAsync("UserByIdSQL").GetAwaiter().GetResult();

                    foreach (var userId in userIds)
                    {
                        var result = queryManager.ExecuteQueryAsync(userQuery, new Dictionary<string, object> { { "userId", userId } }).GetAwaiter().GetResult();

                        if (!result.Items.Any())
                        {
                            return false;
                        }
                    }

                    return true;
                })
            };

            _validateRelatedEntities = new GlobalMethod()
            {
                Name = "validateRelatedEntities",
                Method = serviceProvider => (Func<ICollection<IDictionary<string, string>>, string, bool>)((relatedEntities, requiredContentType) =>
                {
                    var queryManager = serviceProvider.GetRequiredService<IQueryManager>();
                    var contentManager = serviceProvider.GetRequiredService<IContentManager>();

                    var entityIds = new HashSet<string>();

                    foreach (var entity in relatedEntities)
                    {
                        entityIds.Add(entity["value"]);
                    }

                    // Check if they are duplicated entity
                    if (entityIds.Count() != relatedEntities.Count())
                    {
                        return false;
                    }

                    // Check if there are any non-existent user id or not correct content type
                    var entityQuery = queryManager.GetQueryAsync("EntityByLocalizationSetSQL").GetAwaiter().GetResult();

                    foreach (var entityId in entityIds)
                    {
                        var result = queryManager.ExecuteQueryAsync(entityQuery, new Dictionary<string, object> { { "localizationSet", entityId } }).GetAwaiter().GetResult();

                        if (!result.Items.Any())
                        {
                            return false;
                        }

                        var contentItemId = (result.Items.First() as JObject)["ContentItemId"].Value<string>();

                        var contentItem = contentManager.GetAsync(contentItemId).GetAwaiter().GetResult();

                        if (contentItem.ContentType != requiredContentType)
                        {
                            return false;
                        }
                    }

                    return true;
                })
            };

            _validateEventDateTime = new GlobalMethod()
            {
                Name = "validateEventDateTime",
                Method = serviceProvider => (Func<string, string, string, string, bool>)((startDate, startTime, endDate, endTime) =>
                {
                    try
                    {
                        DateTime startDateTime = DateTime.Parse($"{startDate} {startTime}");
                        DateTime endDateTime = DateTime.Parse($"{endDate} {endTime}");

                        return startDateTime <= endDateTime;
                    }
                    catch (FormatException)
                    {
                        return false;
                    }
                })
            };

            _validateTaxonomyName = new GlobalMethod()
            {
                Name = "validateTaxonomyName",
                Method = serviceProvider => (Func<string, string, ContentItem, bool>)((type, name, existingTerm) =>
                {
                    var shortcodeService = serviceProvider.GetRequiredService<IShortcodeService>();
                    var taxonomyManager = serviceProvider.GetRequiredService<TaxonomyManager>();

                    var terms = taxonomyManager.GetTaxonomyTermsAsync(type).GetAwaiter().GetResult();

                    if (terms != null)
                    {
                        foreach (var term in terms)
                        {
                            var termName = shortcodeService.ProcessAsync(term.DisplayText).GetAwaiter().GetResult();

                            if (existingTerm != null && term.ContentItemId != existingTerm.ContentItemId && term.DisplayText == name)
                            {
                                return false;
                            }
                            else if (term.DisplayText == name)
                            {
                                return false;
                            }
                        }

                        return true;
                    }

                    return false;
                })
            };

            _validateTopics = new GlobalMethod()
            {
                Name = "validateTopics",
                Method = serviceProvider => (Func<ICollection<IDictionary<string, string>>, bool>)((topics) =>
                {
                    var taxonomyManager = serviceProvider.GetRequiredService<TaxonomyManager>();

                    var topicIds = new HashSet<string>();

                    foreach (var topic in topics)
                    {
                        topicIds.Add(topic["value"]);
                    }

                    // Check if they are duplicated entity
                    if (topicIds.Count() != topics.Count())
                    {
                        return false;
                    }

                    foreach (var topicId in topicIds)
                    {
                        var topic = taxonomyManager.GetTaxonomyTermByIdAsync("Topics", topicId);

                        if (topic == null)
                        {
                            return false;
                        }
                    }

                    return true;
                })
            };

            _validateEntityType = new GlobalMethod()
            {
                Name = "validateEntityType",
                Method = serviceProvider => (Func<string, IDictionary<string, string>, bool>)((typeTaxonomy, type) =>
                 {
                     if (type.Count() > 2) // 2 because each key-value pair is counted as 2 elements
                     {
                         return false;
                     }

                     var taxonomyManager = serviceProvider.GetRequiredService<TaxonomyManager>();

                     var typeTerm = taxonomyManager.GetTaxonomyTermByIdAsync(typeTaxonomy, type["value"]).GetAwaiter().GetResult();

                     if (typeTerm == null)
                     {
                         return false;
                     }

                     return true;
                 })
            };

            _validateUrl = new GlobalMethod()
            {
                Name = "validateUrl",
                Method = serviceProvider => (Func<string, string, bool>)((url, allowedHosts) =>
                 {
                     if(!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                     {
                         return false;
                     }

                    if(allowedHosts.Any())
                    {
                        string host = new Uri(url).Host;

                        if(!allowedHosts.Contains(host))
                        {
                            return false;
                        }
                    }

                     return true;
                 })
            };

            _validateRoles = new GlobalMethod()
            {
                Name = "validateRoles",
                Method = serviceProvider => (Func<string[], bool>)((roles) =>
                 {
                     var roleService = serviceProvider.GetRequiredService<IRoleService>();

                     var allowedRoles = roleService.GetRoleNamesAsync().GetAwaiter().GetResult();

                     foreach(var role in roles)
                     {
                         if(!allowedRoles.Contains(role))
                         {
                             return false;
                         }
                     }

                     return true;
                 })
            };
        }

        public IEnumerable<GlobalMethod> GetMethods()
        {
            return new[] { _validateRadarEntityName, _validateMaxStringLength, _validateString, _validateMemberWithoutRole, _validateMemberWithRole,
            _validateRelatedEntities, _validateEventDateTime, _validateTaxonomyName, _validateTopics, _validateEntityType, _validateUrl, _validateRoles };
        }
    }
}

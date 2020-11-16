using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Handlers;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Modules;
using OrchardCore.Scripting;
using OrchardCore.Users;
using OrchardCore.Users.Models;
using OrchardCore.Users.Services;

namespace StatCan.OrchardCore.Scripting
{
    public class UserSettingsGlobalMethodsProvider : IGlobalMethodProvider
    {
        private readonly GlobalMethod _updateUserProperties;

        public UserSettingsGlobalMethodsProvider(IHttpContextAccessor httpContextAccessor, ILogger<UserSettingsGlobalMethodsProvider> logger)
        {
            _updateUserProperties = new GlobalMethod
            {
                Name = "updateUserSettings",
                Method = serviceProvider => (Func<string, object, bool>)((type, properties) =>
                {
                    var contentDefinitionManager  = serviceProvider.GetRequiredService<IContentDefinitionManager>();
                    IEnumerable<IContentHandler> contentHandlers = serviceProvider.GetRequiredService<IEnumerable<IContentHandler>>();
                    var contentManager  = serviceProvider.GetRequiredService<IContentManager>();
                    var authorizationService  = serviceProvider.GetRequiredService<IAuthorizationService>();
                    var userService  = serviceProvider.GetRequiredService<IUserService>();
                    var userManager  = serviceProvider.GetRequiredService<UserManager<IUser>>();

                    var userClaim = httpContextAccessor.HttpContext.User;
                    var def = contentDefinitionManager.GetTypeDefinition(type);

                    if(def == null)
                    {
                        return false;
                    }

                    if (! authorizationService.AuthorizeAsync(userClaim, CustomUserSettingsPermissions.CreatePermissionForType(def)).GetAwaiter().GetResult())
                    {
                        // permission error
                        return false;
                    }

                    if(def.GetSettings<ContentTypeSettings>().Stereotype != "CustomUserSettings")
                    {
                        // better error message
                        return false;
                    }

                    var user = (User)userService.GetAuthenticatedUserAsync(userClaim).GetAwaiter().GetResult();

                    var contentItem = GetUserSettingsAsync(contentManager, user, def).GetAwaiter().GetResult();
                    contentItem.Merge(properties, new JsonMergeSettings { MergeArrayHandling = MergeArrayHandling.Replace });
                    var updateContentContext = new UpdateContentContext(contentItem);

                    // invoke oc handlers
                    contentHandlers.InvokeAsync((handler, updateContentContext) => handler.UpdatingAsync(updateContentContext), updateContentContext, logger).GetAwaiter().GetResult();
                    contentHandlers.Reverse().InvokeAsync((handler, updateContentContext) => handler.UpdatedAsync(updateContentContext), updateContentContext, logger).GetAwaiter().GetResult();

                    // set the object property
                    user.Properties[def.Name] = JObject.FromObject(contentItem);

                    userManager.UpdateAsync(user).GetAwaiter().GetResult();

                    return true;
                }
                )
            };
        }

        public IEnumerable<GlobalMethod> GetMethods()
        {
            return new[] { _updateUserProperties };
        }

        private async Task<ContentItem> GetUserSettingsAsync(IContentManager contentManager, User user, ContentTypeDefinition settingsType)
        {
            JToken property;
            ContentItem contentItem;

            if (user.Properties.TryGetValue(settingsType.Name, out property))
            {
                var existing = property.ToObject<ContentItem>();

                // Create a new item to take into account the current type definition.
                contentItem = await contentManager.NewAsync(existing.ContentType);
                contentItem.Merge(existing);
            }
            else
            {
                contentItem = await contentManager.NewAsync(settingsType.Name);
            }

            return contentItem;
        }
    }
}

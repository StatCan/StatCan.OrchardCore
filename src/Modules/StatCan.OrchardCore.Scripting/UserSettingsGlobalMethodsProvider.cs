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
    public enum StatusEnum
    {
        Success=0,
        Unauthorized=1,
        TypeError=2,
    }
    public class UserSettingsGlobalMethodsProvider : IGlobalMethodProvider
    {
        private readonly GlobalMethod _updateUserProperties;

        public UserSettingsGlobalMethodsProvider(IHttpContextAccessor httpContextAccessor, ILogger<UserSettingsGlobalMethodsProvider> logger)
        {
            _updateUserProperties = new GlobalMethod
            {
                Name = "updateCustomUserSettings",
                Method = serviceProvider => (Func<string, object, StatusEnum>)((type, properties) =>
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
                        return StatusEnum.TypeError;
                    }

                    if (!authorizationService.AuthorizeAsync(userClaim, CustomUserSettingsPermissions.CreatePermissionForType(def)).GetAwaiter().GetResult())
                    {
                        return StatusEnum.Unauthorized;
                    }

                    if(def.GetSettings<ContentTypeSettings>().Stereotype != "CustomUserSettings")
                    {
                        return StatusEnum.TypeError;
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
                    return StatusEnum.Success;
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

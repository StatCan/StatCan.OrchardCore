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
using OrchardCore.Email;
using OrchardCore.Entities;
using OrchardCore.Modules;
using OrchardCore.Scripting;
using OrchardCore.Settings;
using OrchardCore.Users;
using OrchardCore.Users.Models;
using OrchardCore.Users.Services;

namespace StatCan.OrchardCore.Scripting
{
    public enum UpdateCustomUserSettingsStatus
    {
        Success=0,
        Unauthorized=1,
        TypeError=2,
    }
    public enum UpdateEmailStatus
    {
        Success = 0,
        Unauthorized = 1,
        InvalidEmail = 2,
        AlreadyExists = 3,
        UpdateError = 4,
    }
    public class UserGlobalMethodsProvider : IGlobalMethodProvider
    {
        private readonly GlobalMethod _updateUserProperties;
        private readonly GlobalMethod _validateEmail;
        private readonly GlobalMethod _updateEmail;

        public UserGlobalMethodsProvider(IHttpContextAccessor httpContextAccessor, ILogger<UserGlobalMethodsProvider> logger)
        {
            _updateUserProperties = new GlobalMethod
            {
                Name = "updateCustomUserSettings",
                Method = serviceProvider => (Func<string, object, UpdateCustomUserSettingsStatus>)((type, properties) =>
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
                        return UpdateCustomUserSettingsStatus.TypeError;
                    }

                    if (def.GetSettings<ContentTypeSettings>().Stereotype != "CustomUserSettings")
                    {
                        return UpdateCustomUserSettingsStatus.TypeError;
                    }
                    
                    if (!authorizationService.AuthorizeAsync(userClaim, CustomUserSettingsPermissions.CreatePermissionForType(def)).GetAwaiter().GetResult())
                    {
                        return UpdateCustomUserSettingsStatus.Unauthorized;
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
                    return UpdateCustomUserSettingsStatus.Success;
                }
                )
            };
            _validateEmail = new GlobalMethod
            {
                Name = "validateEmail",
                Method = serviceProvider => (Func<string, bool>)((email) =>
                {
                    if(string.IsNullOrEmpty(email))
                    {
                        return false;
                    }
                    var emailValidator = serviceProvider.GetRequiredService<IEmailAddressValidator>();

                    return emailValidator.Validate(email);
                }
                )
            };
            _updateEmail = new GlobalMethod
            {
                Name = "updateEmail",
                Method = serviceProvider => (Func<string, UpdateEmailStatus>)((email) =>
                {
                    var siteService = serviceProvider.GetRequiredService<ISiteService>();
                    if (!siteService.GetSiteSettingsAsync().GetAwaiter().GetResult().As<ChangeEmailSettings>().AllowChangeEmail)
                    {
                        return UpdateEmailStatus.Unauthorized;
                    }

                    var emailValidator = serviceProvider.GetRequiredService<IEmailAddressValidator>();
                    if (string.IsNullOrEmpty(email) || !emailValidator.Validate(email))
                    {
                        return UpdateEmailStatus.InvalidEmail;
                    }

                    var userService = serviceProvider.GetRequiredService<IUserService>();
                    var userManager = serviceProvider.GetRequiredService<UserManager<IUser>>();

                    var userClaim = httpContextAccessor.HttpContext.User;

                    var user = userService.GetAuthenticatedUserAsync(userClaim).GetAwaiter().GetResult();
                    var userWithEmail = userManager.FindByEmailAsync(email).GetAwaiter().GetResult();

                    if (((User)user).Email.Equals(email, StringComparison.OrdinalIgnoreCase))
                    {
                        // nothing to do, email is the same
                        return UpdateEmailStatus.Success;
                    }
                    else if (userWithEmail != null && user.UserName != userWithEmail.UserName)
                    {
                        return UpdateEmailStatus.AlreadyExists;
                    }
                    else
                    {
                        if (userService.ChangeEmailAsync(user, email, (key, message) => logger.LogError(message)).GetAwaiter().GetResult())
                        {
                            return UpdateEmailStatus.Success;
                        }

                        return UpdateEmailStatus.UpdateError;
                    }
                }
                )
            };
        }

        public IEnumerable<GlobalMethod> GetMethods()
        {
            return new[] { _updateUserProperties, _validateEmail, _updateEmail };
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

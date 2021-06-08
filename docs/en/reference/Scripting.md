# Scripting 

The following methods are available in addition to the OrchardCore [scripting methods](https://docs.orchardcore.net/en/dev/docs/reference/modules/Scripting/#methods).

**Note**: The feature (module) where the scripts are defined needs to be enabled for the methods to be available in your scripts. For example, to get the `httpContext()` function, you must enable the `OrchardCore.Workflows.Http` feature in the admin panel.

## Scripting module (`StatCan.OrchardCore.Scripting`)

### Http

| Function | Description 
| -------- | ----------- |
|`httpRedirect(url: String): void`| Calls the `HttpContext.Response.Redirect()` method. The `~/` will be replaced by the tenant base path. |

### Forms

| Function | Description 
| -------- | ----------- |
|`requestFormAsJsonObject(): JObject`| Returns a sanitized JObject representation of the `HttpContext.Request.Form` object. Sanitization is performed by Orchard's [sanitizer](https://docs.orchardcore.net/en/dev/docs/reference/core/Sanitizer/). |
| `addError(name: String, errorMessage: String): void` | Adds an error to the mvc ModelState dictionary with the specified name. Use the `serverValidationMessage` name to add a global error message to your VueForm. |
| `hasErrors(): Boolean` | Returns true if the error dictionary contains any errors. |
| `validateReCaptcha(recaptchaResponse): Boolean` | Returns true if the recaptchaResponse is valid, false if invalid. |
| `notify(type: String, message: String): Boolean` | Adds a message to the Orchard INotifier. Returns true if the the notification got added properly to the notifier. Type can be `success`,`information`, `warning` or `error` |

### Contents

These methods are added when the `OrchardCore.Contents` module is enabled

| Function | Description 
| -------- | ----------- |
|`contentByItemId(contentItemId: String): ContentItem`| Returns the ContentItem with the specified contentItemId |
|`contentByItemIdVersion(contentItemId: String, version: String): ContentItem`| Returns the ContentItem with the specified contentItemId and specific version. Version can be one of these: `Published`, `Latest`, `Draft`, `DraftRequired`. If version is blank, the Published version is returned |
|`contentByVersionId(versionId: String): ContentItem`| Returns the ContentItem with the specified versionId |
|`ownContentByType(type: String): ContentItem`| Returns all ContentItems of type where the owner is the current user |


### Deployment

These methods are added when the `OrchardCore.Deployment` module is enabled

| Function | Description 
| -------- | ----------- |
|`importRecipeJson(json: String): RecipeJsonResult`| Returns true if the recipe executed successfully or false if something went wrong. Return values Success = 0, InvalidJson = 1, Exception = 2 |

### Users

These methods are added when the `OrchardCore.Users` module is enabled

| Function | Description 
| -------- | ----------- |
|`validateEmail(email: String): Boolean`| Validates an email address |
|`updateEmail(email: String): UpdateEmailStatus`| Validates an email address. Return values: Success = 0, Unauthorized = 1, InvalidEmail = 2, AlreadyExists = 3, UpdateError = 4 |
|`updateCustomUserSettings(contentType: String, properties: Object): UpdateSettingsStatus`| Updates the CustomUserSettings with the specified contentType with the passed properties. Return values: Success = 0, Unauthorized = 1, TypeError = 2 |
|`setUserRole(userName: String, roleName: String): Boolean`| Sets a role to a user. Security critical function. |
|`isAuthenticated(): Boolean`| Returns true if the user is Authenticated. |
|`isInRole(userName: String, roleName: String): Boolean`| Returns true if the user with specified userName has the specified role. |

### Localization

These methods are added when the `OrchardCore.ContentLocalization` module is enabled

| Function | Description 
| -------- | ----------- |
|`contentByLocalizationSet(localizationSet: String, culture: String): ContentItem`|  Returns the ContentItem with the attached localizationSet for the specified culture |

### Taxonomy

These methods are added when the `OrchardCore.Taxonomies` module is enabled

| Function | Description 
| -------- | ----------- |
|`taxonomyTerms(taxonomyContentItemId: String, termContentItemIds: String[]): ContentItem[]`| Returns the Taxonomy Term ContentItems that are listed in the `termContentItemIds` for the taxonomy with id `taxonomyContentItemId`. |
|`taxonomyTermsJson(termObject: JObject): ContentItem[]`| Returns all the taxonomy terms that are listed on the termObject. The termObject is the object output by the TaxonomyPicker field and should contain a `TaxonomyContentItemId` string and a `TermContentItemIds` array. |
## LocalizedText module (`StatCan.OrchardCore.LocalizedText`)

You can get the values stored in the LocalizedTextPart inside a script.

| Function | Description 
| -------- | ----------- |
| `getLocalizedTextValues(contentItem: ContentItem): JObject` | Returns a JObject representation of the LocalizedTextPart for the current thread culture |

## VueForms module (`StatCan.OrchardCore.VueForms`)

| Function | Description 
| -------- | ----------- |
| `getFormContentItem(): ContentItem` | Only available in the VueForm server side scripts. Returns the current VueForm ContentItem instance. |
| `debug(name: String, value: Object)` | Outputs the object in the debug response when the VueForm is in debug mode |


## Media

These methods are added when the `OrchardCore.Media` module is enabled

The `SaveMediaResult` object has the following shape. 
```javascript
{ 
  name: String, 
  size: long, 
  folder: String, 
  mediaPath: String, 
  hasError: Boolean, 
  errorMessage: String 
}
```

| Function | Description 
| -------- | ----------- |
| `saveMedia(folder: String, renameIfExists: Boolean): SaveMediaResult[]` | Saves all files present on the HttpRequest to a specific folder. A number is appended to the file name if the file already exists. |
|`hasMediaError(mediaResults: SaveMediaResult[]): Boolean ` | returns true if one of the items in the SaveMediaResult array has an error  |
|`getMediaErrors(mediaResults: SaveMediaResult[]): String[]` | Gets the list of media paths as an array of strings  |
|`setMediaError(name: String, mediaResults: SaveMediaResult[])` | Sets the ModelError with the specified name to the concatenation of all media errors in the SaveMediaResult array. A newline is inserted between each error |
|`getMediaPaths( mediaResults: SaveMediaResult[]): String[]` | Gets the list of media paths as an array of strings |


### The `saveMedia` method usage

The `saveMedia` method returns an array of `SaveMediaResult` objects. Each object in the array represents a file that was uploaded. 
If an error occurs when trying to save a file, the `hasError` boolean property will be set to `true` and the `errorMessage` property will be populated with the error message. 
If no errors occurs, the `mediaPath` property will be set to the path of the image as saved in the media library.

For example, you can iterate through all the `SaveMediaResult` objects like this:
```javascript
const result = saveMedia('test', true);

for (const file of result) {
  log('Information', `${file.mediaPath}`); 
}

```



# Scripting 

The following methods are available in addition to the OrchardCore [scripting methods](https://docs.orchardcore.net/en/dev/docs/reference/modules/Scripting/#methods).

**Note**: The feature (module) where the scripts are defined needs to be enabled for the methods to be available in your scripts. For example, to get the `httpContext()` function, you must enable the `OrchardCore.Workflows.Http` feature in the admin panel.

## Scripting module (`StatCan.OrchardCore.Scripting`)

### Http

| Function | Description 
| -------- | ----------- |
|`httpRedirect(url: String): void`| Calls the `HttpContext.Response.Redirect()` method, prefixing the passed url with the tenant pathBase |

### Forms

| Function | Description 
| -------- | ----------- |
|`requestFormAsJsonObject(): JObject`| Returns a sanitized JObject representation of the `HttpContext.Request.Form` object. Sanitization is performed by Orchard's [sanitizer](https://docs.orchardcore.net/en/dev/docs/reference/core/Sanitizer/). |

### Contents

These methods are added when the `OrchardCore.Contents` module is enabled

| Function | Description 
| -------- | ----------- |
|`contentByItemId(contentItemId: String): ContentItem`| Returns the ContentItem with the specified contentItemId |

### Localization

These methods are added when the `OrchardCore.ContentLocalization` module is enabled

| Function | Description 
| -------- | ----------- |
|`contentByLocalizationSet(localizationSet: String, culture: String): ContentItem`|  Returns the ContentItem with the attached localizationSet for the specified culture |

## LocalizedText module (`StatCan.OrchardCore.LocalizedText)

You can get the values stored in the LocalizedTextPart inside a script.

| Function | Description 
| -------- | ----------- |
| `getLocalizedTextValues(contentItem: ContentItem): JObject` | Returns a JObject representation of the LocalizedTextPart for the current thread culture |

## VueForms module (`StatCan.OrchardCore.VueForms)

| Function | Description 
| -------- | ----------- |
| `addError(name: String, errorMessage: String): void` | Adds an error to the input / VeeValidate.ValidationProvider with the specified name |
| `getFormContentItem(): ContentItem` | Only available in the VueForm server side scripts. Returns the current VueForm ContentItem instance. |
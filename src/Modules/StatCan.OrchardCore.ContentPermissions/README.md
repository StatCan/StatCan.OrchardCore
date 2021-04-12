# Etch.OrchardCore.ContentPermissions

Module for Orchard Core to enable configuring access at a content item level.

## Usage

Enabled the "Content Permissions" feature, which will make a new "Content Permissions" part available. Attach this part to the desired content types, which will add a new "Security" tab to the content editor. From this tab the content item permissions can be enabled, which will display all the roles in the CMS. Select the roles that can access the content item and publish. Below are different ways of handling when the user isn't associated to one of the roles specified on the part.

### Redirection

Users can be redirected to a specific URL that can be defined in the settings for the content permissions part.

### Display Alternative Content

Users can be displayed an alternative by customising the view template, as shown below. This enables the ability to restrict whether users can see specific widgets on a page.

#### Liquid

Example of how to check users permission and display alternative content with Liquid template.

```
{% assign canView = Model.ContentItem | user_can_view %}
{% if canView %}
	<p>Awesome content that you have permission to view.</p>
{% else %}
	<p>Unfortunately you're not able to view this content.</p>
{% endif %}
```

#### Razor

Example of how to check users permission and display alternative content with Razor template.

```
@inject Etch.OrchardCore.ContentPermissions.Services.IContentPermissionsService ContentPermissionsService

@if (ContentPermissionsService.CanAccess(Model.ContentItem))
{
    <p>Awesome content that you have permission to view.</p>
}
else
{
    <p>Unfortunately you're not able to view this content.</p>
}
```

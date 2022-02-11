# Radar
This module implements the Digital Radar.

## Content Types
### Radar entities
The 4 entities listed below are considered primary entities in radar. They are the top level entities being maintained by radar. All primary entities has RadarEntityPart attached to it.

- Project

- Event

- Community

- Opportunity

### Taxonomies
The only taxonomy in radar is Topic. They can be attached to primary entities. Topics has its own CRUD views.

### Secondary entities
Secondary entities are content items that must be attached to primary entities. For example, Artifact is an example of secondary entitiy. Artifact has its own CRUD views but they are dependent on primary entities. 

## Localization
### Primary and secondary entities
Localization of primary and secondary entities are done using the ContentLocalizationPart. When creating new entities from the frontend form, localized versions are created automatically. Note that tranlsations are not done automatically. The form simply creates other localized content items with text in the current localization.

### Taxonomies
Localization of Topic is done using shortcodes.

### Other labels
For labels like button labels, the localization is done using .po files. 

## Forms
The approach taken to implement the forms is to create one form content item for each radar entities and load the values at runtime. 

### Scripting 
Radar has various script methods related to forms. They help with CRUD operations of radar entities and validation.

### Frontend
The forms are implemented using vue forms. The forms have their own controller actions defined in `FormController.cs`. The form values and options are filled at runtime in `RadarFormPartDisplayDriver`. The display driver fetches the content item values according to the route. The format of the routes are defined in `Startup.cs`. In the views the initial values are loaded in the `InitialValueLoader.liquid` shape. The shape populates the `formValues` object on the global `window` object with the values. After this shape has been rendered, the form values can then be accessed like `window.formValues.[fieldName]`. The `data` object in the vue forms should refer to this `formValues` object.

### Backend

#### FormValueProvider
This is used to populate the form with values of an existing content item. For example, if you want to update experiment1 then the `FormValueProvider` will fetch the values of experiment1 and store them in `RadarFormPart`.

#### FormOptionsProvider
Same process to `FormValueProvider` 

#### ValueConverters
The `ValueConverters` are used to extract and parse the raw form values into an intermediate state. In other words, they convert raw form values into a json object with only the relevent information. This json object can then be used for validation.

#### ContentConverters
The `ContentConverters` are used to convert the json object created by `ValueConverters` into Orchard content items. Results converted by `ContentConverters` can be directly saved using `IContentManager`.

## Custom Permissions
Radar has some custom permissions on top of Orchard and `ContentPermissionPart`. Please refer to `RadarPermissionPartDisplayDriver.cs` for more detail.

## Custom Api endpoints
Radar has some custom endpoints such as async global search, async user search etc... . Please refer to `FormController.cs` and `ListController.cs` for more detail. 

## Custom Vue Components
Radar has some custom view components for global search and tabs.

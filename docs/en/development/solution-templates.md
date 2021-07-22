# Solution Templates

Save time by using one of our templated solutions! We're leveraging `dotnet new`([see Microsoft developer docs](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-new)) here.

## The Solution Template

* This template sets up a new solution that allows you to create your own [Themes](https://docs.orchardcore.net/en/dev/docs/getting-started/theme/) and Modules.
* Uses the `StatCan.OrchardCore` package as hosted on [Cloudsmith](https://cloudsmith.io/~statcan-digitalinnovation/repos/statcan-orchardcore/packages/)


### Downloading

To create a new site using this boilerplate you'll first need to install the template, which is hosted on CloudSmith.

```CMD
dotnet new -i StatCan.OrchardCore.ProjectTemplates::1.0.0-rc3-* --nuget-source https://nuget.cloudsmith.io/statcan-digitalinnovation/statcan-orchardcore/v3/index.json
```

### Run
Once the installation is complete run this command to create your project boilerplate

```
dotnet new stc-oc-siteboilerplate -n Project.StatCan.OrchardCore -o Project.StatCan.OrchardCore -p "Name" -pd "Desciption"
```

#### Parameters

- `-n | --name`: Namespace and name of the .Net project e.g. Project.OrchardCore
- `-o | --output`: Location to place the generated output.
- `-p | --pname`: Project name displayed in the read me.
- `-pd | --pdescription`: Project description displayed in the read me.
- `-ov | --orchard-version`: OrchardCore version to use.
- `-sov | --statcan-orchard-version`: StatCan orchard version to use.

## Additional templates

Additional templates are provided by OrchardCore. See the documentation [here](https://docs.orchardcore.net/en/dev/docs/getting-started/templates/).

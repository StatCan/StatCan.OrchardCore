# Code Generation Templates

We provide a `dotnet new` template for creating new websites pre-configured to use the packages available in the StatCan.OrchardCore repository.
More information about `dotnet new` can be found [here](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-new)

This template is referencing the nightly build of Orchard Core [`1.0.0-rc2-14344`](https://cloudsmith.io/~orchardcore/repos/preview/packages/detail/nuget/OrchardCore.Application.Cms.Targets/1.0.0-rc2-14344/) StatCan.OrchardCore [![Latest Version @ Cloudsmith](https://api-prd.cloudsmith.io/badges/version/statcan-digitalinnovation/statcan-orchardcore/nuget/StatCan.OrchardCore.Application.Targets/latest/x/?render=true&badge_token=gAAAAABfdxH6m77MW-BAV88OzcnBYogkQkz-9UGxGe3tKAO5j2o_zhK9bs8K-zdIfR8Js2G37dZ6ZaXqKJ7k-z1oh1kBDSVxeEk-OqZpRruKsZOucGSh9Us%3D)](https://cloudsmith.io/~statcan-digitalinnovation/repos/statcan-orchardcore/packages/detail/nuget/StatCan.OrchardCore.Application.Targets/latest/)

## Installing the StatCan Site boilerplate

To create a new site using this boilerplate you'll first need to install the template, which is hosted on CloudSmith.

```CMD
dotnet new -i StatCan.OrchardCore.ProjectTemplates::1.0.0-rc2-* --nuget-source https://nuget.cloudsmith.io/statcan-digitalinnovation/statcan-orchardcore/v3/index.json
```

## Create a new website
Once the installation is complete run this command to create your project boilerplate

```
dotnet new stc-oc-siteboilerplate -n Project.StatCan.OrchardCore -o Project.StatCan.OrchardCore -p "Name" -pd "Desciption"
```

### Parameters

- `-n | --name`: Namespace and name of the .Net project e.g. Project.OrchardCore
- `-o | --output`: Location to place the generated output.
- `-p | --pname`: Project name displayed in the read me.
- `-pd | --pdescription`: Project description displayed in the read me.
- `-ov | --orchard-version`: OrchardCore version to use.
- `-sov | --statcan-orchard-version`: StatCan orchard version to use.


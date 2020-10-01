# StatCan OrchardCore Site Boilerplate

Use this template to easily setup your OrchardCore developper environment for a new project

This template is referencing the nightly build of Orchard Core [`1.0.0-rc2-14344`](https://cloudsmith.io/~orchardcore/repos/preview/packages/detail/nuget/OrchardCore.Application.Cms.Targets/1.0.0-rc2-14344/) StatCan.OrchardCore [`1.0.0-rc2-2`](https://cloudsmith.io/~statcan-digitalinnovation/repos/statcan-orchardcore/packages/detail/nuget/StatCan.OrchardCore.Application.Targets/1.0.0-rc2-2/)

## Prerequisities

- [.NET Core](https://docs.microsoft.com/en-us/dotnet/core/). Download the latest version from [here](https://www.microsoft.com/net/download/core).

### Getting Started

To create a new site using this boilerplate use the `dotnet new` command. First you'll need to install the template, which is hosted on NuGet.

```
dotnet new -i StatCan.OrchardCore.SiteBoilerplate --nuget-source https://api.nuget.org/v3/index.json
```

Once the installation is complete run this command to create your boilerplate.

```
dotnet new stc-oc-siteboilerplate -n Project.StatCan.OrchardCore -o Project.StatCan.OrchardCore -p "Name" -pd "Desciption"
```

### Parameters

The dotnet new parameters are 

- `-n` `--name`: Namespace and name of the .Net project e.g. Project.OrchardCore
- `-o` `--output`: Location to place the generated output.
- `-p` `--pname`: Project name displayed in the read me.
- `-pd` `--pdescription`: Project description displayed in the read me.



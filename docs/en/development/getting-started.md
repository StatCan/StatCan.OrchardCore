# Getting Started

Hello fellow developer, Get started here !

## Developer environment

### Dependencies

- dotnet core 3.1
- node 10+
- Yarn 1.17+

### Nuget packages

We use an in-house build of Orchard that we update periodically. This build is deployed to artifactory 
and currently has a deployed version number `1.0.0-in1`. In this case, `in` stands for innovation.

To be able to run the `dotnet restore` command, you will need to add your artifactory credentials
to the nuget.config file on your computer. 

On windows, this file is located under `%appdata%\NuGet\NuGet.config`

Edit the file and input your Artifactory Encrypted password under the "cleartextpassword" value.

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <clear />
    <add key="ArtifactoryNuGetV3" value="https://artifactory.cloud.statcan.ca/artifactory/api/nuget/v3/nuget" protocolVersion="3" />
  </packageSources>
<packageSourceCredentials>
    <ArtifactoryNuGetV3>
      <add key="username" value="jp.tissot"/>
      <add key="cleartextpassword" value="<artifactory encrypted password>" />
    </ArtifactoryNuGetV3>
  </packageSourceCredentials>
</configuration>

```

#### Setting up your nuget feed globally

This is not required since we include a `nuget.config` file at the root of the repository.
If you want to use the development version of OrchardCore, you need to add a nuget
package source to your nuget configuration file.
In linux, this file is located `~/.nuget/NuGet/NuGet.Config`

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" protocolVersion="3" />
    <add key="OrchardPreview" value="https://www.myget.org/F/orchardcore-preview/api/v3/index.json" protocolVersion="3" />
  </packageSources>
</configuration>
```


### Recommended VSCode extensions

- Omnisharp (C#)
- Liquid Language Support
- Bracked pair colorizer
- vscode-icons
- GitLens
- EditorConfig
- vscode-solution-explorer

### Recommended powershell setup

Run these commands to setup your powershell profile script:

```powershell
# From a powershell terminal
# This displays the path of your profile
$profile
# Test to see if it exists
test-path $profile
# Create it if it does not exist
new-item -path $profile -itemtype file -force
# Edit it in VSCode
code $profile
```

Adding these functions can save a lot of keystrokes:

```powershell
$innoGitRepo = "<path-to>\innovation-website"

# Navigate to directory
function in { set-location $innoGitRepo }
# Open cypress
function it {
  set-location "$($innoGitRepo)\test"
  npm run cypress
}
# Build
function ib { 
  set-location "$($innoGitRepo)\src\Inno.Web"
  dotnet build
}
# Clean
function ic { 
  set-location "$($innoGitRepo)\src\Inno.Web"
  dotnet clean
}
# Run
function ir {
  set-location "$($innoGitRepo)\src\Inno.Web"
  dotnet run
}
# Run (Production)
function ip {
  set-location "$($innoGitRepo)\src\Inno.Web"
  dotnet .\bin\Debug\netcoreapp3.0\Inno.Web.dll
}
# Build & Run (Production)
function ibp { 
  ib
  dotnet .\bin\Debug\netcoreapp3.0\Inno.Web.dll
}
```
Don't forget to restart your powershell session to load the changes.

## Quickstart 

- Run this ese commands in powershell:
  - From anywhere `ir` if you have the above powershell setup or
  - From the root of the project `dotnet run --project src/Inno.Web/Inno.Web.csproj` if you don't
- Visit [https://localhost:5001](https://localhost:5001) for Orchard dev site
- When presented with the orchard setup screen,
  - Select the the recipe you want to try. I suggest using the `Software as a Service` recipe
    and then use Orchard's Tenant feature to test different sites.
  - Use sqlite as a database to keep things simple
- Visit [https://localhost:5001/admin](https://localhost:5001/admin) to use the admin interface of Orchard.

## Starting fresh

To cleanup the environment and start fresh, you need to delete the `src/Inno.Web/App_Data` folder. 
This deletes all configuration and databases (if you are using SQLite).

You can also delete tenants individually by deleting the data in `src/Inno.Web/App_Data/Sites/{TenantName}` and 
removing the entry in `src/Inno.Web/App_Data/tenants.json`

## Updating assets

When updating assets, you need to run gulp to copy the assets into the wwwroot folder. To install gulp, run `yarn install`
Simply run `yarn run build` which will build the css and copy the css and js files to the `wwwroot/` folder of each module.

You can also run `yarn run watch` to rebuild assets when changes are detected.

Supported types: SaSS, ts, js, and Less.

## Testing

The framework used to test the innovation website is Cypress.

Note: Running the tests assume a clean environment and may fail if you run them on an existing instance.

To run tests:

- From the `test` folder of the project, run `yarn install` (if you have yarn installed) or `npm install`
- From the same folder, you can run `yarn cypress` to open cypress and start running tests

## Debugging

We include a vscode `launch.json` / `tasks.json` files to help debug your code.

To launch the site with debugging, open the Debug (ctrl+shift+D) vscode window.
Select the .NET Core Launch(web) if the project is not running, or
.NET Core Attach to attach the debugger to attach to a running project.



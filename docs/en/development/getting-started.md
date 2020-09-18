# Getting Started

Hello fellow developer, Get started here !

## Developer environment

### Dependencies

- dotnet core 3.1
- node 10+
- Yarn 1.17+

### Recommended VSCode extensions

- Omnisharp (C#)
- Liquid Language Support
- Bracket pair colorizer
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
$innoGitRepo = "<path-to>\StatCan.OrchardCore"

# Navigate to directory
function in { set-location $innoGitRepo }
# Run the clean script from the test folder. Warning: this deletes your App_Data
function iac {
  set-location "$($innoGitRepo)\test"
  npm run clean
}
# Open cypress
function it {
  set-location "$($innoGitRepo)\test"
  npm run cypress
}
# Run css watcher
function iw {
  set-location "$($innoGitRepo)"
  npm run watch
}
# Build
function ib { 
  set-location "$($innoGitRepo)src\StatCan.OrchardCore.Cms.Web"
  dotnet build
}
# Clean
function ic { 
  set-location "$($innoGitRepo)src\StatCan.OrchardCore.Cms.Web"
  dotnet clean
}
# Run
function ir {
  set-location "$($innoGitRepo)src\StatCan.OrchardCore.Cms.Web"
  dotnet run
}
# Run (Production)
function ip {
  set-location "$($innoGitRepo)src\StatCan.OrchardCore.Cms.Web"
  dotnet .\bin\Debug\netcoreapp3.0\StatCan.OrchardCore.Cms.Web.dll
}
# Build & Run (Production)
function ibp { 
  ib
  dotnet .\bin\Debug\netcoreapp3.1\StatCan.OrchardCore.Cms.Web.dll
}

```
Don't forget to restart your powershell session to load the changes.

## Quickstart 

- Run these commands in powershell:
  - From anywhere `ir` if you have the above powershell setup or
  - From the root of the project `dotnet run --project src/StatCan.OrchardCore.Cms.Web/StatCan.OrchardCore.Cms.Web.csproj` if you don't
- Visit [https://localhost:5001](https://localhost:5001) for Orchard dev site
- When presented with the orchard setup screen,
  - Select the the recipe you want to try. I suggest using the `Software as a Service` recipe
    and then use Orchard's Tenant feature to test different sites.
  - Use sqlite as a database to keep things simple
- Visit [https://localhost:5001/admin](https://localhost:5001/admin) to use the admin interface of Orchard.

## Starting fresh

To cleanup the environment and start fresh, you need to delete the `src/StatCan.OrchardCore.Cms.Web/App_Data` folder. 
This deletes all configuration and databases (if you are using SQLite).

You can also delete tenants individually by deleting the data in `src/StatCan.OrchardCore.Cms.Web/App_Data/Sites/{TenantName}` and 
removing the entry in `src/StatCan.OrchardCore.Cms.Web/App_Data/tenants.json`

## Updating assets

When updating assets, you need to run gulp to copy the assets into the wwwroot folder. To install gulp, run `npm install`
Simply run `npm run build` which will build the css and copy the css and js files to the `wwwroot/` folder of each module.

You can also run `npm run watch` to rebuild assets when changes are detected.

Supported types: SaSS, ts, js, and Less.

## Testing

The framework used to test the innovation website is Cypress.

Note: The tests assume a clean environment and may fail if you run them on an existing instance.

To run tests:

- From the `test` folder of the project, run `npm install`.
- From the same folder, you can run `npm run cypress` to open cypress and start running tests

## Debugging

We include `launch.json` / `tasks.json` files to help debug your code.

To launch the site with debugging, open the Debug (ctrl+shift+D) vscode window.
Select the .NET Core Launch(web) if the project is not running, or
.NET Core Attach to attach the debugger to attach to a running project.



# Logging

Our main project uses [Serilog](https://serilog.net/) structured logging.
The default logging configuration can be found in the [appsettings.json](https://github.com/StatCan/StatCan.OrchardCore/blob/master/src/StatCan.OrchardCore.Cms.Web/appsettings.json) file at the root of the web project.

It is recommended to override these values using Environment Variables when in a production environment.

```powershell
$env:Serilog__MinimumLevel="Information"
```

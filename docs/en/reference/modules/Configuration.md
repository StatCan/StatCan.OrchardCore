# Deployment configuration

The `StatCan.OrchardCore.Configuration` library adds support for configuring persistent modules, smtp settings, reverse proxy setting and https settings with orchard's [configuration](https://docs.orchardcore.net/en/dev/docs/reference/core/Configuration/).

## Example appsettings.json

```json
{
  "OrchardCore": {
    "StatCan_Configuration": {
      "OverwriteSmtpSettings": true,
      "OverwriteReverseProxySettings": true,
      "OverwriteHttpsSettings": true
    },
    "StatCan_Smtp": {
      "DefaultSender": "no-reply@cloud.statcan.ca",
      "DeliveryMethod": 0,
      "Host": "smtp.cloud.statcan.ca",
      "PickupDirectoryLocation": null,
      "AutoSelectEncryption": false,
      "EncryptionMethod": 2,
      "Port": 25,
      "RequireCredentials": true,
      "UseDefaultCredentials": false,
      "UserName": "test",
      "Password": "mysmtppassword"
    },
    "StatCan_Https": {
      "RequireHttps": true,
      "EnableStrictTransportSecurity": false,
      "RequireHttpsPermanent": false,
      "SslPort": null
    },
    "StatCan_ReverseProxy": {
      "EnableXForwardedFor": true,
      "EnableXForwardedHost": true,
      "EnableXForwardedProto": true
    },
    "StatCan_PersistentFeatures": [
      "StatCan.OrchardCore.DisplayHelpers",
      "StatCan.OrchardCore.Scripting"
    ]
  }
}
```

## Sections

### StatCan_Configuration

The variables in this section define if the related settings will be overwritten when the tenant activates. For example, if someone modifies the smtp settings from the Admin interface and the `OverwriteSmtpSettings` is set to true, the settings will be reset to those specified via configuration the next time the tenant activates.

### StatCan_Smtp
This section will update Orchard's [email module configuration](https://docs.orchardcore.net/en/dev/docs/reference/modules/Email). 

### StatCan_Https
This section will update Orchard's [https module configuration](https://docs.orchardcore.net/en/dev/docs/reference/modules/Https). 

### StatCan_ReverseProxy
This section will update Orchard's [reverse proxy module configuration](https://docs.orchardcore.net/en/dev/docs/reference/modules/ReverseProxy). 

### StatCan_PersistentFeatures
This section defines a list of features that will be enabled by default when a tenant is activated, and that will re-enable themselves then disabled. Support for this should make it's way in Orchard's repository eventually.


## Environment variables

It is recommended to use [environment variables](https://docs.orchardcore.net/en/dev/docs/reference/core/Configuration/index.html#ishellconfiguration-via-environment-variables) to configure these settings in a production environment.

```shell
# Configuration
OrchardCore__StatCan_Configuration__OverwriteSmtpSettings
OrchardCore__StatCan_Configuration__OverwriteReverseProxySettings
OrchardCore__StatCan_Configuration__OverwriteHttpsSettings

# Smtp
OrchardCore__StatCan_Smtp__DefaultSender
OrchardCore__StatCan_Smtp__DeliveryMethod
OrchardCore__StatCan_Smtp__Host
OrchardCore__StatCan_Smtp__PickupDirectoryLocation
OrchardCore__StatCan_Smtp__AutoSelectEncryption
OrchardCore__StatCan_Smtp__EncryptionMethod
OrchardCore__StatCan_Smtp__Port
OrchardCore__StatCan_Smtp__RequireCredentials
OrchardCore__StatCan_Smtp__UseDefaultCredentials
OrchardCore__StatCan_Smtp__UserName
OrchardCore__StatCan_Smtp__Password

# Https
OrchardCore__StatCan_Https__RequireHttps
OrchardCore__StatCan_Https__EnableStrictTransportSecurity
OrchardCore__StatCan_Https__RequireHttpsPermanent
OrchardCore__StatCan_Https__SslPort

# ReverseProxy
OrchardCore__StatCan_ReverseProxy__EnableXForwardedFor
OrchardCore__StatCan_ReverseProxy__EnableXForwardedHost
OrchardCore__StatCan_ReverseProxy__EnableXForwardedProto

# PersistentFeatures
OrchardCore__StatCan_PersistentFeatures__0
OrchardCore__StatCan_PersistentFeatures__1

```

Here is an example on how to set these environment variables with powershell
```powershell
$env:OrchardCore__StatCan_Configuration__OverwriteReverseProxySettings="true"
$env:OrchardCore__StatCan_ReverseProxy__EnableXForwardedFor="true"
$env:OrchardCore__StatCan_ReverseProxy__EnableXForwardedHost="true"
$env:OrchardCore__StatCan_ReverseProxy__EnableXForwardedProto="true"
$env:OrchardCore__StatCan_Https__RequireHttps="true"
$env:OrchardCore__StatCan_Configuration__OverwriteHttpsSettings="true"
$env:OrchardCore__StatCan_PersistentFeatures__0="OrchardCore.ReverseProxy"
$env:OrchardCore__StatCan_PersistentFeatures__1="OrchardCore.Https"

```

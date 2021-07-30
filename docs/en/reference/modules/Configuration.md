# Deployment configuration

The `StatCan.OrchardCore.Configuration` library adds support for configuring persistent modules, smtp settings, reverse proxy setting and https settings with orchard's [configuration](https://docs.orchardcore.net/en/dev/docs/reference/core/Configuration/).

## Example appsettings.json

```json
{
  "OrchardCore": {
    "Features": [
      "StatCan.OrchardCore.Configuration",
      "OrchardCore.Email",
      "OrchardCore.ReverseProxy",
      "OrchardCore.Https"
    ],
    "StatCan_Configuration": {
      "OverwriteSmtpSettings": true,
      "OverwriteReverseProxySettings": true,
      "OverwriteHttpsSettings": true
    },
    "StatCan_Smtp": {
      "DefaultSender": "no-reply@statcan.ca",
      "DeliveryMethod": 0,
      "Host": "smtp.statcan.ca",
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


## Environment variables

It is recommended to use [environment variables](https://docs.orchardcore.net/en/dev/docs/reference/core/Configuration/index.html#ishellconfiguration-via-environment-variables) to configure these settings in a production environment.

```shell
# Configuration
OrchardCore__StatCan_Configuration__OverwriteSmtpSettings: bool
OrchardCore__StatCan_Configuration__OverwriteReverseProxySettings: bool
OrchardCore__StatCan_Configuration__OverwriteHttpsSettings: bool

# Smtp
OrchardCore__StatCan_Smtp__DefaultSender: string
OrchardCore__StatCan_Smtp__DeliveryMethod: 0=Network;1=SpecifiedPickupDirectory
OrchardCore__StatCan_Smtp__Host: string
OrchardCore__StatCan_Smtp__PickupDirectoryLocation: string
OrchardCore__StatCan_Smtp__AutoSelectEncryption: bool
OrchardCore__StatCan_Smtp__EncryptionMethod: 0=none;1=SSLTLS;2=STARTTLS
OrchardCore__StatCan_Smtp__Port: int
OrchardCore__StatCan_Smtp__RequireCredentials: bool
OrchardCore__StatCan_Smtp__UseDefaultCredentials: bool
OrchardCore__StatCan_Smtp__UserName: string
OrchardCore__StatCan_Smtp__Password: string (unencrypted)

# Https
OrchardCore__StatCan_Https__RequireHttps: bool
OrchardCore__StatCan_Https__EnableStrictTransportSecurity: bool
OrchardCore__StatCan_Https__RequireHttpsPermanent: bool
OrchardCore__StatCan_Https__SslPort: int

# ReverseProxy
OrchardCore__StatCan_ReverseProxy__EnableXForwardedFor: bool
OrchardCore__StatCan_ReverseProxy__EnableXForwardedHost: bool
OrchardCore__StatCan_ReverseProxy__EnableXForwardedProto: bool

# Features
OrchardCore__Features__0: string
OrchardCore__Features__1: string

```

Here is an example on how to set these environment variables with powershell
```powershell
$env:OrchardCore__Features__0="StatCan.OrchardCore.Configuration"
$env:OrchardCore__Features__1="OrchardCore.ReverseProxy"
$env:OrchardCore__Features__2="OrchardCore.Https"

$env:OrchardCore__StatCan_Configuration__OverwriteReverseProxySettings="true"
$env:OrchardCore__StatCan_ReverseProxy__EnableXForwardedFor="true"
$env:OrchardCore__StatCan_ReverseProxy__EnableXForwardedHost="true"
$env:OrchardCore__StatCan_ReverseProxy__EnableXForwardedProto="true"

$env:OrchardCore__StatCan_Configuration__OverwriteHttpsSettings="true"
$env:OrchardCore__StatCan_Https__RequireHttps="true"


$env:OrchardCore__StatCan_Configuration__OverwriteSmtpSettings="true"
$env:OrchardCore__StatCan_Smtp__DefaultSender="no-reply@statcan.ca"
$env:OrchardCore__StatCan_Smtp__DeliveryMethod="0"
$env:OrchardCore__StatCan_Smtp__Host="smtp.jp.com"
$env:OrchardCore__StatCan_Smtp__AutoSelectEncryption="false"
$env:OrchardCore__StatCan_Smtp__EncryptionMethod="2"
$env:OrchardCore__StatCan_Smtp__Port="25"
$env:OrchardCore__StatCan_Smtp__RequireCredentials="true"
$env:OrchardCore__StatCan_Smtp__UseDefaultCredentials="false"
$env:OrchardCore__StatCan_Smtp__UserName="myusername"
$env:OrchardCore__StatCan_Smtp__Password="mysmtppassword"

```

# Other Configuration

This is how you can configure environment variables for the AllowedFileExtensions of media files

```powershell
$env:OrchardCore__OrchardCore_Media__AllowedFileExtensions__1=".jpeg"
$env:OrchardCore__OrchardCore_Media__AllowedFileExtensions__2=".png"
$env:OrchardCore__OrchardCore_Media__AllowedFileExtensions__3=".gif"
$env:OrchardCore__OrchardCore_Media__AllowedFileExtensions__4=".ico"
$env:OrchardCore__OrchardCore_Media__AllowedFileExtensions__5=".svg"
$env:OrchardCore__OrchardCore_Media__AllowedFileExtensions__6=".pdf"
$env:OrchardCore__OrchardCore_Media__AllowedFileExtensions__7=".doc"
$env:OrchardCore__OrchardCore_Media__AllowedFileExtensions__8=".docx"
$env:OrchardCore__OrchardCore_Media__AllowedFileExtensions__9=".ppt"
$env:OrchardCore__OrchardCore_Media__AllowedFileExtensions__10=".pptx"
$env:OrchardCore__OrchardCore_Media__AllowedFileExtensions__11=".pps"
$env:OrchardCore__OrchardCore_Media__AllowedFileExtensions__12=".ppsx"
$env:OrchardCore__OrchardCore_Media__AllowedFileExtensions__13=".odt"
$env:OrchardCore__OrchardCore_Media__AllowedFileExtensions__14=".xls"
$env:OrchardCore__OrchardCore_Media__AllowedFileExtensions__15=".xlsx"
$env:OrchardCore__OrchardCore_Media__AllowedFileExtensions__16=".psd"
$env:OrchardCore__OrchardCore_Media__AllowedFileExtensions__17=".mp3"
$env:OrchardCore__OrchardCore_Media__AllowedFileExtensions__18=".m4a"
$env:OrchardCore__OrchardCore_Media__AllowedFileExtensions__19=".ogg"
$env:OrchardCore__OrchardCore_Media__AllowedFileExtensions__20=".wav"
$env:OrchardCore__OrchardCore_Media__AllowedFileExtensions__21=".mp4"
$env:OrchardCore__OrchardCore_Media__AllowedFileExtensions__22=".m4v"
$env:OrchardCore__OrchardCore_Media__AllowedFileExtensions__23=".mov"
$env:OrchardCore__OrchardCore_Media__AllowedFileExtensions__24=".wmv"
$env:OrchardCore__OrchardCore_Media__AllowedFileExtensions__25=".avi"
$env:OrchardCore__OrchardCore_Media__AllowedFileExtensions__26=".mpg"
$env:OrchardCore__OrchardCore_Media__AllowedFileExtensions__27=".ogv"
$env:OrchardCore__OrchardCore_Media__AllowedFileExtensions__28=".3gp"

$env:OrchardCore__OrchardCore_Media__AllowedFileExtensions__29=".csv"
$env:OrchardCore__OrchardCore_Media__AllowedFileExtensions__30=".msg"

```

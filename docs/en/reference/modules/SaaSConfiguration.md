# SaaS Configuration (`StatCan.OrchardCore.SaaSConfiguration`)

This module aims at simplifying the configuration of a SaaS application. 

## Features
- Automatic configuration of OpenId Server (Server, Application, Scopes)
- Automatic configuration of OpenId Clients for tenants.

When a new tenant is created and has the `StatCan.OrchardCore.SaaSConfiguration.Client` feature enabled, this module will automatically sync the Client Id and Secret when the Main tenant modifies the settings.

## Roadmap
- Automatic configuration of Login / Registration settings for child tenants.
- Role mapping configuration for child tenants
- Mapping of UserProfile properties for tenants (based on the Default tenant's claims)
- Running of recipes in child tenants (maybe multiple at once)


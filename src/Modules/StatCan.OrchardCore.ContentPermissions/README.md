# Etch.OrchardCore.ContentPermissions

This module was renamed to allow us to deploy our version to nuget. Credit goes to Etchuk for this module.

Module for Orchard Core to enable configuring access at a content item level.

## Build Status

[![Build Status](https://secure.travis-ci.org/etchuk/Etch.OrchardCore.ContentPermissions.png?branch=master)](http://travis-ci.org/etchuk/Etch.OrchardCore.ContentPermissions) [![NuGet](https://img.shields.io/nuget/v/Etch.OrchardCore.ContentPermissions.svg)](https://www.nuget.org/packages/Etch.OrchardCore.ContentPermissions)

## Orchard Core Reference

This module is referencing the RC2 build of Orchard Core ([`1.0.0-rc2-13450`](https://www.nuget.org/packages/OrchardCore.Module.Targets/1.0.0-rc1-10004)).

## Usage

Enabled the "Content Permissions" feature, which will make a new "Content Permissions" part available. Attach this part to the desired content types, which will add a new "Security" tab to the content editor. From this tab the content item permissions can be enabled, which will display all the roles in the CMS. Select the roles that can access the content item and publish. Users not associated to one of the specified roles will receive a 403 response and redirected to `/Error/403`. The redirect URL can be customised within the settings for the content part.
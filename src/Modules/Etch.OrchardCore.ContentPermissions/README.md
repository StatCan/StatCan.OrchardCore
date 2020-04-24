# Etch.OrchardCore.ContentPermissions

Module for Orchard Core to enable configuring access at a content item level.

## Build Status

[![Build Status](https://secure.travis-ci.org/etchuk/Etch.OrchardCore.ContentPermissions.png?branch=master)](http://travis-ci.org/etchuk/Etch.OrchardCore.ContentPermissions) [![NuGet](https://img.shields.io/nuget/v/Etch.OrchardCore.ContentPermissions.svg)](https://www.nuget.org/packages/Etch.OrchardCore.ContentPermissions)

## Orchard Core Reference

This module is referencing the RC1 build of Orchard Core ([`1.0.0-rc1-10004`](https://www.nuget.org/packages/OrchardCore.Module.Targets/1.0.0-rc1-10004)).

## Installing

This module is available on [NuGet](https://www.nuget.org/packages/Etch.OrchardCore.ContentPermissions). Add a reference to your Orchard Core web project via the NuGet package manager. Search for "Etch.OrchardCore.Fields", ensuring include prereleases is checked.

Alternatively you can [download the source](https://github.com/etchuk/Etch.OrchardCore.ContentPermissions/archive/master.zip) or clone the repository to your local machine. Add the project to your solution that contains an Orchard Core project and add a reference to Etch.OrchardCore.ContentPermissions.

## Usage

Enabled the "Content Permissions" feature, which will make a new "Content Permissions" part available. Attach this part to the desired content types, which will add a new "Security" tab to the content editor. From this tab the content item permissions can be enabled, which will display all the roles in the CMS. Select the roles that can access the content item and publish. Users not associated to one of the specified roles will receive a 403 response and redirected to `/Error/403`. The redirect URL can be customised within the settings for the content part.
# Workflows

This document contains documentation for all workflow tasks and activities available in this repository.


## GitHub (`StatCan.OrchardCore.GitHub`)

The following activities are available with the GitHub module

| Activity | Type | Description |
| -------- | ---- | ----------- |
| Commit File | Task | Create or updates a file on a specific branch |
| Create Branch | Task | Creates a branch from a git reference |
| Create Pull Request | Task | Creates a pull request |


## VueForms (`StatCan.OrchardCore.VueForms`)

| Activity | Type | Description |
| -------- | ---- | ----------- |
| VueForm Submitted | Event | Triggerred when a VueForm is valid. Runs for all selected VueForms in the Event options. |

The VueForm Submitted event includes the `VueForm` content item as an input and the correlationId of the form. You can access the Form data by using the `requestFormAsJsonObject()` scripting method.

## EmailTemplates (`StatCan.OrchardCore.EmailTemplates`)

| Activity | Type | Description |
| -------- | ---- | ----------- |
| Send Email With Template | Task | Sends an email with the selected template. |

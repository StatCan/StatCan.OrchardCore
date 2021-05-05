# Localization

Orchard uses [PO](https://www.gnu.org/software/gettext/manual/html_node/PO-Files.html) files to support translation / pluralization of static strings.

See the [orchard documentation](https://orchardcore.readthedocs.io/en/dev/docs/guides/install-localization-files/)

## Automatic translations

We have a GitHub action [script](https://github.com/StatCan/StatCan.OrchardCore/blob/master/.github/workflows/translation.yml) that automates the translation process for us. 

This action is triggerred manually by running the workflow from [here](https://github.com/StatCan/StatCan.OrchardCore/actions/workflows/translation.yml).

This script does the following:
1. Extracts the translation in po files from the code with the [PoExtractor.OrchardCore](https://github.com/lukaskabrt/PoExtractor) dotnet tool.
2. Runs our [translation_script](https://github.com/StatCan/StatCan.OrchardCore/blob/master/translation_script/po-gtranslator.js) that uses the free Google Translate api to get a best guess estimate of the translations.
3. Creates a pull request with the new translations strings.

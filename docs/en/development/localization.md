# Localization

Orchard uses [PO](https://www.gnu.org/software/gettext/manual/html_node/PO-Files.html) files to support translation / pluralization of static strings.

See the [orchard documentation](https://orchardcore.readthedocs.io/en/dev/docs/guides/install-localization-files/).

## How to generate POT files from our modules

1. Install the [po-extractor](https://github.com/lukaskabrt/PoExtractor) tool. `dotnet tool install --global PoExtractor.OrchardCore`
2. Run `extractpo-oc C:\PathToRepo\src C:\temp\OrchardInno --liquid`

## Where to place translated PO files


## How to create translations

I like to use POEdit to create the translation. Another good option is to upload the po file to Google [Translator Toolkit](https://translate.google.com/toolkit/list?hl=en#translations/active). Google automatically translates all strings in the file using Google Translate which makes it much easier than having to translate everything from scratch.

## How to sync changes to the pot files

Generate new pot files using the command then either

- use POEdit to update every PO file from a corresponding POT file
- OR
- use the [msgmerge](https://www.gnu.org/software/gettext/manual/gettext.html#Updating) utility from the [gettext](https://www.gnu.org/software/gettext/) library.

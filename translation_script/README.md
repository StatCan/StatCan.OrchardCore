# po-gtranslator
An node.js script for translating PO files using Google Translate API.


## Usage from powershell

```powershell
$env:GOOGLE_APPLICATION_CREDENTIALS="path-to-key"

node po-gtranslator.js --project_id=oc-translation --po_source="C:\translations\extract" --po_dest="C:\...\StatCan.OrchardCore\src\StatCan.OrchardCore.Cms.Web\Localization\fr" --lang=fr

```


## Requirements

In order to use Google Translate API you need to setup a project in your Google Cloud Console, please check a simple guide here: https://cloud.google.com/translate/docs/quickstart-client-libraries

Feel free to fork the project and submit enhancements and fixes!

Proudly developed by [Diego Imbriani]([https:/](https://diegoimbriani.me/)) ([GreenTreeLabs](https://www.greentreelabs.net)) 💪 and adapted by [Jean-Philippe Tissot](https://github.com/jptissot)

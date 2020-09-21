# LocalizedText (`StatCan.OrchardCore.LocalizedText`)

This module adds a **LocalizedText Part** that stores `[{ name, [{ culture, value}]}]` objects. 

## LocalizedTextPart

Attach this part to your content items to have the ability to add `name -> value` pairs that vary by culture.

### Usages

This part is meant to be used in cases where a Single ContentItem needs to hold localized values. 
As a "best practice" this should only be used when most of the data is non localizable and some is localizable.

## Liquid

Please see the [liquid](../Liquid.md#localizedtext-statcanorchardcorelocalizedtext) documentation for liquid filters.

## Scripting

Please see the [scripting](../Scripting.md#localizedtext-module-statcanorchardcorelocalizedtext) documentation
# LocalizedText (`StatCan.OrchardCore.LocalizedText`)

This module adds a **LocalizedText Part** that stores `[{ name, [{ culture, value}]}]` objects. 


## LocalizedTextPart

Attach this part to your content items to have the ability to add `name -> value` pairs that vary by culture.

### Usages

This part is meant to be used in cases where a Single ContentItem needs to hold localized values. 
As a "best practice" this should only be used when most of the data is non localizable and some is localizable.


## `localize` liquid filter

Use the `localize` liquid filter to reference and output a value that matches the current culture and name provided. 

### Examples

Data: 
```json
[
  {
    "Name": "my_value",
    "LocalizedItems": [
      {
        "Culture":"en",
        "Value":"Some English Value"
      },
      {
        "Culture":"fr",
        "Value":"Some French Value"
      }
    ]
  }
]

```
Input

```liquid
{{ "my_value" | localize }}

{{ Model.ContentItem | localize: "my_value"}}
```

Output

```text
Some English Value

Some English Value
```

#### You can also pass parameters to your values

Data: 
```json
[
  {
    "Name": "my_parameterized_value",
    "LocalizedItems": [
      {
        "Culture":"en",
        "Value":"Some {0} English Value"
      },
      {
        "Culture":"fr",
        "Value":"Some {0} French Value"
      }
    ]
  }
]

```
Input

```liquid
{{ "my_parameterized_value" | localize: "parameterized" }}

{{ Model.ContentItem | localize: "my_parameterized_value", "parameterized" }}
```

Output

```text
Some parameterized English Value

Some parameterized English Value
```

#### if you want to render html, you need to `| raw`

Data: 
```json
[
  {
    "Name": "my_html_value",
    "LocalizedItems": [
      {
        "Culture":"en",
        "Value":"<span>Some English Value</span>"
      },
      {
        "Culture":"fr",
        "Value":"<span>Some French Value</span>"
      }
    ]
  }
]

```
Input

```liquid
{{ "my_html_value" | localize }}

{{ Model.ContentItem | localize: "my_html_value" | raw }}
```

Output

```text
&lt;span&gt;bold value&lt;/span&gt;

<span>Some English Value</span>
```

## Scripting

You can get 
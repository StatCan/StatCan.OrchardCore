# Liquid

## LocalizedText (`StatCan.OrchardCore.LocalizedText`) 

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

## GitHub (`StatCan.OrchardCore.GitHub`)

Here are some liquid filters provided by the GitHub module.

`tokenName` refers to the name of the token you added to the github settings.

### github_pr filter

Returns the `PullRequest` object for the specified pull request number.

```liquid
{{ 123 | github_pullrequest: "owner", "repo", "tokenName" }}
```

### github_pr_reviewcomments filter

Returns a list of `PullRequestReviewComment` object for the specified pull request number.
This returns the comments related to PR reviews. Use the `github_comments` filter to get pr discussions.

```liquid
{{ 123 | github_pullrequest_comments: "owner", "repo", "tokenName" }}
```

### github_issue filter

Returns the `Issue` object for the specified issue number.

```liquid
{{ 123 | github_issue: "owner", "repo", "tokenName" }}
```

### github_comments filter

Returns a list of `IssueComment` for the specified issue / pull request number.

```liquid
{{ 123 | github_comments: "owner", "repo", "tokenName" }}
```

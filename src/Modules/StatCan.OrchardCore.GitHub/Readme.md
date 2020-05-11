# StatCan.OrchardCore.GitHub

This module adds GitHub api workflow tasks to OrchardCore.

Your GitHub api token must be set throught the _Configuration -> Settings -> GitHub Api_ menu in the admin dashboard.

## Activities

The following activities are available

| Activity | Type | Description |
| -------- | ---- | ----------- |
| Commit File | Task | Create or updates a file on a specific branch |
| Create Branch | Task | Creates a branch from a git reference |
| Create Pull Request | Task | Creates a pull request |


## Liquid Filters

### github_pr filter

Returns the `PullRequest` object for the specified pull request number.

```liquid
{{ 123 | github_pullrequest: "owner", "repo" }}
```

### github_pr_reviewcomments filter

Returns a list of `PullRequestReviewComment` object for the specified pull request number.
This returns the comments related to PR reviews. Use the `github_comments` filter to get pr discussions.

```liquid
{{ 123 | github_pullrequest_comments: "owner", "repo" }}
```

### github_issue filter

Returns the `Issue` object for the specified issue number.

```liquid
{{ 123 | github_issue: "owner", "repo" }}

### github_comments filter

Returns a list of `IssueComment` for the specified issue / pull request number.

```liquid
{{ 123 | github_issue_comments: "owner", "repo" }}
```
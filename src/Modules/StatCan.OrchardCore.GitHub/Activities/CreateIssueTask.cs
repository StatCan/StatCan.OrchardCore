using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Octokit;
using OrchardCore.Workflows.Abstractions.Models;
using OrchardCore.Workflows.Activities;
using OrchardCore.Workflows.Models;
using OrchardCore.Workflows.Services;
using StatCan.OrchardCore.GitHub.LiquidModel;
using StatCan.OrchardCore.GitHub.Services;

namespace StatCan.OrchardCore.GitHub.Activities
{
    public class CreateIssueTask : TaskActivity
    {
        private readonly IWorkflowExpressionEvaluator _expressionEvaluator;
        private readonly IGitHubApiService _gitHubApiService;
        private readonly ILogger _logger;
        private readonly IStringLocalizer S;

        public CreateIssueTask(
            IStringLocalizer<CommitFileTask> localizer,
            IWorkflowExpressionEvaluator expressionEvaluator,
            IGitHubApiService gitHubApiService,
            ILogger<CreateBranchTask> logger
        )
        {
            S = localizer;
            _expressionEvaluator = expressionEvaluator;
            _gitHubApiService = gitHubApiService;
            _logger = logger;
        }

        public override string Name => nameof(CreateIssueTask);

        public override LocalizedString DisplayText => S["GitHub - Create Issue"];

        public override LocalizedString Category => S["GitHub"];

        public WorkflowExpression<string> Owner
        {
            get => GetProperty(() => new WorkflowExpression<string>());
            set => SetProperty(value);
        }
        public WorkflowExpression<string> Repo
        {
            get => GetProperty(() => new WorkflowExpression<string>());
            set => SetProperty(value);
        }

        public WorkflowExpression<string> Title
        {
            get => GetProperty(() => new WorkflowExpression<string>());
            set => SetProperty(value);
        }
        public WorkflowExpression<string> Description
        {
            get => GetProperty(() => new WorkflowExpression<string>());
            set => SetProperty(value);
        }
        // commad separated list of labels
        public WorkflowExpression<string> Labels
        {
            get => GetProperty(() => new WorkflowExpression<string>());
            set => SetProperty(value);
        }

        public override IEnumerable<Outcome> GetPossibleOutcomes(WorkflowExecutionContext workflowContext, ActivityContext activityContext)
        {
            var outcomes = new List<Outcome>
            {
                new Outcome("IssueCreated", S["Issue created"]),
                new Outcome("Error", S["Error"])
            };

            return outcomes;
        }

        public override async Task<ActivityExecutionResult> ExecuteAsync(WorkflowExecutionContext workflowContext, ActivityContext activityContext)
        {
            try
            {
                var client = await _gitHubApiService.GetGitHubClient();

                var owner = await _expressionEvaluator.EvaluateAsync(Owner, workflowContext);
                var repo = await _expressionEvaluator.EvaluateAsync(Repo, workflowContext);
                var title = await _expressionEvaluator.EvaluateAsync(Title, workflowContext);
                var description = await _expressionEvaluator.EvaluateAsync(Description, workflowContext);
                var labels = await _expressionEvaluator.EvaluateAsync(Labels, workflowContext);

                var newIssue = new NewIssue(title) { Body = description };

                foreach (var label in labels.Split(","))
                {
                    newIssue.Labels.Add(label.Trim());
                }

                var issue = await client.Issue.Create(owner, repo, newIssue);
                workflowContext.LastResult = issue;
                return Outcomes("IssueCreated");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occured in CreateIssueTask");
                workflowContext.LastResult = new ErrorResult
                {
                    Message = ex.Message,
                    Exception = ex
                };
                return Outcomes("Error");
            }
        }
    }
}

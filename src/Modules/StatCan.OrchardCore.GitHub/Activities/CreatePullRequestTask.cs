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
    public class CreatePullRequestTask : TaskActivity
    {
        private readonly IWorkflowExpressionEvaluator _expressionEvaluator;
        private readonly IGitHubApiService _gitHubApiService;
        private readonly ILogger _logger;
        private readonly IStringLocalizer S;

        public CreatePullRequestTask(
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

        public override string Name => nameof(CreatePullRequestTask);

        public override LocalizedString DisplayText => S["GitHub - Create Pull Request"];

        public override LocalizedString Category => S["GitHub"];

        public string TokenName
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }
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
        // Name of the branch to commit to
        public WorkflowExpression<string> SourceBranch
        {
            get => GetProperty(() => new WorkflowExpression<string>());
            set => SetProperty(value);
        }

        public WorkflowExpression<string> TargetBranch
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

        public override IEnumerable<Outcome> GetPossibleOutcomes(WorkflowExecutionContext workflowContext, ActivityContext activityContext)
        {
            var outcomes = new List<Outcome>
            {
                new Outcome("PullRequestCreated", S["Pull Request created"]),
                new Outcome("Error", S["Error"])
            };

            return outcomes;
        }

        public override async Task<ActivityExecutionResult> ExecuteAsync(WorkflowExecutionContext workflowContext, ActivityContext activityContext)
        {
            try
            {
                var client = await _gitHubApiService.GetGitHubClient(TokenName);

                var owner = await _expressionEvaluator.EvaluateAsync(Owner, workflowContext);
                var repo = await _expressionEvaluator.EvaluateAsync(Repo, workflowContext);
                var sourceBranch = await _expressionEvaluator.EvaluateAsync(SourceBranch, workflowContext);
                var targetBranch = await _expressionEvaluator.EvaluateAsync(TargetBranch, workflowContext);
                var title = await _expressionEvaluator.EvaluateAsync(Title, workflowContext);
                var description = await _expressionEvaluator.EvaluateAsync(Description, workflowContext);


                var pr = await client.PullRequest.Create(owner, repo, new NewPullRequest(title, sourceBranch, targetBranch) { Body = description });
                workflowContext.LastResult = pr;
                return Outcomes("PullRequestCreated");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occured in CreatePullRequestTask");
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

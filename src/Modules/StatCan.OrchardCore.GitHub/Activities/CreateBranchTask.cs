using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Octokit.Helpers;
using OrchardCore.Workflows.Abstractions.Models;
using OrchardCore.Workflows.Activities;
using OrchardCore.Workflows.Models;
using OrchardCore.Workflows.Services;
using StatCan.OrchardCore.GitHub.LiquidModel;
using StatCan.OrchardCore.GitHub.Services;

namespace StatCan.OrchardCore.GitHub.Activities
{
    public class CreateBranchTask : TaskActivity
    {
        private readonly IWorkflowExpressionEvaluator _expressionEvaluator;
        private readonly IGitHubApiService _gitHubApiService;
        private readonly ILogger _logger;
        private readonly IStringLocalizer S;

        public CreateBranchTask(
            IStringLocalizer<CreateBranchTask> localizer,
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

        public override string Name => nameof(CreateBranchTask);

        public override LocalizedString DisplayText => S["GitHub - Create Branch"];

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
        // Name of the reference to create the branch from
        public WorkflowExpression<string> ReferenceName
        {
            get => GetProperty(() => new WorkflowExpression<string>());
            set => SetProperty(value);
        }
        // Target branch name
        public WorkflowExpression<string> TargetBranchName
        {
            get => GetProperty(() => new WorkflowExpression<string>());
            set => SetProperty(value);
        }


        public override IEnumerable<Outcome> GetPossibleOutcomes(WorkflowExecutionContext workflowContext, ActivityContext activityContext)
        {
            var outcomes = new List<Outcome>
            {
                new Outcome("BranchCreated", S["Branch created"]),
                new Outcome("Error", S["Error"])
            };

            return outcomes;
        }

        public override async Task<ActivityExecutionResult> ExecuteAsync(WorkflowExecutionContext workflowContext, ActivityContext activityContext)
        {
            try
            {
                var client = await _gitHubApiService.GetGitHubClient(TokenName);

                var owner = await _expressionEvaluator.EvaluateAsync(Owner, workflowContext, null);
                var repo = await _expressionEvaluator.EvaluateAsync(Repo, workflowContext, null);
                var referenceName = await _expressionEvaluator.EvaluateAsync(ReferenceName, workflowContext, null);
                var targetBranchName = await _expressionEvaluator.EvaluateAsync(TargetBranchName, workflowContext, null);

                // get the latest commit of the reference branch as a reference object
                var branchReference = await client.Git.Reference.Get(owner, repo, $"{referenceName}");

                // create the branch from the reference
                var newBranch = await client.Git.Reference.CreateBranch(owner, repo, targetBranchName, branchReference);
                workflowContext.LastResult = newBranch;
                return Outcomes("BranchCreated");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occured in CreateBranchTask");
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

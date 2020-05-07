using System;
using System.Collections.Generic;
using System.Linq;
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
    public class CommitFileTask : TaskActivity
    {
        private readonly IWorkflowExpressionEvaluator _expressionEvaluator;
        private readonly IGitHubApiService _gitHubApiService;
        private readonly ILogger _logger;
        private readonly IStringLocalizer S;

        public CommitFileTask(
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

        public override string Name => nameof(CommitFileTask);

        public override LocalizedString DisplayText => S["GitHub - Commit file"];

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
        // Name of the branch to commit to
        public WorkflowExpression<string> BranchName
        {
            get => GetProperty(() => new WorkflowExpression<string>());
            set => SetProperty(value);
        }

        public WorkflowExpression<string> FileName
        {
            get => GetProperty(() => new WorkflowExpression<string>());
            set => SetProperty(value);
        }

        public WorkflowExpression<string> FileContents
        {
            get => GetProperty(() => new WorkflowExpression<string>());
            set => SetProperty(value);
        }
        public WorkflowExpression<string> CommitMessage
        {
            get => GetProperty(() => new WorkflowExpression<string>());
            set => SetProperty(value);
        }

        public override IEnumerable<Outcome> GetPossibleOutcomes(WorkflowExecutionContext workflowContext, ActivityContext activityContext)
        {
            var outcomes = new List<Outcome>
            {
                new Outcome("FileCommitted", S["File committed"]),
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
                var branchName = await _expressionEvaluator.EvaluateAsync(BranchName, workflowContext);
                var fileName = await _expressionEvaluator.EvaluateAsync(FileName, workflowContext);
                var fileContents = await _expressionEvaluator.EvaluateAsync(FileContents, workflowContext);
                var commitMessage = await _expressionEvaluator.EvaluateAsync(CommitMessage, workflowContext);

                RepositoryContentChangeSet changeSet;
                try
                {
                    // Try to get the file, will throw if not found
                    var existingFile = await client.Repository.Content.GetAllContentsByRef(owner, repo, fileName, branchName);

                    // update the file
                    changeSet = await client.Repository.Content.UpdateFile(owner, repo, fileName,
                       new UpdateFileRequest(commitMessage, fileContents, existingFile.First().Sha, branchName));
                }
                catch (Octokit.NotFoundException)
                {
                    // Create the file if not found
                    changeSet = await client.Repository.Content.CreateFile(owner, repo, fileName, new CreateFileRequest(commitMessage, fileContents, branchName));
                   
                }
                workflowContext.LastResult = changeSet;
                return Outcomes("FileCommitted");


            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occured in CommitFileTask");
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

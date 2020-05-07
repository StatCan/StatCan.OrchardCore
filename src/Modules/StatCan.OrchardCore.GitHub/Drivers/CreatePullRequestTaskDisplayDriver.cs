using OrchardCore.Workflows.Display;
using OrchardCore.Workflows.Models;
using StatCan.OrchardCore.GitHub.Activities;
using StatCan.OrchardCore.GitHub.ViewModels;

namespace StatCan.OrchardCore.GitHub.Drivers
{
    public class CreatePullRequestTaskDisplayDriver : ActivityDisplayDriver<CreatePullRequestTask, CreatePullRequestTaskViewModel>
    {
        protected override void EditActivity(CreatePullRequestTask activity, CreatePullRequestTaskViewModel model)
        {
            model.Owner = activity.Owner.Expression;
            model.Repo = activity.Repo.Expression;
            model.SourceBranch = activity.SourceBranch.Expression;
            model.TargetBranch = activity.TargetBranch.Expression;
            model.Title = activity.Title.Expression;
            model.Description = activity.Description.Expression;
        }

        protected override void UpdateActivity(CreatePullRequestTaskViewModel model, CreatePullRequestTask activity)
        {
            activity.Owner = new WorkflowExpression<string>(model.Owner?.Trim());
            activity.Repo = new WorkflowExpression<string>(model.Repo?.Trim());
            activity.SourceBranch = new WorkflowExpression<string>(model.SourceBranch?.Trim());
            activity.TargetBranch = new WorkflowExpression<string>(model.TargetBranch?.Trim());
            activity.Title = new WorkflowExpression<string>(model.Title?.Trim());
            activity.Description = new WorkflowExpression<string>(model.Description);
        }
    }
}

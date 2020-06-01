using Microsoft.AspNetCore.Mvc.ActionConstraints;
using OrchardCore.Workflows.Display;
using OrchardCore.Workflows.Models;
using StatCan.OrchardCore.GitHub.Activities;
using StatCan.OrchardCore.GitHub.ViewModels;

namespace StatCan.OrchardCore.GitHub.Drivers
{
    public class CommitFileTaskDisplayDriver : ActivityDisplayDriver<CommitFileTask, CommitFileTaskViewModel>
    {
        protected override void EditActivity(CommitFileTask activity, CommitFileTaskViewModel model)
        {
            model.TokenName = activity.TokenName;
            model.Owner = activity.Owner.Expression;
            model.Repo = activity.Repo.Expression;
            model.BranchName = activity.BranchName.Expression;
            model.FileName = activity.FileName.Expression;
            model.CommitMessage = activity.CommitMessage.Expression;
            model.FileContents = activity.FileContents.Expression;
        }

        protected override void UpdateActivity(CommitFileTaskViewModel model, CommitFileTask activity)
        {
            activity.TokenName = model.TokenName?.Trim();
            activity.Owner = new WorkflowExpression<string>(model.Owner?.Trim());
            activity.Repo = new WorkflowExpression<string>(model.Repo?.Trim());
            activity.BranchName = new WorkflowExpression<string>(model.BranchName?.Trim());
            activity.FileName = new WorkflowExpression<string>(model.FileName?.Trim());
            activity.CommitMessage = new WorkflowExpression<string>(model.CommitMessage);
            activity.FileContents = new WorkflowExpression<string>(model.FileContents);
        }
    }
}

using OrchardCore.Workflows.Display;
using OrchardCore.Workflows.Models;
using StatCan.OrchardCore.GitHub.Activities;
using StatCan.OrchardCore.GitHub.ViewModels;

namespace StatCan.OrchardCore.GitHub.Drivers
{
    public class CreateBranchTaskDisplayDriver : ActivityDisplayDriver<CreateBranchTask, CreateBranchTaskViewModel>
    {
        protected override void EditActivity(CreateBranchTask activity, CreateBranchTaskViewModel model)
        {
            model.TokenName = activity.TokenName;
            model.Owner = activity.Owner.Expression;
            model.Repo = activity.Repo.Expression;
            model.ReferenceName = activity.ReferenceName.Expression;
            model.TargetBranchName = activity.TargetBranchName.Expression;
        }

        protected override void UpdateActivity(CreateBranchTaskViewModel model, CreateBranchTask activity)
        {
            activity.TokenName = model.TokenName?.Trim();
            activity.Owner = new WorkflowExpression<string>(model.Owner?.Trim());
            activity.Repo = new WorkflowExpression<string>(model.Repo?.Trim());
            activity.ReferenceName = new WorkflowExpression<string>(model.ReferenceName?.Trim());
            activity.TargetBranchName = new WorkflowExpression<string>(model.TargetBranchName?.Trim());
        }
    }
}

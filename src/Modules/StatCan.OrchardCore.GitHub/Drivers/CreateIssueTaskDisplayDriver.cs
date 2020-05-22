using OrchardCore.Workflows.Display;
using OrchardCore.Workflows.Models;
using StatCan.OrchardCore.GitHub.Activities;
using StatCan.OrchardCore.GitHub.ViewModels;

namespace StatCan.OrchardCore.GitHub.Drivers
{
    public class CreateIssueTaskDisplayDriver : ActivityDisplayDriver<CreateIssueTask, CreateIssueTaskViewModel>
    {
        protected override void EditActivity(CreateIssueTask activity, CreateIssueTaskViewModel model)
        {
            model.Owner = activity.Owner.Expression;
            model.Repo = activity.Repo.Expression;
            model.Title = activity.Title.Expression;
            model.Description = activity.Description.Expression;
            model.Labels = activity.Labels.Expression;
        }

        protected override void UpdateActivity(CreateIssueTaskViewModel model, CreateIssueTask activity)
        {
            activity.Owner = new WorkflowExpression<string>(model.Owner?.Trim());
            activity.Repo = new WorkflowExpression<string>(model.Repo?.Trim());
            activity.Title = new WorkflowExpression<string>(model.Title?.Trim());
            activity.Description = new WorkflowExpression<string>(model.Description);
            activity.Labels = new WorkflowExpression<string>(model.Labels.Trim());
        }
    }
}

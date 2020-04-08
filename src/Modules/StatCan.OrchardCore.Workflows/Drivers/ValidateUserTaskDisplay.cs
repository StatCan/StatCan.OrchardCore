using OrchardCore.Workflows.Display;
using StatCan.OrchardCore.Workflows.Tasks;
using StatCan.OrchardCore.Workflows.ViewModels;

namespace StatCan.OrchardCore.Workflows.Drivers
{
    public class ValidateUserTaskDisplay : ActivityDisplayDriver<ValidateUserTask, ValidateUserTaskViewModel>
    {
        protected override void EditActivity(ValidateUserTask activity, ValidateUserTaskViewModel model)
        {
            model.RoleNames = activity.RoleNames;
            model.SetUser = activity.SetUser;
        }

        protected override void UpdateActivity(ValidateUserTaskViewModel model, ValidateUserTask activity)
        {
            activity.RoleNames = model.RoleNames;
            activity.SetUser = model.SetUser;
        }
    }
}
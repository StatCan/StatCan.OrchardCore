using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using OrchardCore.Workflows.Abstractions.Models;
using OrchardCore.Workflows.Activities;
using OrchardCore.Workflows.Models;
using OrchardCore.Users;
using StatCan.OrchardCore.Workflows.ViewModels;

namespace StatCan.OrchardCore.Workflows.Tasks
{
    public class ValidateUserTask : TaskActivity
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<IUser> _userManager;

        public ValidateUserTask(UserManager<IUser> userManager, IHttpContextAccessor httpContextAccessor, IStringLocalizer<ValidateUserTask> localizer)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            T = localizer;
        }

        public IStringLocalizer T { get; }

        public override string Name => nameof(ValidateUserTask);
        public override LocalizedString Category => T["User"];

        public string RoleNames
        {
            get => GetProperty(() => string.Empty);
            set => SetProperty(value);
        }

        public bool SetUser
        {
            get => GetProperty(() => true);
            set => SetProperty(value);
        }

        public override LocalizedString DisplayText => T["Validate User Task"];

        public override IEnumerable<Outcome> GetPossibleOutcomes(WorkflowExecutionContext workflowContext, ActivityContext activityContext)
        {
            return Outcomes(T["Authenticated_InRole"], T["Authenticated"], T["Anonymous"]);
        }

        public override ActivityExecutionResult Execute(WorkflowExecutionContext workflowContext, ActivityContext activityContext)
        {
            var user = _httpContextAccessor.HttpContext.User;
            var isAuthenticated = user?.Identity?.IsAuthenticated;
            if (isAuthenticated.HasValue && isAuthenticated.Value)
            {
                if (SetUser)
                {
                    var userInfo = _userManager.GetUserAsync(user).GetAwaiter().GetResult();
                    var email = _userManager.GetEmailAsync(userInfo).GetAwaiter().GetResult();
                    workflowContext.Properties["User"] = new ValidateUserTaskPropertyViewModel(){ Name = user.Identity.Name, Email = email };
                }
                if (!string.IsNullOrEmpty(RoleNames))
                {
                    var roles = RoleNames.Replace(" ","").Split(',');
                    foreach(var role in roles)
                    {
                        if(user.HasClaim(ClaimTypes.Role, role))
                        {
                            return Outcomes("Authenticated_InRole");
                        }
                    }
                }

                return Outcomes("Authenticated");
            }
            return Outcomes("Anonymous");
        }


    }
}
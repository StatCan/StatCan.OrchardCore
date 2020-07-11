using Microsoft.Extensions.Localization;
using OrchardCore.ContentManagement;
using OrchardCore.Workflows.Abstractions.Models;
using OrchardCore.Workflows.Activities;
using OrchardCore.Workflows.Helpers;
using OrchardCore.Workflows.Models;
using System.Collections.Generic;
using System.Linq;

namespace StatCan.OrchardCore.AjaxForms.Workflows
{
    public class AjaxFormSubmittedEvent : Activity, IEvent
    {
        public const string InputKey = "AjaxForm";
        private readonly IStringLocalizer S;

        public AjaxFormSubmittedEvent(IStringLocalizer<AjaxFormSubmittedEvent> localizer)
        {
            S = localizer;
        }
        public override string Name => nameof(AjaxFormSubmittedEvent);

        public override LocalizedString DisplayText => S["AjaxForm Submitted Event"];

        public override LocalizedString Category => S["AjaxForm"];

        public override IEnumerable<Outcome> GetPossibleOutcomes(WorkflowExecutionContext workflowContext, ActivityContext activityContext)
        {
            return Outcomes(S["Done"]);
        }

        public bool ExecuteForAllForms
        {
            get => GetProperty(defaultValue: () => false);
            set => SetProperty(value);
        }

        // List of ContentItemId's of forms that can trigger this event
        public IList<string> AjaxFormIds
        {
            get => GetProperty<IList<string>>(defaultValue: () => new List<string>());
            set => SetProperty(value);
        }

        public override bool CanExecute(WorkflowExecutionContext workflowContext, ActivityContext activityContext)
        {
           var contentItem = GetForm(workflowContext);

            if(contentItem == null)
            {
                return false;
            }

            var cleanedIdList = AjaxFormIds.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            // empty list means all forms execute the workflow.
            return !cleanedIdList.Any() || cleanedIdList.Any(s => s == contentItem.ContentItem.ContentItemId);
        }
        public override ActivityExecutionResult Execute(WorkflowExecutionContext workflowContext, ActivityContext activityContext)
        {
            return Halt();
        }

        public override ActivityExecutionResult Resume(WorkflowExecutionContext workflowContext, ActivityContext activityContext)
        {
            return Outcomes("Done");
        }

        private IContent GetForm(WorkflowExecutionContext workflowContext)
        {
           // get the form from the workflow Input or Properties
            var content = workflowContext.Input.GetValue<IContent>(InputKey)
                ?? workflowContext.Properties.GetValue<IContent>(InputKey);

            if (content != null)
            {
                return content;
            }

            return null;
        }
    }
}

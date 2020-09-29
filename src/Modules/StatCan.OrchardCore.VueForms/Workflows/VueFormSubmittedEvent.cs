using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Linq;
using OrchardCore.ContentManagement;
using OrchardCore.Workflows.Abstractions.Models;
using OrchardCore.Workflows.Activities;
using OrchardCore.Workflows.Helpers;
using OrchardCore.Workflows.Models;

namespace StatCan.OrchardCore.VueForms.Workflows
{
    public class VueFormSubmittedEvent : Activity, IEvent
    {
        public const string InputKey = "VueForm";
        private readonly IStringLocalizer S;

        public VueFormSubmittedEvent(IStringLocalizer<VueFormSubmittedEvent> localizer)
        {
            S = localizer;
        }

        public override string Name => nameof(VueFormSubmittedEvent);

        public override LocalizedString DisplayText => S["VueForm Submitted Event"];

        public override LocalizedString Category => S["VueForm"];

        public override IEnumerable<Outcome> GetPossibleOutcomes(WorkflowExecutionContext workflowContext, ActivityContext activityContext)
        {
            return Outcomes(S["Done"]);
        }

        public bool ExecuteForAllForms
        {
            get => GetProperty(defaultValue: () => false);
            set => SetProperty(value);
        }

        // List of ContentItemId's of forms that trigger this event
        public IList<string> VueFormIds
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

            var cleanedIdList = VueFormIds.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
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

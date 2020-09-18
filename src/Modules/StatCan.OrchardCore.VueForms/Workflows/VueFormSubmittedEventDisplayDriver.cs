using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YesSql;
using OrchardCore.ContentManagement.Records;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Workflows.Display;

namespace StatCan.OrchardCore.VueForms.Workflows
{
    public class VueFormSubmittedEventDisplayDriver : ActivityDisplayDriver<VueFormSubmittedEvent, VueFormSubmittedEventViewModel>
    {
        private readonly ISession _session;
        public VueFormSubmittedEventDisplayDriver(ISession session)
        {
            _session = session;
        }
        protected async override ValueTask EditActivityAsync(VueFormSubmittedEvent activity, VueFormSubmittedEventViewModel model)
        {
            model.ExecuteForAllForms = activity.ExecuteForAllForms;
            model.AllItems = await GetForms(activity);
        }

        protected override void UpdateActivity(VueFormSubmittedEventViewModel model, VueFormSubmittedEvent activity)
        {
            activity.ExecuteForAllForms = model.ExecuteForAllForms;
            activity.VueFormIds = model.SelectedFormIds;
        }

        public override IDisplayResult Display(VueFormSubmittedEvent activity)
        {
            return Combine(
               Shape($"VueFormSubmittedEvent_Fields_Thumbnail", new VueFormSubmittedEventViewModel(activity)).Location("Thumbnail", "Content"),
               Factory($"VueFormSubmittedEvent_Fields_Design", async ctx =>
               {
                   var forms = await _session.QueryIndex<ContentItemIndex>(q => q.ContentType == "VueForm").ListAsync();

                   var shape = new VueFormSubmittedEventViewModel(activity);
                   shape.ExecuteForAllForms = activity.ExecuteForAllForms;

                   shape.AllItems = await GetForms(activity);

                   return shape;
               }).Location("Design", "Content")
           );
        }

        private async Task<IList<SelectListItem>> GetForms(VueFormSubmittedEvent activity)
        {
            var forms = await _session.QueryIndex<ContentItemIndex>(q => q.ContentType == "VueForm" && q.Latest).ListAsync();
            return forms.Select(f => new SelectListItem(f.DisplayText, f.ContentItemId, activity.VueFormIds.Contains(f.ContentItemId))).ToList();
        }
    }
}
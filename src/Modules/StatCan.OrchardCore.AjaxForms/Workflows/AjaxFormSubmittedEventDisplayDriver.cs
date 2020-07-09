using Microsoft.AspNetCore.Mvc.Rendering;
using OrchardCore.ContentManagement.Records;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Workflows.Display;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YesSql;

namespace StatCan.OrchardCore.AjaxForms.Workflows
{
    public class AjaxFormSubmittedEventDisplayDriver : ActivityDisplayDriver<AjaxFormSubmittedEvent, AjaxFormSubmittedEventViewModel>
    {
        private readonly ISession _session;
        public AjaxFormSubmittedEventDisplayDriver(ISession session)
        {
            _session = session;
        }
        public override IDisplayResult Display(AjaxFormSubmittedEvent activity)
        {
            return Combine(
               Shape($"AjaxFormSubmittedEvent_Fields_Thumbnail", new AjaxFormSubmittedEventViewModel(activity)).Location("Thumbnail", "Content"),
               Factory($"AjaxFormSubmittedEvent_Fields_Design", async ctx =>
               {
                   var forms = await _session.QueryIndex<ContentItemIndex>(q => q.ContentType == "AjaxForm").ListAsync();

                   var shape = new AjaxFormSubmittedEventViewModel(activity);
                   shape.ExecuteForAllForms = activity.ExecuteForAllForms;
                   shape.AllItems = forms.Select(f => new SelectListItem(f.DisplayText, f.ContentItemId, activity.AjaxFormIds.Contains(f.ContentItemId))).ToList();
                   return shape;
               }).Location("Design", "Content")
           );
        }
    }
}
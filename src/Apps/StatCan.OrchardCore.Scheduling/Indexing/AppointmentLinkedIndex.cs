using OrchardCore.ContentManagement;
using StatCan.OrchardCore.Scheduling.Models;
using System.Collections.Generic;
using System.Linq;
using YesSql.Indexes;

namespace StatCan.OrchardCore.Scheduling.Indexing
{
    public class AppointmentLinkedIndex : MapIndex
    {
        public string ContentItemId { get;set; }
        public string LinkedContentItemId { get; set; }
        public string CalendarTermContentItemId { get; set; }
    }

    public class AppointmentLinkedIndexProvider : IndexProvider<ContentItem>
    {
        public override void Describe(DescribeContext<ContentItem> context)
        {
            context.For<AppointmentLinkedIndex>()
                .Map(contentItem =>
                {
                    if (!contentItem.Published && !contentItem.Latest)
                    {
                        return null;
                    }

                    var part = contentItem.As<Appointment>();

                    if(part == null)
                    {
                        return null;
                    }

                    var results = new List<AppointmentLinkedIndex>();

                    if(part.LinkedContent == null || part.LinkedContent?.ContentItemIds == null)
                    {
                        return null;
                    }
                    foreach(var id in part.LinkedContent.ContentItemIds)
                    {
                        results.Add(new AppointmentLinkedIndex{
                            CalendarTermContentItemId = part.Calendar?.TermContentItemIds?.FirstOrDefault(),
                            ContentItemId = contentItem.ContentItemId,
                            LinkedContentItemId = id
                        });
                    }

                    return results;
                });
        }
    }
}

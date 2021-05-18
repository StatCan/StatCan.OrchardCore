using OrchardCore.ContentManagement;
using StatCan.OrchardCore.Scheduling.Models;
using System;
using System.Linq;
using YesSql.Indexes;

namespace StatCan.OrchardCore.Scheduling.Indexing
{
    public class AppointmentIndex : MapIndex
    {
        public string ContentItemId { get;set; }
        public TimeSpan? StartTime {get; set; }
        public int? StartDay { get; set; }
        public int? StartMonth { get; set; }
        public int? StartYear { get; set; }
        public TimeSpan? EndTime {get; set; }
        public int? EndDay { get; set; }
        public int? EndMonth { get; set; }
        public int? EndYear { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }
        public string LocationTermContentItemId { get; set; }
    }

    public class AppointmentIndexProvider : IndexProvider<ContentItem>
    {
        public override void Describe(DescribeContext<ContentItem> context)
        {
            context.For<AppointmentIndex>()
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

                    return new AppointmentIndex
                    {
                        ContentItemId = contentItem.ContentItemId,
                        LocationTermContentItemId = part.Location?.TermContentItemIds?.FirstOrDefault(),
                        StartTime = part.StartDate?.Value?.TimeOfDay,
                        StartDay = part.StartDate?.Value?.Day,
                        StartMonth = part.StartDate?.Value?.Month,
                        StartYear = part.StartDate?.Value?.Year,
                        StartDate = part.StartDate?.Value,
                        EndTime = part.EndDate?.Value?.TimeOfDay,
                        EndDay = part.EndDate?.Value?.Day,
                        EndMonth = part.EndDate?.Value?.Month,
                        EndYear = part.EndDate?.Value?.Year,
                        EndDate = part.EndDate?.Value,
                        Status = part.Status?.Text,
                    };
                });
        }
    }
}

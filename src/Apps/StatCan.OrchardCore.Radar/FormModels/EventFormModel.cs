using System;
using System.Collections.Generic;
namespace StatCan.OrchardCore.Radar.FormModels
{
    public class EventFormModel : EntityFormModel
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public ICollection<IDictionary<string, string>> Attendees { get; set; }
        public ICollection<IDictionary<string, string>> EventOrganizers { get; set; }
    }
}

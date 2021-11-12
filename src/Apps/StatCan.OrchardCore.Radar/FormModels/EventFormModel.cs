using System;
using System.Collections.Generic;
namespace StatCan.OrchardCore.Radar.FormModels
{
    public class EventFormModel : EntityFormModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<string> Attendees { get; set; }
        public ICollection<IDictionary<string, string>> EventOrganizers { get; set; }
    }
}

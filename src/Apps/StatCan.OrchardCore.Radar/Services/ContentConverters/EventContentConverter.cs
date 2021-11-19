using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using StatCan.OrchardCore.Radar.FormModels;

namespace StatCan.OrchardCore.Radar.Services.ContentConverters
{
    public class EventContentConverter : BaseContentConverter
    {
        public EventContentConverter(BaseContentConverterDependency baseContentConverterDependency) : base(baseContentConverterDependency)
        {

        }

        public override async Task<JObject> ConvertFromFormModelAsync(FormModel formModel, dynamic context)
        {
            EventFormModel eventFormModel = (EventFormModel)formModel;

            var eventContentObject = new
            {
                Published = GetPublishStatus(eventFormModel.PublishStatus),
                Event = new
                {
                    Attendees = new {
                        UserIds = MapDictListToStringList(eventFormModel.Attendees, attendee => attendee["value"].ToString()),
                        UserNames = MapDictListToStringList(eventFormModel.Attendees, attendee => attendee["label"].ToString()),
                    },
                    StartDate = new {
                        Value = DateTime.Parse($"{eventFormModel.StartDate} {eventFormModel.StartTime}")
                    },
                    EndDate = new {
                        Value = DateTime.Parse($"{eventFormModel.EndDate} {eventFormModel.EndTime}")
                    }
                },
                RadarEntityPart = new
                {
                    Name = new
                    {
                        Text = eventFormModel.Name
                    },
                    Description = new
                    {
                        Text = eventFormModel.Description
                    },
                    Topics = new
                    {
                        TaxonomyContentItemId = await GetTaxonomyIdAsync("Topics"),
                        TermContentItemIds = MapDictListToStringList(eventFormModel.Topics, topic => topic["value"]),
                        TagNames = MapDictListToStringList(eventFormModel.Topics, topic => topic["label"])
                    }
                },
                ContentPermissionsPart = new
                {
                    Enabled = true,
                    Roles = eventFormModel.Roles
                },
                EventOrganizer = new
                {
                    ContentItems = await GetMembersContentAsync(eventFormModel.EventOrganizers, "EventOrganizer", organizer =>
                    {
                        var organizerObject = new
                        {
                            EventOrganizer = new
                            {
                                Organizer = new
                                {
                                    UserIds = new string[] { organizer["value"] },
                                    UserNames = new string[] { organizer["label"] }
                                },
                                Role = new
                                {
                                    Text = ""
                                }
                            }
                        };

                        return organizerObject;
                    })
                }
            };

            return JObject.FromObject(eventContentObject);
        }
    }
}

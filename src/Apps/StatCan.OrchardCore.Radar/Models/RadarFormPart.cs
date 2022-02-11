using OrchardCore.ContentManagement;

namespace StatCan.OrchardCore.Radar.Models
{
    public class RadarFormPart : ContentPart
    {
        public string Id { get; set; }

        //This proprety holds the initial values for the form. This property is only filled when building the form.
        public string InitialValues { get; set; }

        // This property holds the static options for the form. This property is only filled when building the form.
        public string Options { get; set; }
    }
}

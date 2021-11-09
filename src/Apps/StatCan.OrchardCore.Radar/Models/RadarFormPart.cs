using System;
using OrchardCore.ContentManagement;

namespace StatCan.OrchardCore.Radar.Models
{
    public class RadarFormPart : ContentPart
    {
        /* This proprety holds the initial values for the form. This proprety is only filled when building the form.
        */
        public string InitialValues { get; set; }
    }
}

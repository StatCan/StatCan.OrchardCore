using System;
using OrchardCore.ContentManagement;

namespace StatCan.OrchardCore.Radar.Models
{
    public class RadarFormPart : ContentPart
    {
        /* This proprety holds the initial values for the form. If this property is null then it means
           the form is a create form.

           This proprety is only filled when building the form. It is not presisted.
        */
        #nullable enable
        public ContentItem? InitialValues { get; set; }
        #nullable disable
    }
}

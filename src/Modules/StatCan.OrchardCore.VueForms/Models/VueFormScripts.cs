using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace StatCan.OrchardCore.VueForms.Models
{
    public class VueFormScripts: ContentPart
    {
        public TextField ClientInitScript { get; set; }
        /// <summary>
        /// Script that runs everytime the form is submitted, to validate the values.
        /// </summary>
        public TextField OnValidation { get; set; }
        /// <summary>
        /// Script that runs after validation. you can
        /// </summary>
        public TextField OnSubmitted { get; set; }
    }
}

using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace StatCan.OrchardCore.AjaxForms.Models
{
    public class AjaxFormScripts: ContentPart
    {
        /// <summary>
        /// Script that runs everytime the form is submitted, to validate the values.
        /// </summary>
        public TextField OnValidation { get; set; }
        /// <summary>
        /// TODO: Implement Client side onChange script
        /// </summary>
        //public TextField OnChange { get; set; }

        /// <summary>
        /// Script that runs after validation. you can
        /// </summary>
        public TextField OnSubmitted { get; set; }
    }
}

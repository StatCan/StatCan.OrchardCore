using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace StatCan.OrchardCore.VueForms.Models
{
    public class VueFormScripts: ContentPart
    {
        /// <summary>
        /// Script that is executed client side before the form component is loaded.
        /// This is meant to be used as a way to initialize Global options for plugins
        /// </summary>
        public TextField ClientInit { get; set; }
        /// <summary>
        /// The VueModel script to merge with the form script
        /// </summary>
        /// <value></value>
        public TextField ComponentOptions { get; set; }

        /// <summary>
        /// Zod validation object used to validate the data client and server side
        /// </summary>
        public TextField ZodValidation { get; set; }
        /// <summary>
        /// Script that runs after validation. you can
        /// </summary>
        public TextField OnSubmitted { get; set; }
    }
}

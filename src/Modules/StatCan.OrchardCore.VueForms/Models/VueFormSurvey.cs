using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace StatCan.OrchardCore.VueForms.Models
{
    public class VueFormSurvey : ContentPart
    {
        public TextField SurveyJson { get; set; }
    }
}

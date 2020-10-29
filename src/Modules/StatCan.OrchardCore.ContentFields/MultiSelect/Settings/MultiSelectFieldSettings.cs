using System.ComponentModel.DataAnnotations.Schema;

namespace StatCan.OrchardCore.ContentFields.MultiSelect.Settings
{
    public class MultiSelectFieldSettings
    {
        public string Hint { get; set; }
        public string[] Options { get; set; }
    }
}

using System.Collections;
using System.Collections.Generic;
namespace StatCan.OrchardCore.Radar.FormModels
{
    public class EntityFormOptionModel : FormOptionModel
    {
        public ICollection<IDictionary<string, string>> TypeOptions { get; set; }
    }
}

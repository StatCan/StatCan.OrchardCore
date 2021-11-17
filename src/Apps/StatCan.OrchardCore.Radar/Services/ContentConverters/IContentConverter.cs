using Newtonsoft.Json.Linq;
using StatCan.OrchardCore.Radar.FormModels;

namespace StatCan.OrchardCore.Radar.Services.ContentConverters
{
    public interface IContentConverter
    {
        JObject ConvertFromFormModel(FormModel formModel, dynamic context);
    }
}

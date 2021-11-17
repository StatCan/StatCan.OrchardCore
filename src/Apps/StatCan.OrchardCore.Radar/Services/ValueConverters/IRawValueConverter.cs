using Newtonsoft.Json.Linq;
using StatCan.OrchardCore.Radar.FormModels;

namespace StatCan.OrchardCore.Radar.Services.ValueConverters
{
    /*
        The purpose of the converter is to convert the raw json from the forms into strongly typed form model
    */
    public interface IRawValueConverter
    {
        FormModel ConvertFromRawValues(JObject rawValues);
    }
}

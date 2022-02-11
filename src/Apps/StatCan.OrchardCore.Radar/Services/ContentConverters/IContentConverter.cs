using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using StatCan.OrchardCore.Radar.FormModels;

namespace StatCan.OrchardCore.Radar.Services.ContentConverters
{
    public interface IContentConverter
    {
        Task<JObject> ConvertAsync(FormModel formModel, dynamic context);
    }
}

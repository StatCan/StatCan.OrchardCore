using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OrchardCore.ResourceManagement;
using System.Threading.Tasks;

namespace StatCan.OrchardCore.Radar.Filters
{
    public class ResourceInjectionFilter : IAsyncResultFilter
    {
        private readonly IResourceManager _resourceManager;

        public ResourceInjectionFilter(IResourceManager resourceManager) => _resourceManager = resourceManager;

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (context.Result is PartialViewResult)
            {
                await next();

                return;
            }

            _resourceManager.RegisterResource("stylesheet", "Radar-styles");

            await next();
        }
    }
}

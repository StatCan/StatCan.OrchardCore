using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OrchardCore.ResourceManagement;
using System.Threading.Tasks;

namespace StatCan.OrchardCore.Radar.Filters
{
    /*
        Because Radar is implemented as a module and uses the Vuetify theme therefore Radar related
        stylesheets must be injected in the every view. In order to avoid redundancy, this filter 
        is used to inject Radar stylesheets on every view except admin.
    */
    public class ResourceInjectionFilter : IAsyncResultFilter
    {
        private readonly IResourceManager _resourceManager;

        public ResourceInjectionFilter(IResourceManager resourceManager) => _resourceManager = resourceManager;

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            string url =  context.HttpContext.Request.Path;
            // Don't inject into the admin views
            if (context.Result is PartialViewResult || url.ToLower().Contains("admin"))
            {
                await next();

                return;
            }

            _resourceManager.RegisterResource("stylesheet", "Radar-styles");

            await next();
        }
    }
}

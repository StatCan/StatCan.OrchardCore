using System;
using System.Threading.Tasks;
using Fluid;
using Fluid.Values;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Liquid;

namespace StatCan.OrchardCore.DisplayHelpers.EventHandlers
{
    /// <summary>
    /// Provides access to <see cref="HttpRequest"/> Items property if the template is running in
    /// a web request.
    /// </summary>
    public class HttpContextItemsEventHandler : ILiquidTemplateEventHandler
    {
        private readonly IServiceProvider _serviceProvider;
        private HttpContext _httpContext;

        static HttpContextItemsEventHandler()
        {
            TemplateContext.GlobalMemberAccessStrategy.Register<HttpContext, FluidValue>((context, name) =>
            {
                switch (name)
                {
                    case "Items": return new ObjectValue(context.Items);
                    default: return null;
                }
            });
        }

        public HttpContextItemsEventHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task RenderingAsync(TemplateContext context)
        {
            // Reuse the value as the service can be resolved by multiple templates
            _httpContext ??= _serviceProvider.GetService<IHttpContextAccessor>()?.HttpContext;
            if (_httpContext != null)
            {
                context.LocalScope.SetValue("HttpContext", _httpContext);
            }

            return Task.CompletedTask;
        }
    }
}

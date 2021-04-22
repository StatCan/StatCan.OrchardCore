using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using OrchardCore.Infrastructure.Html;
using OrchardCore.Scripting;

namespace StatCan.OrchardCore.Scripting
{
    public class HttpGlobalMethodsProvider : IGlobalMethodProvider
    {
        private readonly GlobalMethod _redirect;

        public HttpGlobalMethodsProvider(IHttpContextAccessor httpContextAccessor)
        {
            _redirect = new GlobalMethod
            {
                Name = "httpRedirect",
                Method = serviceProvider => (Action<String>)((url) =>
                {
                    if(url.StartsWith("~/"))
                    {
                        url = httpContextAccessor.HttpContext.Request.PathBase + url[1..];
                    }
                    if(url?.Length == 0)
                    {
                        url = httpContextAccessor.HttpContext.Request.PathBase;
                    }
                    httpContextAccessor.HttpContext.Response.Redirect(url);
                }
                )
            };
        }

        public IEnumerable<GlobalMethod> GetMethods()
        {
            return new[] { _redirect };
        }
    }
}

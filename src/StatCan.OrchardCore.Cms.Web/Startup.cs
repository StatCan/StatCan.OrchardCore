using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using StatCan.OrchardCore.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

namespace web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOrchardCms();
            services.Configure<IdentityOptions>(options =>
            {
               Configuration.GetSection("IdentityOptions").Bind(options);
            });
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.OnAppendCookie = cookieContext =>
                {
                    // Disabling same-site is required for external login provider support to properly set the user on the auth redirect
                    if (cookieContext.CookieName.StartsWith("orchauth_"))
                    {
                        cookieContext.CookieOptions.SameSite = SameSiteMode.None;
                    }
                };
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStatCanSecurityHeaders()
                .UseStaticFiles()
                .UseOrchardCore(builder=>builder
                    .UseStatCanCookiePolicy());
        }
    }
}

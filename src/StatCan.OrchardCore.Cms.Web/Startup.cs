using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using StatCan.OrchardCore.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using OrchardCore.Logging;
using Microsoft.AspNetCore.ResponseCompression;

namespace web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOrchardCms().ConfigureServices(tenantServices =>
                tenantServices.ConfigureHtmlSanitizer(sanitizer => sanitizer.AllowedSchemes.Add("mailto"))
            );
            // This configuration applies to all tenants.
            services.Configure<IdentityOptions>(options => Configuration.GetSection("IdentityOptions").Bind(options));
            services.Configure<GzipCompressionProviderOptions>(options => options.Level = System.IO.Compression.CompressionLevel.Optimal);
            services.AddResponseCompression(options =>
            {
                options.MimeTypes = new[]
                {
                    // General
                    "text/plain",
                    // Static files
                    "text/css",
                    "text/javascript",
                    "application/javascript",
                    // MVC
                    "text/html",
                    "application/xml",
                    "text/xml",
                    "application/json",
                    "text/json",
                    // Fonts
                    "font/otf",
                    "font/ttf",
                    "application/x-font",
                    "application/x-font-opentype",
                    "application/x-font-truetype",
                    "application/x-font-ttf",
                    // WebAssembly
                    "application/wasm",
                    // Custom
                    "image/svg+xml"
                };
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseResponseCompression();
            app.UseStatCanSecurityHeaders();
            app.UseStaticFiles();
            app.UseOrchardCore(c => c
                .UseSerilogTenantNameLogging()
                .UseStatCanCookiePolicy()
            );
        }
    }
}

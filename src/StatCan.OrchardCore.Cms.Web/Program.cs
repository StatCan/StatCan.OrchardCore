using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace web
{
    [SuppressMessage("csharp", "RCS1102", Justification = "Program file")]
    public class Program
    {
        public static Task Main(string[] args)
            => BuildHost(args).RunAsync();

        public static IHost BuildHost(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging => logging.ClearProviders())
                .UseSerilog((hostingContext, configBuilder) =>
                    {
                        configBuilder.ReadFrom.Configuration(hostingContext.Configuration)
                        .Enrich.FromLogContext();
                    })
                .ConfigureWebHostDefaults(webBuilder => webBuilder
                    .UseStartup<Startup>()
                ).Build();
    }
}

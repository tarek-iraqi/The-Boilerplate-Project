using Helpers.Constants;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Persistence.Context;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi
{
    public class Program
    {
        protected Program() { }

        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

                if (!env.IsEnvironment("Prelive") || !env.IsProduction())
                {
                    try
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                        dbContext.Database.Migrate();
                    }
                    catch (Exception ex)
                    {
                        logger.LogError($"Migration Error: {ex.Message}");
                    }
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((hostBuilder, config) => ConfigureSerilog(hostBuilder, config))
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static void ConfigureSerilog(HostBuilderContext hostBuilder, LoggerConfiguration config)
        {
            if (hostBuilder.HostingEnvironment.IsDevelopment())
            {
                config.MinimumLevel.Debug();
                config.MinimumLevel.Override("Microsoft", LogEventLevel.Information);
                config.Enrich.FromLogContext();
                config.Enrich.WithExceptionDetails();
                config.WriteTo.Console();
            }
            else if (hostBuilder.HostingEnvironment.IsStaging() ||
                    hostBuilder.HostingEnvironment.IsEnvironment("Demo"))
            {
                var stageLogsFolder = Path.Combine(hostBuilder.HostingEnvironment.ContentRootPath,
                    KeyValueConstants.StageLogsPath);

                var demoLogsFolder = Path.Combine(hostBuilder.HostingEnvironment.ContentRootPath,
                    KeyValueConstants.DemoLogsPath);

                if (!Directory.Exists(stageLogsFolder))
                {
                    Directory.CreateDirectory(stageLogsFolder);
                }

                if (!Directory.Exists(demoLogsFolder))
                {
                    Directory.CreateDirectory(demoLogsFolder);
                }

                config.MinimumLevel.Error();
                config.MinimumLevel.Override("Microsoft", LogEventLevel.Warning);
                config.Enrich.FromLogContext();
                config.Enrich.WithExceptionDetails();
                config.WriteTo.File(Path.Combine(
                 hostBuilder.HostingEnvironment.IsStaging() ? stageLogsFolder : demoLogsFolder,
                 $"{DateTime.Now.ToString("dd-MM-yyyy")}_logs.txt"));
            }
        }
    }
}

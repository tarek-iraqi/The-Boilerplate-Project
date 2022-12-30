using Application.Contracts;
using Hangfire;
using Helpers.Constants;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Configurations;
using WebApi.Extensions;
using WebApi.Filters;
using WebApi.Middlewares;

namespace WebApi;

public class Startup
{
    private readonly IWebHostEnvironment _env;

    public IConfiguration _configuration { get; }

    public Startup(IConfiguration configuration, IWebHostEnvironment env)
    {
        _configuration = configuration;
        _env = env;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.InstallServices(_configuration, typeof(IServiceInstaller).Assembly);
        services.AddHttpContextAccessor();
    }

    public void Configure(IApplicationBuilder app, IBackgroundCronJobs cronJobs)
    {
        RecurringJob.AddOrUpdate(() => cronJobs.HandleDomainEvents(), "* * * * *");

        app.UseRequestLocalization();

        app.UseHttpsRedirection();

        app.UseMiddleware<ErrorHandlingMiddleware>();

        app.UseCors(KeyValueConstants.AllowedCrosOrigins);

        app.UseStaticFiles();

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi");
            c.DefaultModelsExpandDepth(-1);
        });

        app.UseRouting();

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseWhen(context => context.Request.Path.StartsWithSegments("/api"), appBuilder =>
        {
            appBuilder.UseMiddleware<ApiKeyMiddleware>();
        });

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();

            endpoints.MapHangfireDashboard("/hf_dashboard", new DashboardOptions
            {
                AppPath = "/swagger/index.html",
                DashboardTitle = "App Background jobs",
                Authorization = new[] { new HangfireAuthorizationFilter() }
            });
        });
    }
}
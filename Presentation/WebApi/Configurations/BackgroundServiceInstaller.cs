using Application.Contracts;
using Hangfire;
using Hangfire.MySql;
using Helpers.Constants;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Transactions;
using WebApi.Services;

namespace WebApi.Configurations;

public class BackgroundServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHangfire(options => options
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseStorage(new MySqlStorage(configuration[KeyValueConstants.HangfireDbConnectionName],
                new MySqlStorageOptions
                {
                    TransactionIsolationLevel = IsolationLevel.ReadCommitted,
                    QueuePollInterval = TimeSpan.FromSeconds(15),
                    JobExpirationCheckInterval = TimeSpan.FromHours(1),
                    CountersAggregateInterval = TimeSpan.FromMinutes(5),
                    PrepareSchemaIfNecessary = true,
                    DashboardJobListLimit = 50000,
                    TransactionTimeout = TimeSpan.FromMinutes(1),
                    TablesPrefix = "hangfire_"
                })));

        services.AddHangfireServer();

        services.AddScoped<IBackgroundCronJobs, BackgroundCronJobs>();
    }
}
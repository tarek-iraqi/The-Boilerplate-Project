using Application.Contracts;
using Helpers.Abstractions;
using Helpers.Constants;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Common;
using Persistence.Context;
using Persistence.Identity;
using Persistence.Interceptors;
using System.Reflection;

namespace WebApi.Extensions;

public static partial class ServiceCollectionExtensions
{
    public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.AddInterceptors(new ConvertDomainEventsToOutboxMessagesInterceptor());
            options.UseMySql(configuration[KeyValueConstants.DbConnection],
                ServerVersion.AutoDetect(configuration[KeyValueConstants.DbConnection]));
        });

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddDataProtection().PersistKeysToDbContext<ApplicationDbContext>();
    }
}

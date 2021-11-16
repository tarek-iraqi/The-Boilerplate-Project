using Application.Interfaces;
using Helpers.Constants;
using Helpers.Interfaces;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Common;
using Persistence.Context;
using Persistence.Identity;
using System.Reflection;

namespace Persistence
{
    public static class ServiceCollectionExtensions
    {
        public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(configuration[KeyValueConstants.DbConnection],
                ServerVersion.AutoDetect(configuration[KeyValueConstants.DbConnection])).UseLazyLoadingProxies());

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddDataProtection().PersistKeysToDbContext<ApplicationDbContext>();
        }
    }
}

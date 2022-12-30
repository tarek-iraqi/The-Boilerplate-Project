using Helpers.Constants;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Configurations;

public class CorsOriginServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        var allowedCrosOrigins = configuration.GetSection("System:AllowedCrosOrigins").Get<string[]>();

        services.AddCors(config =>
        {
            config.AddPolicy(KeyValueConstants.AllowedCrosOrigins,
                p => p.SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithOrigins(allowedCrosOrigins)
                    .AllowCredentials());
        });
    }
}

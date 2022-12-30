using Helpers.Constants;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

namespace WebApi.Configurations;

public class LocalizationServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddLocalization();

        CultureInfo[] supportedCultures = new[]
        {
            new CultureInfo(KeyValueConstants.ArabicLanguageWithCulture),
            new CultureInfo(KeyValueConstants.EnglishLanguageWithCulture),
            new CultureInfo(KeyValueConstants.ArabicLanguage),
            new CultureInfo(KeyValueConstants.EnglishLanguage)
        };
        services.Configure<RequestLocalizationOptions>(options =>
        {
            options.DefaultRequestCulture = new RequestCulture(KeyValueConstants.EnglishLanguageWithCulture);
            options.SupportedUICultures = supportedCultures;
        });
    }
}

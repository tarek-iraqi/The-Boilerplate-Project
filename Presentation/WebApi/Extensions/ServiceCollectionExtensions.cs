using FluentValidation;
using Helpers.BaseModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using WebApi.Configurations;

namespace WebApi.Extensions;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection InstallServices(this IServiceCollection services, IConfiguration configuration,
        params Assembly[] assemblies)
    {
        var serviceInstallers = assemblies
            .SelectMany(a => a.DefinedTypes)
            .Where(IsAssignableToType<IServiceInstaller>)
            .Select(Activator.CreateInstance)
            .Cast<IServiceInstaller>();

        foreach (var installer in serviceInstallers)
        {
            installer.Install(services, configuration);
        }

        return services;

        static bool IsAssignableToType<T>(TypeInfo typeInfo) =>
            typeof(T).IsAssignableFrom(typeInfo) && !typeInfo.IsInterface && !typeInfo.IsAbstract;
    }
}
using Application;
using Application.Common;
using FluentValidation;
using FluentValidation.AspNetCore;
using Helpers.BaseModels;
using Helpers.Localization;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace WebApi.Configurations;

public class ControllersServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers()
            .AddDataAnnotationsLocalization(o =>
                o.DataAnnotationLocalizerProvider = (type, factory) => factory.Create(typeof(SharedResource)))
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = new List<ErrorResult>();

                    foreach (var key in context.ModelState.Keys)
                    {
                        var value = context.ModelState[key];
                        foreach (var error in value.Errors)
                        {
                            errors.Add(new ErrorResult { Type = key, Error = error.ErrorMessage });
                        }
                    }

                    var problemDetails = new Error
                    {
                        Errors = errors
                    };

                    return new UnprocessableEntityObjectResult(problemDetails)
                    {
                        ContentTypes = { "application/problem+json" }
                    };
                };
            });

        services.AddFluentValidationAutoValidation()
            .AddValidatorsFromAssembly(ApplicationAssemblyReference.Assembly)
            .AddFluentValidationClientsideAdapters();

        services.AddMediatR(config => config.RegisterServicesFromAssemblies(ApplicationAssemblyReference.Assembly));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PipelineValidationBehavior<,>));
    }
}

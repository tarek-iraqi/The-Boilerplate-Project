using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using WebApi.Extensions.Swagger;

namespace WebApi.Configurations;

public class SwaggerServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(c =>
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath, true);
            c.CustomSchemaIds(a => a.FullName);
            c.AddServer(new OpenApiServer { Url = configuration["System:Api_URL"] });
            c.OperationFilter<FileUploadOperationFilter>();
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "WebApi",
                License = new OpenApiLicense()
                {
                    Name = "MIT License",
                    Url = new Uri(configuration["License"])
                }
            });

            c.AddSecurityDefinition("Api-Key", new OpenApiSecurityScheme
            {
                Name = "X-API-Key",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "X-API-Key",
                BearerFormat = "string",
                Description = "Input your client api key",
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Api-Key",
                        },
                        Scheme = "X-API-Key",
                        Name = "X-API-Key",
                        In = ParameterLocation.Header,
                    }, new List<string>()
                },
            });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Description = "Input your Bearer token to access this API",
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer",
                        },
                        Scheme = "Bearer",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    }, new List<string>()
                },
            });


        });
    }
}

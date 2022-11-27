using Application;
using Application.Authorization;
using Application.Common;
using Application.Contracts;
using FluentValidation.AspNetCore;
using Hangfire;
using Hangfire.MySql;
using Helpers.Constants;
using Helpers.Models;
using Helpers.Resources;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebApi.Extensions.Swagger;
using WebApi.Filters;
using WebApi.Services;
using IsolationLevel = System.Transactions.IsolationLevel;

namespace WebApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddWebApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();
            services.AddMediatR(Assembly.GetAssembly(typeof(IApplicationLayer)));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PipelineValidationBehavior<,>));
            services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationRequirementHandler>();
            services.AddScoped<ApiResultFilterAttribute>();
            services.AddScoped<IBackgroundCronJobs, BackgroundCronJobs>();
        }

        public static void AddSwaggerService(this IServiceCollection services, IConfiguration configuration)
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

        public static void AddVersioningService(this IServiceCollection services)
        {
            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            });
        }

        public static void AddLocalizationService(this IServiceCollection services)
        {
            // services.AddJsonLocalization();
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

        public static void AddControllersService(this IServiceCollection services)
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

            services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
        }

        public static void AddCorsOriginService(this IServiceCollection services, IConfiguration configuration)
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

        public static void AddAuthenticationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(config =>
            {
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(config =>
            {
                config.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = KeyValueConstants.Issuer,
                    ValidAudience = KeyValueConstants.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>(KeyValueConstants.SecretHashKey)))
                };

                config.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query[KeyValueConstants.AccessToken];
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments(KeyValueConstants.SignalREndPoint))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });
        }

        public static void AddHangfireService(this IServiceCollection services, IConfiguration configuration)
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
        }
    }
}

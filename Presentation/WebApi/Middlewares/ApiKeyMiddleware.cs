using Application.Interfaces;
using Helpers.Constants;
using Helpers.Exceptions;
using Helpers.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Middlewares
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string APIKEYNAME = "x-api-key";
        private readonly IApplicationConfiguration _configuration;
        public ApiKeyMiddleware(RequestDelegate next, IApplicationConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (!_configuration.GetAppSettings().UrlsSkipApiKey.Contains(context.Request.Path.Value.ToLower()))
            {
                if (!context.Request.Headers.TryGetValue(APIKEYNAME, out var extractedApiKey))
                {
                    throw new AppCustomException(ErrorStatusCodes.InvalidHeader,
                        new List<Tuple<string, string>> { new Tuple<string, string>(APIKEYNAME, LocalizationKeys.MissingApiKey) });
                }

                var appConfig = context.RequestServices.GetRequiredService<IApplicationConfiguration>();

                var apiClients = appConfig.GetApiClients();

                if (!apiClients.Any(a => a.ApiKey.Equals(extractedApiKey)))
                {
                    throw new AppCustomException(ErrorStatusCodes.InvalidHeader,
                        new List<Tuple<string, string>> { new Tuple<string, string>(APIKEYNAME, LocalizationKeys.InvalidApiKey) });
                }
            }

            await _next(context);
        }
    }
}
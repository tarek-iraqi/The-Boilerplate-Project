using Domain.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Helpers.Exceptions;
using Helpers.Models;
using Helpers.Resources;

namespace WebApi.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public ErrorHandlingMiddleware(RequestDelegate next,
            ILogger<ErrorHandlingMiddleware> logger, IStringLocalizer<SharedResource> localizer)
        {
            _logger = logger;
            _localizer = localizer;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            object errors = null;

            switch (ex)
            {
                case AppCustomException re:
                    errors = new Error { Errors = LocalizeErrorMessages(re.Errors, re.ErrorPlaceholders) };
                    context.Response.StatusCode = (int)re.HttpCode;
                    break;
                case Exception e:
                    _logger.LogError(ex, ex.Message);
                    errors = new Error { Errors = new List<ErrorResult> { new ErrorResult { Type = "ERROR", Error = _localizer["INTERNAL_SERVER_ERROR"].Value } } };
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            context.Response.ContentType = "application/json";

            if (errors != null)
            {
                var result = JsonSerializer.Serialize(errors);

                await context.Response.WriteAsync(result);
            }
        }

        private List<ErrorResult> LocalizeErrorMessages(List<Tuple<string, string>> errors, Dictionary<int, string[]> errorPlaceholders)
        {
            List<ErrorResult> localizedErrors = new List<ErrorResult>();

            for (var i = 0; i < errors.Count; i++)
            {
                if (errorPlaceholders != null && errorPlaceholders.Any(a => a.Key == i))
                    localizedErrors.Add(new ErrorResult { Type = errors.ElementAt(i).Item1, Error = _localizer[errors.ElementAt(i).Item2, errorPlaceholders[i]].Value });
                else
                    localizedErrors.Add(new ErrorResult { Type = errors.ElementAt(i).Item1, Error = _localizer[errors.ElementAt(i).Item2].Value });
            }

            return localizedErrors;
        }
    }
}

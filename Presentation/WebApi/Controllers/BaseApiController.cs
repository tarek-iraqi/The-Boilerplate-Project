using Application.Contracts;
using Helpers.BaseModels;
using Helpers.Constants;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Net;

namespace WebApi.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/v{version:apiVersion}")]
    public abstract class BaseApiController : ControllerBase
    {
        protected IMediator _mediator => HttpContext.RequestServices.GetService<IMediator>();

        protected IApplicationLocalization _localizer =>
            HttpContext.RequestServices.GetService<IApplicationLocalization>();

        protected string _lang => HttpContext.Request.Headers["Accept-Language"].Count > 0
            ? HttpContext.Request.Headers["Accept-Language"][0]
            : KeyValueConstants.EnglishLanguageWithCulture;

        protected IActionResult EndpointResult(OperationResult operationResult)
        {
            if (operationResult.IsSuccess)
                return Ok(Result.Success(_localizer.Get(operationResult.Message)));

            return EndpointErrorResult(operationResult);
        }

        protected IActionResult EndpointResult<T>(OperationResult<T> operationResult)
        {
            if (operationResult.IsSuccess)
                return Ok(Result.Success(operationResult.Data));

            return EndpointErrorResult(operationResult);
        }

        protected IActionResult EndpointResult<T>(OperationResult<PaginatedResult<T>> operationResult)
        {
            if (operationResult.IsSuccess)
                return Ok(Result.Success(operationResult.Data));

            return EndpointErrorResult(operationResult);
        }

        private IActionResult EndpointErrorResult(OperationResult operationResult)
        {
            return operationResult.HttpStatusCode switch
            {
                HttpStatusCode.NotFound when operationResult.Errors.Any() =>
                    NotFound(Result.Fail(operationResult.Errors.Select(x => new ErrorResult
                    {
                        Type = x.Type,
                        Error = _localizer.Get(x.Error, x.ErrorPlaceholders ?? Array.Empty<string>())
                    }))),

                HttpStatusCode.NotFound when !string.IsNullOrWhiteSpace(operationResult.Message) =>
                    NotFound(Result.Fail(_localizer.Get(operationResult.Message))),

                HttpStatusCode.BadRequest when operationResult.Errors.Any() =>
                    BadRequest(Result.Fail(operationResult.Errors.Select(x => new ErrorResult
                    {
                        Type = x.Type,
                        Error = _localizer.Get(x.Error, x.ErrorPlaceholders ?? Array.Empty<string>())
                    }))),

                HttpStatusCode.BadRequest when !string.IsNullOrWhiteSpace(operationResult.Message) =>
                    BadRequest(Result.Fail(_localizer.Get(operationResult.Message))),

                _ => BadRequest()
            };
        }

    }
}

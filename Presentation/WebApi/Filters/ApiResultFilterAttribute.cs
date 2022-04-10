using Application.Contracts;
using Helpers.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Text.Json;

namespace WebApi.Filters
{
    public class ApiResultFilterAttribute : ActionFilterAttribute
    {
        private readonly IApplicationLocalization _localizer;

        public ApiResultFilterAttribute(IApplicationLocalization localizer)
        {
            _localizer = localizer;
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is JsonResult result)
            {
                if (result.Value is OperationResult resultObject)
                {
                    if (resultObject.IsSuccess)
                        SuccessResponse(context, resultObject);
                    else
                        ErrorResponse(context, resultObject);
                }

                base.OnActionExecuted(context);
            }
        }

        private void SuccessResponse(ActionExecutedContext context, OperationResult resultObject)
        {
            if (resultObject.Data != null)
            {
                var successResult = ResultWithData.Success(resultObject.Data);

                var newResult = new ContentResult()
                {
                    Content = JsonSerializer.Serialize(successResult),
                    StatusCode = (int)resultObject.HttpStatusCode,
                    ContentType = "application/json"
                };

                context.Result = newResult;
            }
            else if (!string.IsNullOrWhiteSpace(resultObject.Message))
            {
                var successResult = Result.Success(_localizer.Get(resultObject.Message));

                var newResult = new ContentResult()
                {
                    Content = JsonSerializer.Serialize(successResult),
                    StatusCode = (int)resultObject.HttpStatusCode,
                    ContentType = "application/json"
                };

                context.Result = newResult;
            }
            else
            {
                var newResult = new ContentResult()
                {
                    Content = string.Empty,
                    StatusCode = (int)resultObject.HttpStatusCode,
                    ContentType = "application/json"
                };

                context.Result = newResult;
            }
        }

        private void ErrorResponse(ActionExecutedContext context, OperationResult resultObject)
        {
            if (resultObject.Errors != null && resultObject.Errors.Length > 0)
            {
                List<ErrorResult> localizedErrors = new List<ErrorResult>();

                for (var i = 0; i < resultObject.Errors.Length; i++)
                {
                    localizedErrors.Add(
                        new ErrorResult
                        {
                            Type = resultObject.Errors[i].Type,
                            Error = _localizer.Get(resultObject.Errors[i].Error,
                                resultObject.Errors[i].ErrorPlaceholders ?? new string[] { })
                        });
                }

                var errors = new Error { Errors = localizedErrors };

                var newResult = new ContentResult()
                {
                    Content = JsonSerializer.Serialize(errors),
                    StatusCode = (int)resultObject.HttpStatusCode,
                    ContentType = "application/json"
                };

                context.Result = newResult;
            }
            else
            {
                var failResult = Result.Fail(_localizer.Get(resultObject.Message));

                var newResult = new ContentResult()
                {
                    Content = JsonSerializer.Serialize(failResult),
                    StatusCode = (int)resultObject.HttpStatusCode,
                    ContentType = "application/json"
                };

                context.Result = newResult;
            }
        }
    }
}

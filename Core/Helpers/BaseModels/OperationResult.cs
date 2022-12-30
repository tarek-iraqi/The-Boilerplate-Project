using Helpers.Localization;
using System;
using System.Net;

namespace Helpers.BaseModels;

public class OperationResult
{
    public bool IsSuccess { get; private init; }
    public bool IsFailed => !IsSuccess;
    public string Message { get; private init; }
    public OperationError[] Errors { get; private init; } = Array.Empty<OperationError>();
    public HttpStatusCode HttpStatusCode { get; private init; }

    public static OperationResult Success(string message = LocalizationKeys.OperationDoneSuccessfully) =>
        new()
        {
            IsSuccess = true,
            Message = message,
            HttpStatusCode = HttpStatusCode.OK
        };

    public static OperationResult<T> Success<T>(T data) =>
        new()
        {
            IsSuccess = true,
            Data = data,
            HttpStatusCode = HttpStatusCode.OK
        };

    public static OperationResult Fail(HttpStatusCode httpStatusCode, params OperationError[] errors) =>
        new()
        {
            IsSuccess = false,
            HttpStatusCode = httpStatusCode,
            Errors = errors
        };

    public static OperationResult<T> Fail<T>(HttpStatusCode httpStatusCode, params OperationError[] errors) =>
        new()
        {
            IsSuccess = false,
            HttpStatusCode = httpStatusCode,
            Errors = errors,
            Data = default
        };
}

public class OperationResult<T> : OperationResult
{
    public T Data { get; init; }
    public string RedirectUrl { get; set; }

    public static implicit operator OperationResult<T>(T data) => Success(data);
}

public class OperationError
{
    public string Type { get; private set; }
    public string Error { get; private set; }
    public string[] ErrorPlaceholders { get; private set; }

    private OperationError(string type, string error, string[] errorPlaceholders = null)
    {
        Type = type;
        Error = error;
        ErrorPlaceholders = errorPlaceholders;
    }
    public static OperationError Add(string type, string error, string[] errorPlaceholders = null) =>
        new(type, error, errorPlaceholders);
}

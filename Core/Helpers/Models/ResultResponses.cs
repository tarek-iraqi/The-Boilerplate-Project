using Helpers.Resources;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json.Serialization;

namespace Helpers.Models
{
    public class OperationResult
    {
        public bool IsSuccess { get; private set; }
        public string Message { get; private set; }
        public object Data { get; private set; }
        public OperationError[] Errors { get; private set; }
        public HttpStatusCode HttpStatusCode { get; private set; }

        private OperationResult(bool isSuccess, string message = LocalizationKeys.OperationDoneSuccessfully)
        {
            IsSuccess = isSuccess;
            Message = message;
            HttpStatusCode = HttpStatusCode.OK;
        }

        private OperationResult(bool isSuccess, object data)
        {
            IsSuccess = isSuccess;
            Data = data;
            HttpStatusCode = HttpStatusCode.OK;
        }

        private OperationResult(bool isSuccess, HttpStatusCode httpStatusCode, params OperationError[] errors)
        {
            IsSuccess = isSuccess;
            Errors = errors;
            HttpStatusCode = httpStatusCode;
        }

        public static OperationResult Success(string message) => new(true, message);
        public static OperationResult Success() => new(true);
        public static OperationResult Success<T>(T data) => new(true, data);
        public static OperationResult Fail(HttpStatusCode httpStatusCode, params OperationError[] errors) =>
            new(false, httpStatusCode, errors);
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


    public class Result
    {
        [JsonPropertyName("is_success")]
        public bool IsSuccess { get; private set; }

        [JsonPropertyName("message")]
        public string Message { get; private set; }

        private Result(bool isSuccess, string message = null)
        {
            IsSuccess = isSuccess;
            Message = message;
        }

        public static Result Success(string message) => new(true, message);
        public static Result Success() => new(true);
        public static Result Fail(string message) => new(false, message);
    }

    public class ResultWithData
    {
        [JsonPropertyName("data")]
        public object Data { get; private set; }

        private ResultWithData(object data)
        {
            Data = data;
        }
        public static ResultWithData Success(object data) => new(data);
    }

    public class Result<T>
    {
        [JsonPropertyName("data")]
        public T Data { get; }

        private Result(T data)
        {
            Data = data;
        }

        public static Result<T> ValueOf(T data) => new(data);
    }

    public class PaginatedResult<T>
    {
        [JsonPropertyName("meta")]
        public MetaData Meta { get; }

        [JsonPropertyName("data")]
        public IEnumerable<T> Data { get; }

        public PaginatedResult(IEnumerable<T> data, long count, int pageNumber, int pageSize)
        {
            Data = data;
            var totalNumberOfPages = Math.Ceiling((decimal)count / pageSize);
            Meta = new MetaData
            {
                Next = pageNumber < totalNumberOfPages,
                Previous = pageNumber > 1,
                TotalPages = (int)totalNumberOfPages,
                TotalRecords = count,
                PageIndex = pageNumber
            };
        }
    }

    public class MetaData
    {
        [JsonPropertyName("hasPrevious")]
        public bool Previous { get; set; }

        [JsonPropertyName("hasNext")]
        public bool Next { get; set; }

        [JsonPropertyName("totalPages")]
        public int TotalPages { get; set; }

        [JsonPropertyName("total")]
        public long TotalRecords { get; set; }

        [JsonPropertyName("pageIndex")]
        public int PageIndex { get; set; }
    }

    public class Error
    {
        [JsonPropertyName("errors")]
        public List<ErrorResult> Errors { get; set; }
    }

    public class ErrorResult
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("error")]
        public string Error { get; set; }
    }
}

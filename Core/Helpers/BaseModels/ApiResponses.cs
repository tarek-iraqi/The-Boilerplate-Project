using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Helpers.BaseModels;

public class Result
{
    public static ResultWithNoData Success(string message = null) =>
        new()
        {
            IsSuccess = true,
            Message = message,
        };

    public static Result<T> Success<T>(T data) =>
        new()
        {
            Data = data
        };

    public static PaginatedResult<T> Success<T>(PaginatedResult<T> data) => data;

    public static ResultWithNoData Fail(string message) =>
        new()
        {
            IsSuccess = false,
            Message = message,
        };

    public static Error Fail(IEnumerable<ErrorResult> errors) =>
        new()
        {
            Errors = errors
        };
}

public class ResultWithNoData
{
    [JsonPropertyName("is_success")]
    public bool IsSuccess { get; init; }

    [JsonPropertyName("message")]
    public string Message { get; init; }
}

public class Result<T> : Result
{
    [JsonPropertyName("data")]
    public T Data { get; init; }
}

public class PaginatedResult<T>
{
    [JsonPropertyName("meta")]
    public PagingMetaData Meta { get; init; }

    [JsonPropertyName("data")]
    public IEnumerable<T> Data { get; init; }

    public PaginatedResult() { }

    public PaginatedResult(IEnumerable<T> data, long count, int pageNumber, int pageSize)
    {
        Data = data;
        var totalNumberOfPages = Math.Ceiling((decimal)count / pageSize);
        Meta = new PagingMetaData
        {
            Next = pageNumber < totalNumberOfPages,
            Previous = pageNumber > 1,
            TotalPages = (int)totalNumberOfPages,
            TotalRecords = count,
            PageIndex = pageNumber
        };
    }
}

public class PagingMetaData
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
    public IEnumerable<ErrorResult> Errors { get; set; }
}

public class ErrorResult
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("error")]
    public string Error { get; set; }
}

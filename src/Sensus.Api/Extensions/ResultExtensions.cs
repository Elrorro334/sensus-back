using Microsoft.AspNetCore.Mvc;
using Sensus.Domain.Common;

namespace Sensus.Api.Extensions;

public static class ResultExtensions
{
    public static IResult ToHttpResult(this Result result)
    {
        if (result.IsSuccess)
        {
            return Results.NoContent();
        }

        return MapError(result.Error);
    }

    public static IResult ToHttpResult<TValue>(this Result<TValue> result)
    {
        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }

        return MapError(result.Error);
    }

    public static IResult ToCreatedAtResult<TValue>(this Result<TValue> result, string uri)
    {
        if (result.IsSuccess)
        {
            return Results.Created(uri, result.Value);
        }

        return MapError(result.Error);
    }

    private static IResult MapError(Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status400BadRequest
        };

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = error.Code,
            Detail = error.Message
        };

        return Results.Problem(problemDetails);
    }
}

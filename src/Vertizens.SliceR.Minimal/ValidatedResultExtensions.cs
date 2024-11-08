﻿using Microsoft.AspNetCore.Http;
using Vertizens.SliceR.Validated;

namespace Vertizens.SliceR.Minimal;
public static class ValidatedResultExtensions
{
    public static IResult ToHttpResult(this ValidatedResult validatedResult)
    {
        if (validatedResult.IsSuccessful)
        {
            return Results.StatusCode(GetStatusCode(validatedResult));
        }

        return validatedResult.ToUnsuccessfulResult();
    }

    public static IResult ToHttpResult<T>(this ValidatedResult<T> validatedResult)
    {
        if (validatedResult.IsSuccessful)
        {
            return Results.Ok(validatedResult.Result);
        }

        return validatedResult.ToUnsuccessfulResult();
    }

    public static IResult ToHttpResult(this ValidatedResult<FileResponse> validatedResult)
    {
        if (validatedResult.IsSuccessful)
        {
            return Results.File(validatedResult.Result!.Content, validatedResult.Result.ContentType, validatedResult.Result.Filename);
        }

        return validatedResult.ToUnsuccessfulResult();
    }

    private static IResult ToUnsuccessfulResult(this ValidatedResult validatedResult)
    {
        var statusCode = GetStatusCode(validatedResult);

        return statusCode switch
        {
            StatusCodes.Status404NotFound => Results.NotFound(),
            StatusCodes.Status403Forbidden => Results.Forbid(),
            _ => Results.ValidationProblem(validatedResult.Messages.ToDictionary(x => x.Key, x => x.Value.ToArray()), statusCode: statusCode),
        };
    }

    private static int GetStatusCode(ValidatedResult validatedResult)
    {
        int statusCode = StatusCodes.Status204NoContent;
        if (validatedResult.IsNotAuthorized)
        {
            statusCode = StatusCodes.Status403Forbidden;
        }
        else if (validatedResult.IsNotFound)
        {
            statusCode = StatusCodes.Status404NotFound;
        }
        else if (validatedResult.Messages != null && validatedResult.Messages.Count != 0)
        {
            statusCode = StatusCodes.Status400BadRequest;
        }

        return statusCode;
    }
}

using GuardRail.Core.Models.Exceptions;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;

namespace GuardRail.Api.Models;

public class GhControllerBase<TController>(
    ILogger<TController> logger)
    : ControllerBase
{
    protected readonly ILogger<TController> Logger = logger;

    public async Task<ApiResult> GhWrappedApiCall(
        Func<Task> function,
        IReadOnlyDictionary<Type, Func<Exception, ILogger<TController>, ApiResult>>? extraErrorHandlers = null)
    {
        try
        {
            await function();
            return Success();
        }
        catch (GuardRailExceptionBase e)
        {
            if (extraErrorHandlers?.TryGetValue(
                    e.GetType(),
                    out var errorHandler) == true)
            {
                return errorHandler(
                    e,
                    Logger);
            }

            Logger.LogError(
                e,
                e.Message);
            return Error(
                e.ExternalMessage);
        }
        catch (Exception e)
        {
            if (extraErrorHandlers?.TryGetValue(
                    e.GetType(),
                    out var errorHandler) == true)
            {
                return errorHandler(
                    e,
                    Logger);
            }

            Logger.LogError(
                e,
                e.Message);
            return Error(
                e.Message);
        }
    }

    public ApiResult<T> GhWrappedApiCall<T>(
        Func<T> function,
        IReadOnlyDictionary<Type, Func<Exception, ILogger<TController>, ApiResult<T>>>? extraErrorHandlers = null)
    {
        try
        {
            var result = function();
            return Ok(
                result);
        }
        catch (GuardRailExceptionBase e)
        {
            if (extraErrorHandlers?.TryGetValue(
                    e.GetType(),
                    out var errorHandler) == true)
            {
                return errorHandler(
                    e,
                    Logger);
            }

            Logger.LogError(
                e,
                e.Message);
            return Error<T>(
                e.ExternalMessage);
        }
        catch (Exception e)
        {
            if (extraErrorHandlers?.TryGetValue(
                    e.GetType(),
                    out var errorHandler) == true)
            {
                return errorHandler(
                    e,
                    Logger);
            }

            Logger.LogError(
                e,
                e.Message);
            return Error<T>(
                e.Message);
        }
    }

    public async Task<ApiResult<T>> GhWrappedApiCall<T>(
        Func<Task<T>> function,
        IReadOnlyDictionary<Type, Func<Exception, ILogger<TController>, ApiResult<T>>>? extraErrorHandlers = null)
    {
        try
        {
            var result = await function();
            return Ok(
                result);
        }
        catch (GuardRailExceptionBase e)
        {
            if (extraErrorHandlers?.TryGetValue(
                    e.GetType(),
                    out var errorHandler) == true)
            {
                return errorHandler(
                    e,
                    Logger);
            }

            Logger.LogError(
                e,
                e.Message);
            return Error<T>(
                e.ExternalMessage);
        }
        catch (Exception e)
        {
            if (extraErrorHandlers?.TryGetValue(
                    e.GetType(),
                    out var errorHandler) == true)
            {
                return errorHandler(
                    e,
                    Logger);
            }

            Logger.LogError(
                e,
                e.Message);
            return Error<T>(
                e.Message);
        }
    }

    public static ApiResult Success() =>
        new(
            StatusCodes.Status200OK);

    public static ApiResult<T> Ok<T>(
        T? item) =>
        new(
            item,
            StatusCodes.Status200OK);

    public static ApiResult<T> Unauthorized<T>(
        string errorMessage) =>
        new(
            default,
            StatusCodes.Status401Unauthorized);

    public static ApiResult Error(
        string errorMessage) =>
        new(
            StatusCodes.Status500InternalServerError);

    public static ApiResult<T> Error<T>(
        string errorMessage) =>
        new(
            default,
            StatusCodes.Status500InternalServerError);
}


public class ApiResult
    : ActionResult,
        IConvertToActionResult,
        IStatusCodeActionResult
{
    public ApiResult(
        int statusCode)
    {
        StatusCode = statusCode;
    }

    public IActionResult Convert() =>
        new ActionResult<object>(
                null)
            .Result;

    public int? StatusCode { get; }
}

public sealed class ApiResult<T>
    : ApiResult
{
    private readonly T? _item;

    public ApiResult(
        T? item,
        int statusCode)
        : base(
            statusCode)
    {
        _item = item;
    }
}
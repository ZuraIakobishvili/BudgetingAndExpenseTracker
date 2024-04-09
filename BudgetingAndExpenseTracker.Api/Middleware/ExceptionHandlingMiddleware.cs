using BudgetingAndExpenseTracker.Core.Exceptions;
using BudgetingAndExpenseTracker.Core.Services.Logger;
using Microsoft.AspNetCore.Mvc;

namespace BudgetingAndExpenseTracker.Infrastructure.Middleware;
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILoggerService _logger;
    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILoggerService logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }

        catch (InvalidRequestException exception)
        {
            _logger.LogError("{Message}, {StackTrace}", exception.Message, exception.StackTrace);
            var problemDetails = new ProblemDetails
            {
                Title = exception.Message,
                Status = StatusCodes.Status400BadRequest
            };

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }

        catch (UserNotFoundException exception)
        {
            _logger.LogError("{Message}, {StackTrace}", exception.Message, exception.StackTrace);
            var problemDetails = new ProblemDetails
            {
                Title = exception.Message,
                Status = StatusCodes.Status400BadRequest
            };

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }

        catch (InvalidExpenseException exception)
        {
            _logger.LogError("{Message}, {StackTrace}", exception.Message, exception.StackTrace);
            var problemDetails = new ProblemDetails
            {
                Title = exception.Message,
                Status = StatusCodes.Status400BadRequest
            };

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }

        catch (InvalidIncomeException exception)
        {
            _logger.LogError("{Message}, {StackTrace}", exception.Message, exception.StackTrace);
            var problemDetails = new ProblemDetails
            {
                Title = exception.Message,
                Status = StatusCodes.Status400BadRequest
            };

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }

        catch (InvalidLimitException exception)
        {
            _logger.LogError("{Message}, {StackTrace}", exception.Message, exception.StackTrace);
            var problemDetails = new ProblemDetails
            {
                Title = exception.Message,
                Status = StatusCodes.Status400BadRequest
            };

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }

        catch (InvalidForecastException exception)
        {
            _logger.LogError("{Message}, {StackTrace}", exception.Message, exception.StackTrace);
            var problemDetails = new ProblemDetails
            {
                Title = exception.Message,
                Status = StatusCodes.Status400BadRequest
            };

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }

        catch (Exception exception)
        {
            _logger.LogFatal("{Message}, {StackTrace}", exception.Message, exception.StackTrace);
            var problemDetails = new ProblemDetails
            {
                Title = exception.Message,
                Status = StatusCodes.Status500InternalServerError
            };

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}
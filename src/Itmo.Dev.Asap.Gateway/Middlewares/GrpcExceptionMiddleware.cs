using Grpc.Core;
using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models;
using System.Net;

namespace Itmo.Dev.Asap.Gateway.Middlewares;

public class GrpcExceptionMiddleware : IMiddleware
{
    private readonly ILogger<GrpcExceptionMiddleware> _logger;

    public GrpcExceptionMiddleware(ILogger<GrpcExceptionMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (RpcException e) when (e.StatusCode is not StatusCode.Internal)
        {
            HttpStatusCode code = e.StatusCode switch
            {
                StatusCode.OK => HttpStatusCode.OK,
                StatusCode.InvalidArgument => HttpStatusCode.BadRequest,
                StatusCode.DeadlineExceeded => HttpStatusCode.RequestTimeout,
                StatusCode.NotFound => HttpStatusCode.NotFound,
                StatusCode.AlreadyExists => HttpStatusCode.Conflict,
                StatusCode.PermissionDenied => HttpStatusCode.Forbidden,
                StatusCode.Unauthenticated => HttpStatusCode.Unauthorized,
                StatusCode.ResourceExhausted => HttpStatusCode.TooManyRequests,
                StatusCode.FailedPrecondition => HttpStatusCode.BadRequest,
                StatusCode.OutOfRange => HttpStatusCode.BadRequest,
                StatusCode.Unimplemented => HttpStatusCode.NotImplemented,
                StatusCode.Internal => HttpStatusCode.InternalServerError,
                StatusCode.Unavailable => HttpStatusCode.BadGateway,
                StatusCode.DataLoss => HttpStatusCode.InternalServerError,
                StatusCode.Cancelled => HttpStatusCode.InternalServerError,
                StatusCode.Unknown => HttpStatusCode.InternalServerError,
                StatusCode.Aborted => HttpStatusCode.InternalServerError,
                _ => HttpStatusCode.InternalServerError,
            };

            if (code is HttpStatusCode.InternalServerError)
            {
                _logger.LogError(e, "Failed to execute grpc request");
            }

            var details = new ErrorDetails(e.Status.Detail);

            context.Response.StatusCode = (int)code;
            await context.Response.WriteAsJsonAsync(details, context.RequestAborted);
        }
    }
}
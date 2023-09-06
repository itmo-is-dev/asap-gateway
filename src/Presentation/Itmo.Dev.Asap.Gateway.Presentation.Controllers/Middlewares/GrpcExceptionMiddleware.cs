using Grpc.Core;
using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models;
using System.Net;

namespace Itmo.Dev.Asap.Gateway.Presentation.Controllers.Middlewares;

public class GrpcExceptionMiddleware : IMiddleware
{
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
                _ => throw new ArgumentOutOfRangeException(),
            };

            var details = new ErrorDetails(e.Status.Detail);

            context.Response.StatusCode = (int)code;
            await context.Response.WriteAsJsonAsync(details, context.RequestAborted);
        }
    }
}
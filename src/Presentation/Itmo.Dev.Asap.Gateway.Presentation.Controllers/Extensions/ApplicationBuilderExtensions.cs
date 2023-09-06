using Itmo.Dev.Asap.Gateway.Presentation.Controllers.Middlewares;

namespace Itmo.Dev.Asap.Gateway.Presentation.Controllers.Extensions;

public static class ApplicationBuilderExtensions
{
    public static void AddControllersMiddleware(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<GrpcExceptionMiddleware>();
    }
}
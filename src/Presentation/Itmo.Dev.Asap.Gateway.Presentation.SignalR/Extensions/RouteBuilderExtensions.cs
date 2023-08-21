using Itmo.Dev.Asap.Gateway.Presentation.SignalR.Queue;

namespace Itmo.Dev.Asap.Gateway.Presentation.SignalR.Extensions;

public static class RouteBuilderExtensions
{
    public static void UseSignalrPresentation(this IEndpointRouteBuilder builder)
    {
        builder.MapHub<QueueHub>("hubs/queue");
    }
}
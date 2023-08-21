using Itmo.Dev.Asap.Gateway.Presentation.SignalR.Queue;

namespace Itmo.Dev.Asap.Gateway.Presentation.SignalR.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSignalrPresentation(this IServiceCollection collection)
    {
        collection.AddSignalR().AddMessagePackProtocol();
        collection.AddHostedService<QueueBackgroundService>();

        return collection;
    }
}
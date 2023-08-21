using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Queue;

namespace Itmo.Dev.Asap.Gateway.Presentation.SignalR.Queue;

public interface IQueueHubClient
{
    Task SendUpdateQueueMessage(QueueUpdatedMessage queue);

    Task SendError(string message);
}
using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Queue;

namespace Itmo.Dev.Asap.Gateway.Sdk.Clients;

public interface IQueueClient
{
    IObservable<QueueUpdatedMessage> QueueUpdates { get; }

    IObservable<string> Errors { get; }

    Task SubscribeToQueueUpdates(Guid subjectCourseId, Guid studentGroupId, CancellationToken cancellationToken);

    Task UnsubscribeFromQueueUpdates(Guid subjectCourseId, Guid studentGroupId, CancellationToken cancellationToken);
}
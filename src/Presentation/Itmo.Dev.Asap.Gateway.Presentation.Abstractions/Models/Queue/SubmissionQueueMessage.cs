using MessagePack;

namespace Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Queue;

[MessagePackObject]
public record SubmissionQueueMessage(
    [property: Key(0)] IReadOnlyDictionary<Guid, StudentMessage> Students,
    [property: Key(1)] IReadOnlyCollection<SubmissionMessage> Submissions);
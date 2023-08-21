using MessagePack;

namespace Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Queue;

[MessagePackObject]
public record QueueUpdatedMessage(
    [property: Key(0)] Guid SubjectCourseId,
    [property: Key(1)] Guid StudentGroupId,
    [property: Key(2)] string StudentGroupName,
    [property: Key(3)] SubmissionQueueMessage SubmissionQueue);
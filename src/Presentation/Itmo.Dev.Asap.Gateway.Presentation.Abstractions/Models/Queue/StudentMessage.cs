using MessagePack;

namespace Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Queue;

[MessagePackObject]
public record StudentMessage(
    [property: Key(0)] UserMessage User,
    [property: Key(1)] Guid? GroupId,
    [property: Key(2)] string GroupName);
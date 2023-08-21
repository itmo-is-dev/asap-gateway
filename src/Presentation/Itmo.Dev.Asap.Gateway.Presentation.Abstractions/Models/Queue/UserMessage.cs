using MessagePack;

namespace Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Queue;

[MessagePackObject]
public record UserMessage(
    [property: Key(0)] Guid Id,
    [property: Key(1)] string FirstName,
    [property: Key(2)] string MiddleName,
    [property: Key(3)] string LastName,
    [property: Key(4)] int? UniversityId,
    [property: Key(5)] string? GithubUsername);
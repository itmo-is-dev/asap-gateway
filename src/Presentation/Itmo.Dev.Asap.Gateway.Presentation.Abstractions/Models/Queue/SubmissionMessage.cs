using Itmo.Dev.Asap.Gateway.Application.Dto.Study;
using MessagePack;

namespace Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Queue;

[MessagePackObject]
public record SubmissionMessage(
    [property: Key(0)] Guid Id,
    [property: Key(1)] Guid StudentId,
    [property: Key(2)] DateTime? SubmissionDate,
    [property: Key(3)] string Payload,
    [property: Key(4)] string AssignmentShortName,
    [property: Key(5)] SubmissionStateDto State);
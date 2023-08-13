namespace Itmo.Dev.Asap.Gateway.Application.Dto.Tables;

public record struct SubmissionsQueueDto(string GroupName, IReadOnlyList<QueueSubmissionDto> Submissions);
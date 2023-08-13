namespace Itmo.Dev.Asap.Gateway.Application.Dto.Study;

public record SubmissionDto(
    Guid Id,
    int Code,
    DateTime SubmissionDate,
    Guid StudentId,
    Guid AssignmentId,
    string Payload,
    double? ExtraPoints,
    double? Points,
    string AssignmentShortName,
    SubmissionStateDto State);
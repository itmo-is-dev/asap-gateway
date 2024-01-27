namespace Itmo.Dev.Asap.Gateway.Application.Dto.Checking;

public record CheckingResultDto(
    Guid AssignmentId,
    string AssignmentName,
    CheckingResultSubmissionDto FirstSubmission,
    CheckingResultSubmissionDto SecondSubmission,
    double SimilarityScore);
namespace Itmo.Dev.Asap.Gateway.Application.Dto.Checking;

public record CheckingResultDto(
    string AssignmentName,
    CheckingResultSubmissionDto FirstSubmission,
    CheckingResultSubmissionDto SecondSubmission,
    double SimilarityScore);
namespace Itmo.Dev.Asap.Gateway.Application.Dto.Checking;

public record CheckingResultSubmissionDto(
    Guid SubmissionId,
    Guid StudentId,
    string FirstName,
    string LastName,
    string GroupName);
namespace Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Checking;

public record GetCheckingResultsRequest(
    long CheckingId,
    IEnumerable<Guid> AssignmentIds,
    IEnumerable<Guid> GroupIds,
    int PageSize,
    string? PageToken);
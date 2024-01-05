namespace Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Checking;

public record GetCheckingResultsRequest(long CheckingId, int PageSize, string? PageToken);
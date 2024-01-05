namespace Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Checking;

public record GetCheckingCodeBlocksRequest(
    long CheckingId,
    Guid FirstSubmissionId,
    Guid SecondSubmissionId,
    int PageSize,
    int Cursor);
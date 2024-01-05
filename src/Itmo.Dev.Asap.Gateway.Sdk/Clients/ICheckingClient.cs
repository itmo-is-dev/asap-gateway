using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Checking;
using Refit;

namespace Itmo.Dev.Asap.Gateway.Sdk.Clients;

public interface ICheckingClient
{
    [Post("/api/checking")]
    Task<ApiResponse<GetCheckingsResponse>> GetCheckingsAsync(
        [Body] GetCheckingsRequest request,
        CancellationToken cancellationToken);

    [Post("/api/checking/result")]
    Task<ApiResponse<GetCheckingResultsResponse>> GetCheckingResultsAsync(
        [Body] GetCheckingResultsRequest request,
        CancellationToken cancellationToken);

    [Post("/api/checking/codeBlocks")]
    Task<ApiResponse<GetCheckingCodeBlocksResponse>> GetCheckingCodeBlocksAsync(
        [Body] GetCheckingCodeBlocksRequest request,
        CancellationToken cancellationToken);
}
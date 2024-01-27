using Itmo.Dev.Asap.Gateway.Application.Dto.Checking;
using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Checking;
using Refit;

namespace Itmo.Dev.Asap.Gateway.Sdk.Clients;

public interface ICheckingClient
{
    [Post("/api/checking")]
    Task<IApiResponse<GetCheckingsResponse>> GetCheckingsAsync(
        [Body] GetCheckingsRequest request,
        CancellationToken cancellationToken);

    [Post("/api/checking/results")]
    Task<IApiResponse<GetCheckingResultsResponse>> GetCheckingResultsAsync(
        [Body] GetCheckingResultsRequest request,
        CancellationToken cancellationToken);

    [Post("/api/checking/codeBlocks")]
    Task<IApiResponse<GetCheckingCodeBlocksResponse>> GetCheckingCodeBlocksAsync(
        [Body] GetCheckingCodeBlocksRequest request,
        CancellationToken cancellationToken);

    [Post("/api/checking/start")]
    Task<IApiResponse<CheckingDto>> StartAsync(
        [Body] StartCheckingRequest request,
        CancellationToken cancellationToken);
}
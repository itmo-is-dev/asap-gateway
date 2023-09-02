using Itmo.Dev.Asap.Gateway.Application.Dto.Github;
using Refit;

namespace Itmo.Dev.Asap.Gateway.Sdk.Clients;

public interface IGithubSearchClient
{
    [Get("/api/github/search/organizations")]
    Task<IApiResponse<IReadOnlyCollection<GithubOrganizationDto>>> SearchOrganizationsAsync(
        [Query] string query,
        CancellationToken cancellationToken);

    [Get("/api/github/search/organizations/{organizationId}/repositories")]
    Task<IApiResponse<IReadOnlyCollection<GithubRepositoryDto>>> SearchRepositoriesAsync(
        long organizationId,
        [Query] string query,
        CancellationToken cancellationToken);

    [Get("/api/github/search/organizations/{organizationId}/teams")]
    Task<IApiResponse<IReadOnlyCollection<GithubTeamDto>>> SearchTeamsAsync(
        long organizationId,
        [Query] string query,
        CancellationToken cancellationToken);
}
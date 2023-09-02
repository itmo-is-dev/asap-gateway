using Itmo.Dev.Asap.Gateway.Application.Dto.Github;
using Itmo.Dev.Asap.Gateway.Github.Mapping;
using Itmo.Dev.Asap.Gateway.Presentation.Authorization;
using Itmo.Dev.Asap.Github.Search;
using Microsoft.AspNetCore.Mvc;

namespace Itmo.Dev.Asap.Gateway.Presentation.Controllers;

[ApiController]
[Route("api/github/search")]
public class GithubSearchController : ControllerBase
{
    public const string Scope = "GithubSearch";

    private readonly GithubSearchService.GithubSearchServiceClient _client;

    public GithubSearchController(GithubSearchService.GithubSearchServiceClient client)
    {
        _client = client;
    }

    [AuthorizeFeature(Scope, nameof(SearchGithubOrganizations))]
    [HttpGet("organizations")]
    public async Task<ActionResult<IEnumerable<GithubOrganizationDto>>> SearchGithubOrganizations(string? query = null)
    {
        if (string.IsNullOrEmpty(query))
            return Ok(Enumerable.Empty<GithubOrganizationDto>());

        var request = new SearchOrganizationsRequest { Query = query };

        SearchOrganizationsResponse response = await _client
            .SearchOrganizationsAsync(request, cancellationToken: HttpContext.RequestAborted);

        IEnumerable<GithubOrganizationDto> organizations = response.Organizations.Select(x => x.MapToDto());

        return Ok(organizations);
    }

    [AuthorizeFeature(Scope, nameof(SearchGithubRepositories))]
    [HttpGet("organizations/{organizationId:long}/repositories")]
    public async Task<ActionResult<IEnumerable<GithubRepositoryDto>>> SearchGithubRepositories(
        long organizationId,
        string? query = null)
    {
        if (string.IsNullOrEmpty(query))
            return Ok(Enumerable.Empty<GithubRepositoryDto>());

        var request = new SearchRepositoriesRequest
        {
            OrganizationId = organizationId,
            Query = query,
        };

        SearchRepositoriesResponse response = await _client
            .SearchRepositoriesAsync(request, cancellationToken: HttpContext.RequestAborted);

        IEnumerable<GithubRepositoryDto> repositories = response.Repositories.Select(x => x.MapToDto());

        return Ok(repositories);
    }

    [AuthorizeFeature(Scope, nameof(SearchGithubTeams))]
    [HttpGet("organizations/{organizationId:long}/teams")]
    public async Task<ActionResult<IEnumerable<GithubTeamDto>>> SearchGithubTeams(
        long organizationId,
        string? query = null)
    {
        if (string.IsNullOrEmpty(query))
            return Ok(Enumerable.Empty<GithubTeamDto>());

        var request = new SearchTeamsRequest
        {
            OrganizationId = organizationId,
            Query = query,
        };

        SearchTeamsResponse response = await _client
            .SearchTeamsAsync(request, cancellationToken: HttpContext.RequestAborted);

        IEnumerable<GithubTeamDto> teams = response.Teams.Select(x => x.MapToDto());

        return Ok(teams);
    }
}
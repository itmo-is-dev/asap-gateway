using Itmo.Dev.Asap.Gateway.Application.Abstractions.Enrichment;
using Itmo.Dev.Asap.Gateway.Application.Abstractions.Enrichment.Builders;
using Itmo.Dev.Asap.Gateway.Application.Dto.Users;
using Itmo.Dev.Asap.Github.Models;
using Itmo.Dev.Asap.Github.Users;

namespace Itmo.Dev.Asap.Gateway.Github.Enrichers;

public class GithubUserEnricher : IEntityEnricher<string, UserDtoBuilder, UserDto>
{
    private readonly GithubUserService.GithubUserServiceClient _client;

    public GithubUserEnricher(GithubUserService.GithubUserServiceClient client)
    {
        _client = client;
    }

    public async Task EnrichAsync(
        IEnrichmentContext<string, UserDtoBuilder, UserDto> context,
        CancellationToken cancellationToken)
    {
        var request = new FindByIdsRequest { UserIds = { context.Ids } };
        FindByIdsResponse response = await _client.FindByIdsAsync(request, cancellationToken: cancellationToken);

        foreach (GithubUser user in response.Users)
        {
            context[user.UserId].GithubUsername = user.Username;
        }
    }
}
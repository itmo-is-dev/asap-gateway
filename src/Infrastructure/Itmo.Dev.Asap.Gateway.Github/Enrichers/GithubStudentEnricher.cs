using Itmo.Dev.Asap.Gateway.Application.Abstractions.Enrichment;
using Itmo.Dev.Asap.Gateway.Application.Abstractions.Enrichment.Builders;
using Itmo.Dev.Asap.Gateway.Application.Dto.Users;
using Itmo.Dev.Asap.Github.Models;
using Itmo.Dev.Asap.Github.Users;

namespace Itmo.Dev.Asap.Gateway.Github.Enrichers;

public class GithubStudentEnricher : IEntityEnricher<string, StudentDtoBuilder, StudentDto>
{
    private readonly GithubUserService.GithubUserServiceClient _client;

    public GithubStudentEnricher(GithubUserService.GithubUserServiceClient client)
    {
        _client = client;
    }

    public async Task EnrichAsync(
        IEnrichmentContext<string, StudentDtoBuilder, StudentDto> context,
        CancellationToken cancellationToken)
    {
        var request = new FindByIdsRequest { UserIds = { context.Ids } };
        FindByIdsResponse response = await _client.FindByIdsAsync(request, cancellationToken: cancellationToken);

        foreach (GithubUser user in response.Users)
        {
            context[user.UserId].User.GithubUsername = user.Username;
        }
    }
}
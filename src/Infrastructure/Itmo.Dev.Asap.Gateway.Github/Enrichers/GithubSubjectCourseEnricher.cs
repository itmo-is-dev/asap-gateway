using Itmo.Dev.Asap.Gateway.Application.Abstractions.Enrichment;
using Itmo.Dev.Asap.Gateway.Application.Abstractions.Enrichment.Builders;
using Itmo.Dev.Asap.Gateway.Application.Dto.SubjectCourses;
using Itmo.Dev.Asap.Gateway.Github.Mapping;
using Itmo.Dev.Asap.Github.Models;
using Itmo.Dev.Asap.Github.SubjectCourses;

namespace Itmo.Dev.Asap.Gateway.Github.Enrichers;

public class GithubSubjectCourseEnricher : IEntityEnricher<string, SubjectCourseDtoBuilder, SubjectCourseDto>
{
    private readonly GithubSubjectCourseService.GithubSubjectCourseServiceClient _client;

    public GithubSubjectCourseEnricher(GithubSubjectCourseService.GithubSubjectCourseServiceClient client)
    {
        _client = client;
    }

    public async Task EnrichAsync(
        IEnrichmentContext<string, SubjectCourseDtoBuilder, SubjectCourseDto> context,
        CancellationToken cancellationToken)
    {
        var request = new FindByIdsRequest { SubjectCourseIds = { context.Ids } };
        FindByIdsResponse response = await _client.FindByIdsAsync(request, cancellationToken: cancellationToken);

        foreach (GithubSubjectCourse subjectCourse in response.SubjectCourses)
        {
            context[subjectCourse.Id].AddAssociation(subjectCourse.MapToAssociation());
        }
    }
}
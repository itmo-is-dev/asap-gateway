using Itmo.Dev.Asap.Gateway.Application.Abstractions.Enrichment;
using Itmo.Dev.Asap.Gateway.Application.Abstractions.Enrichment.Builders;
using Itmo.Dev.Asap.Gateway.Application.Dto.SubjectCourses;
using Itmo.Dev.Asap.Gateway.Google.Mapping;
using Itmo.Dev.Asap.Google;

namespace Itmo.Dev.Asap.Gateway.Google.Enrichers;

public class GoogleSubjectCourseEnricher : IEntityEnricher<string, SubjectCourseDtoBuilder, SubjectCourseDto>
{
    private readonly GoogleSubjectCourseService.GoogleSubjectCourseServiceClient _client;

    public GoogleSubjectCourseEnricher(GoogleSubjectCourseService.GoogleSubjectCourseServiceClient client)
    {
        _client = client;
    }

    public async Task EnrichAsync(
        IEnrichmentContext<string, SubjectCourseDtoBuilder, SubjectCourseDto> context,
        CancellationToken cancellationToken)
    {
        var request = new FindByIdsRequest { Ids = { context.Ids } };
        FindByIdsResponse response = await _client.FindByIdsAsync(request, cancellationToken: cancellationToken);

        foreach (GoogleSubjectCourse subjectCourse in response.SubjectCourses)
        {
            context[subjectCourse.Id].AddAssociation(subjectCourse.MapToAssociation());
        }
    }
}
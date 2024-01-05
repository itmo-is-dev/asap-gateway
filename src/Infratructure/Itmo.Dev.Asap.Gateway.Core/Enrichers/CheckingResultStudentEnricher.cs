using Itmo.Dev.Asap.Core.Models;
using Itmo.Dev.Asap.Core.Students;
using Itmo.Dev.Asap.Gateway.Application.Abstractions.Enrichment;
using Itmo.Dev.Asap.Gateway.Application.Abstractions.Enrichment.Builders;
using Itmo.Dev.Asap.Gateway.Application.Dto.Checking;

namespace Itmo.Dev.Asap.Gateway.Core.Enrichers;

public class CheckingResultStudentEnricher :
    IEntityEnricher<CheckingResultKey, CheckingResultBuilder, CheckingResultDto>
{
    private readonly StudentService.StudentServiceClient _client;

    public CheckingResultStudentEnricher(StudentService.StudentServiceClient client)
    {
        _client = client;
    }

    public async Task EnrichAsync(
        IEnrichmentContext<CheckingResultKey, CheckingResultBuilder, CheckingResultDto> context,
        CancellationToken cancellationToken)
    {
        IEnumerable<string> ids = ExtractStudentIds(context.Builders).Distinct().Select(x => x.ToString());

        var request = new QueryStudentRequest
        {
            Ids = { ids },
            PageSize = int.MaxValue,
        };

        QueryStudentResponse response = await _client
            .QueryAsync(request, cancellationToken: cancellationToken);

        var students = response.Students.ToDictionary(x => Guid.Parse(x.User.Id));

        foreach (CheckingResultBuilder builder in context.Builders)
        {
            builder.FirstSubmissionStudent = students.TryGetValue(builder.FirstSubmission.UserId, out Student? first)
                ? Map(first)
                : null;

            builder.SecondSubmissionStudent = students.TryGetValue(builder.SecondSubmission.UserId, out Student? second)
                ? Map(second)
                : null;
        }
    }

    private static IEnumerable<Guid> ExtractStudentIds(IEnumerable<CheckingResultBuilder> builders)
    {
        foreach (CheckingResultBuilder builder in builders)
        {
            yield return builder.FirstSubmission.UserId;
            yield return builder.SecondSubmission.UserId;
        }
    }

    private static CheckingResultStudentInfo Map(Student student)
    {
        return new CheckingResultStudentInfo(
            student.User.FirstName,
            student.User.LastName,
            student.GroupName);
    }
}
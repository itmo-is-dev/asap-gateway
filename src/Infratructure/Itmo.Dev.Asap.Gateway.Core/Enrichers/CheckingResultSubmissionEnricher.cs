using Itmo.Dev.Asap.Core.Models;
using Itmo.Dev.Asap.Core.Submissions;
using Itmo.Dev.Asap.Gateway.Application.Abstractions.Enrichment;
using Itmo.Dev.Asap.Gateway.Application.Abstractions.Enrichment.Builders;
using Itmo.Dev.Asap.Gateway.Application.Dto.Checking;
using Itmo.Dev.Asap.Gateway.Core.Mapping;

namespace Itmo.Dev.Asap.Gateway.Core.Enrichers;

public class CheckingResultSubmissionEnricher :
    IEntityEnricher<CheckingResultKey, CheckingResultBuilder, CheckingResultDto>
{
    private readonly SubmissionService.SubmissionServiceClient _client;

    public CheckingResultSubmissionEnricher(SubmissionService.SubmissionServiceClient client)
    {
        _client = client;
    }

    public async Task EnrichAsync(
        IEnrichmentContext<CheckingResultKey, CheckingResultBuilder, CheckingResultDto> context,
        CancellationToken cancellationToken)
    {
        IEnumerable<string> submissionIds = ExtractSubmissionIds(context.Builders)
            .Distinct()
            .Select(x => x.ToString());

        var request = new QueryInfoRequest { SubmissionIds = { submissionIds } };
        QueryInfoResponse response = await _client.QueryInfoAsync(request, cancellationToken: cancellationToken);

        var submissions = response.Submissions.ToDictionary(x => Guid.Parse(x.SubmissionId));

        foreach (CheckingResultBuilder builder in context.Builders)
        {
            builder.FirstSubmissionState = submissions
                .TryGetValue(builder.FirstSubmission.SubmissionId, out SubmissionInfo? first)
                ? first.State.ToDto()
                : null;

            builder.SecondSubmissionState = submissions
                .TryGetValue(builder.SecondSubmission.SubmissionId, out SubmissionInfo? second)
                ? second.State.ToDto()
                : null;
        }
    }

    private IEnumerable<Guid> ExtractSubmissionIds(IEnumerable<CheckingResultBuilder> builders)
    {
        foreach (CheckingResultBuilder builder in builders)
        {
            yield return builder.FirstSubmission.SubmissionId;
            yield return builder.SecondSubmission.SubmissionId;
        }
    }
}
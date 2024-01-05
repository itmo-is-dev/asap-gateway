using Itmo.Dev.Asap.Core.Assignments;
using Itmo.Dev.Asap.Core.Models;
using Itmo.Dev.Asap.Gateway.Application.Abstractions.Enrichment;
using Itmo.Dev.Asap.Gateway.Application.Abstractions.Enrichment.Builders;
using Itmo.Dev.Asap.Gateway.Application.Dto.Checking;

namespace Itmo.Dev.Asap.Gateway.Core.Enrichers;

public class CheckingResultAssignmentEnricher :
    IEntityEnricher<CheckingResultKey, CheckingResultBuilder, CheckingResultDto>
{
    private readonly AssignmentsService.AssignmentsServiceClient _client;

    public CheckingResultAssignmentEnricher(AssignmentsService.AssignmentsServiceClient client)
    {
        _client = client;
    }

    public async Task EnrichAsync(
        IEnrichmentContext<CheckingResultKey, CheckingResultBuilder, CheckingResultDto> context,
        CancellationToken cancellationToken)
    {
        IEnumerable<string> ids = context.Builders
            .Select(x => x.AssignmentId)
            .Distinct()
            .Select(x => x.ToString());

        var request = new QueryRequest
        {
            Ids = { ids },
        };

        QueryResponse response = await _client
            .QueryAsync(request, cancellationToken: cancellationToken);

        var assignments = response.Assignments.ToDictionary(x => Guid.Parse(x.Id));

        foreach (CheckingResultBuilder builder in context.Builders)
        {
            builder.AssignmentName = assignments.TryGetValue(builder.AssignmentId, out Assignment? assignment)
                ? assignment.Title
                : null;
        }
    }
}
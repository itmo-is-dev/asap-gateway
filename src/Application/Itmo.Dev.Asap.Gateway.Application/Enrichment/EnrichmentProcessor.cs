using Itmo.Dev.Asap.Gateway.Application.Abstractions.Enrichment;
using Microsoft.Extensions.Logging;

namespace Itmo.Dev.Asap.Gateway.Application.Enrichment;

public class EnrichmentProcessor<TIdentifier, TBuilder, TEntity> : IEnrichmentProcessor<TIdentifier, TBuilder, TEntity>
    where TBuilder : IEntityBuilder<TIdentifier, TEntity>
    where TIdentifier : notnull
{
    private readonly IReadOnlyCollection<IEntityEnricher<TIdentifier, TBuilder, TEntity>> _enrichers;
    private readonly ILogger<EnrichmentProcessor<TIdentifier, TBuilder, TEntity>> _logger;

    public EnrichmentProcessor(
        IEnumerable<IEntityEnricher<TIdentifier, TBuilder, TEntity>> enrichers,
        ILogger<EnrichmentProcessor<TIdentifier, TBuilder, TEntity>> logger)
    {
        _logger = logger;
        _enrichers = enrichers.ToArray();
    }

    public async Task<IEnumerable<TEntity>> EnrichAsync(
        IEnumerable<TBuilder> builders,
        CancellationToken cancellationToken)
    {
        var context = new EnrichmentContext<TIdentifier, TBuilder, TEntity>(builders);

        foreach (IEntityEnricher<TIdentifier, TBuilder, TEntity> enricher in _enrichers)
        {
            try
            {
                await enricher.EnrichAsync(context, cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "Failed enrich entity");
            }
        }

        return context.Builders.Select(x => x.Build());
    }
}
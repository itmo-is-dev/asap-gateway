using Itmo.Dev.Asap.Gateway.Application.Abstractions.Enrichment;

namespace Itmo.Dev.Asap.Gateway.Application.Enrichment;

public class EnrichmentProcessor<TIdentifier, TBuilder, TEntity> : IEnrichmentProcessor<TIdentifier, TBuilder, TEntity>
    where TBuilder : IEntityBuilder<TIdentifier, TEntity>
    where TIdentifier : notnull
{
    private readonly IReadOnlyCollection<IEntityEnricher<TIdentifier, TBuilder, TEntity>> _enrichers;

    public EnrichmentProcessor(IEnumerable<IEntityEnricher<TIdentifier, TBuilder, TEntity>> enrichers)
    {
        _enrichers = enrichers.ToArray();
    }

    public async Task<IEnumerable<TEntity>> EnrichAsync(
        IEnumerable<TBuilder> builders,
        CancellationToken cancellationToken)
    {
        var context = new EnrichmentContext<TIdentifier, TBuilder, TEntity>(builders);

        foreach (IEntityEnricher<TIdentifier, TBuilder, TEntity> enricher in _enrichers)
        {
            await enricher.EnrichAsync(context, cancellationToken);
        }

        return context.Builders.Select(x => x.Build());
    }
}
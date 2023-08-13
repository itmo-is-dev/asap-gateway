namespace Itmo.Dev.Asap.Gateway.Application.Abstractions.Enrichment;

public interface IEntityEnricher<TIdentifier, in TBuilder, in TEntity>
    where TBuilder : IEntityBuilder<TIdentifier, TEntity>
    where TIdentifier : notnull
{
    Task EnrichAsync(IEnrichmentContext<TIdentifier, TBuilder, TEntity> context, CancellationToken cancellationToken);
}
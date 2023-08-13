namespace Itmo.Dev.Asap.Gateway.Application.Abstractions.Enrichment;

public interface IEnrichmentProcessor<TIdentifier, in TBuilder, TEntity>
    where TBuilder : IEntityBuilder<TIdentifier, TEntity>
    where TIdentifier : notnull
{
    Task<IEnumerable<TEntity>> EnrichAsync(IEnumerable<TBuilder> builders, CancellationToken cancellationToken);
}
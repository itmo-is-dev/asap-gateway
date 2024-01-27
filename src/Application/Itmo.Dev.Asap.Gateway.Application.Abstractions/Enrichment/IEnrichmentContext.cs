namespace Itmo.Dev.Asap.Gateway.Application.Abstractions.Enrichment;

public interface IEnrichmentContext<TIdentifier, out TBuilder, out TEntity>
    where TBuilder : IEntityBuilder<TIdentifier, TEntity>
    where TIdentifier : notnull
{
    IEnumerable<TIdentifier> Ids { get; }

    IEnumerable<TBuilder> Builders { get; }

    TBuilder this[TIdentifier identifier] { get; }
}
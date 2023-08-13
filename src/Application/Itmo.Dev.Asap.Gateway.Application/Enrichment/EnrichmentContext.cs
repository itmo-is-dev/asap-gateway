using Itmo.Dev.Asap.Gateway.Application.Abstractions.Enrichment;

namespace Itmo.Dev.Asap.Gateway.Application.Enrichment;

public class EnrichmentContext<TIdentifier, TBuilder, TEntity> : IEnrichmentContext<TIdentifier, TBuilder, TEntity>
    where TBuilder : IEntityBuilder<TIdentifier, TEntity>
    where TIdentifier : notnull
{
    private readonly IReadOnlyDictionary<TIdentifier, TBuilder> _builders;

    public EnrichmentContext(IEnumerable<TBuilder> builders)
    {
        _builders = builders.ToDictionary(x => x.Id, x => x);

        Ids = _builders.Keys.ToArray();
        Builders = _builders.Values;
    }

    public IEnumerable<TIdentifier> Ids { get; }

    public IEnumerable<TBuilder> Builders { get; }

    public TBuilder this[TIdentifier identifier] => _builders[identifier];
}
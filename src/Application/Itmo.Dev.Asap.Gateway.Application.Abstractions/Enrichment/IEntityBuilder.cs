namespace Itmo.Dev.Asap.Gateway.Application.Abstractions.Enrichment;

public interface IEntityBuilder<out TIdentifier, out TEntity> where TIdentifier : notnull
{
    TIdentifier Id { get; }

    TEntity Build();
}
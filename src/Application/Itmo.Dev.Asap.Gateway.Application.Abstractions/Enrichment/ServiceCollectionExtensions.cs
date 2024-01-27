using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Itmo.Dev.Asap.Gateway.Application.Abstractions.Enrichment;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection TryAddEnricher<TEnricher, TKey, TBuilder, TEntity>(
        this IServiceCollection collection)
        where TEnricher : class, IEntityEnricher<TKey, TBuilder, TEntity>
        where TKey : notnull
        where TBuilder : IEntityBuilder<TKey, TEntity>
    {
        collection.TryAddEnumerable(
            ServiceDescriptor.Scoped<IEntityEnricher<TKey, TBuilder, TEntity>, TEnricher>());

        return collection;
    }
}
using Itmo.Dev.Asap.Gateway.Application.Abstractions.Enrichment;
using Itmo.Dev.Asap.Gateway.Application.Enrichment;
using Microsoft.Extensions.DependencyInjection;

namespace Itmo.Dev.Asap.Gateway.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection collection)
    {
        collection.AddScoped(typeof(IEnrichmentProcessor<,,>), typeof(EnrichmentProcessor<,,>));
        return collection;
    }
}
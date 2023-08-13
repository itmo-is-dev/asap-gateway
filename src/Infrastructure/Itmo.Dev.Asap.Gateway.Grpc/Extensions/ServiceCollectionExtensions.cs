using Itmo.Dev.Asap.Gateway.Grpc.Interceptors;
using Itmo.Dev.Asap.Gateway.Grpc.Tools;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Itmo.Dev.Asap.Gateway.Grpc.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGrpcInfrastructure(
        this IServiceCollection collection,
        IConfiguration configuration)
    {
        IConfigurationSection clientSection = configuration.GetSection("Infrastructure:Grpc:Clients");

        foreach (IConfigurationSection childSection in clientSection.GetChildren())
        {
            collection.Configure<GrpcClientOptions>(childSection.Key, childSection);
        }

        collection.AddScoped<AuthenticationInterceptor>();

        return collection;
    }
}
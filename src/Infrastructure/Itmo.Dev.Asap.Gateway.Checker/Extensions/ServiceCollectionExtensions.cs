using Grpc.Net.ClientFactory;
using Itmo.Dev.Asap.Checker;
using Itmo.Dev.Asap.Gateway.Grpc.Interceptors;
using Itmo.Dev.Asap.Gateway.Grpc.Tools;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Itmo.Dev.Asap.Gateway.Checker.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCheckerGrpcClients(this IServiceCollection collection)
    {
        AddClient<CheckingService.CheckingServiceClient>();

        return collection;

        void AddClient<TClient>() where TClient : class
        {
            collection
                .AddGrpcClient<TClient>((sp, o) =>
                {
                    IOptionsMonitor<GrpcClientOptions> monitor = sp
                        .GetRequiredService<IOptionsMonitor<GrpcClientOptions>>();

                    GrpcClientOptions options = monitor.Get("asap-checker");

                    o.Address = options.Uri;
                })
                .AddInterceptor<AuthenticationInterceptor>(InterceptorScope.Client);
        }
    }
}
using Grpc.Net.ClientFactory;
using Itmo.Dev.Asap.Auth;
using Itmo.Dev.Asap.Gateway.Application.Abstractions.Services;
using Itmo.Dev.Asap.Gateway.Auth.Services;
using Itmo.Dev.Asap.Gateway.Grpc.Interceptors;
using Itmo.Dev.Asap.Gateway.Grpc.Tools;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Itmo.Dev.Asap.Gateway.Auth.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAuthGrpcClients(this IServiceCollection collection)
    {
        collection.AddScoped<IAuthService, AuthService>();

        collection
            .AddGrpcClient<IdentityService.IdentityServiceClient>((sp, o) =>
            {
                IOptionsMonitor<GrpcClientOptions> monitor = sp
                    .GetRequiredService<IOptionsMonitor<GrpcClientOptions>>();

                GrpcClientOptions options = monitor.Get("asap-auth");

                o.Address = options.Uri;
            })
            .AddInterceptor<AuthenticationInterceptor>(InterceptorScope.Client);

        return collection;
    }
}
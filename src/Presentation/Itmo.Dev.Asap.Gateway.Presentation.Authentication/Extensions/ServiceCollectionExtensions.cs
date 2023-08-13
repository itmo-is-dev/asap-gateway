using Itmo.Dev.Asap.Gateway.Presentation.Authentication.Providers;

namespace Itmo.Dev.Asap.Gateway.Presentation.Authentication.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGatewayAuthentication(this IServiceCollection collection)
    {
        collection
            .AddAuthentication(options => options.DefaultScheme = "Bearer")
            .AddScheme<AuthAuthenticationOptions, AuthAuthenticationHandler>("Bearer", _ => { });

        collection.AddScoped<TokenProvider>();
        collection.AddScoped<ITokenProvider>(p => p.GetRequiredService<TokenProvider>());

        return collection;
    }
}
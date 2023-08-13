using Itmo.Dev.Asap.Gateway.Presentation.Authorization.Models;
using Itmo.Dev.Asap.Gateway.Presentation.Authorization.Services;
using Itmo.Dev.Asap.Gateway.Presentation.Authorization.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Itmo.Dev.Asap.Gateway.Presentation.Authorization;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGatewayAuthorization(this IServiceCollection collection)
    {
        collection.AddOptions<AuthorizationConfiguration>().BindConfiguration(Constants.SectionKey);

        collection.AddSingleton<IAuthorizationPolicyProvider, AuthorizationFeaturePolicyProvider>();
        collection.AddSingleton<IAuthorizationHandler, FeatureAuthorizationHandler>();
        collection.AddSingleton<IFeatureService, FeatureService>();

        return collection;
    }
}
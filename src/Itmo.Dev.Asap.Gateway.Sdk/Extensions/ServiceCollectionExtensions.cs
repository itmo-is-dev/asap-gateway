using Itmo.Dev.Asap.Gateway.Sdk.Authentication;
using Itmo.Dev.Asap.Gateway.Sdk.Clients;
using Itmo.Dev.Asap.Gateway.Sdk.Tools;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Refit;

namespace Itmo.Dev.Asap.Gateway.Sdk.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAsapSdk(this IServiceCollection collection)
    {
        collection.AddOptions<GatewayOptions>().BindConfiguration("Gateway");

        collection.AddTransient<AuthenticationHandler>();

        AddClient<IAssignmentClient>();
        AddClient<IGithubManagementClient>();
        AddClient<IIdentityClient>();
        AddClient<IStudentClient>();
        AddClient<IStudentGroupClient>();
        AddClient<ISubjectClient>();
        AddClient<ISubjectCourseClient>();
        AddClient<ISubjectCourseGroupClient>();
        AddClient<IUserClient>();

        return collection;

        void AddClient<TClient>() where TClient : class
        {
            collection
                .AddRefitClient<TClient>()
                .ConfigureHttpClient((sp, client) =>
                {
                    IOptions<GatewayOptions> options = sp.GetRequiredService<IOptions<GatewayOptions>>();
                    client.BaseAddress = options.Value.Uri;
                })
                .AddHttpMessageHandler<AuthenticationHandler>();
        }
    }
}
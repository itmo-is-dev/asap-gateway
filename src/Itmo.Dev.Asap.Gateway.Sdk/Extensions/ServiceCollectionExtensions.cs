using FluentSerialization;
using FluentSerialization.Extensions.NewtonsoftJson;
using Itmo.Dev.Asap.Gateway.Application.Dto.Tools;
using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Tools;
using Itmo.Dev.Asap.Gateway.Sdk.Authentication;
using Itmo.Dev.Asap.Gateway.Sdk.Clients;
using Itmo.Dev.Asap.Gateway.Sdk.Clients.Implementation;
using Itmo.Dev.Asap.Gateway.Sdk.Tools;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Refit;

namespace Itmo.Dev.Asap.Gateway.Sdk.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAsapSdk(this IServiceCollection collection)
    {
        collection.AddOptions<GatewayOptions>().BindConfiguration("Gateway");

        collection.AddTransient<AuthenticationHandler>();

        AddClient<IAssignmentClient>();
        AddClient<ICheckingClient>();
        AddClient<IGithubManagementClient>();
        AddClient<IGithubSearchClient>();
        AddClient<IIdentityClient>();
        AddClient<IStudentClient>();
        AddClient<IStudentGroupClient>();
        AddClient<ISubjectClient>();
        AddClient<ISubjectCourseClient>();
        AddClient<ISubjectCourseGroupClient>();
        AddClient<ISubmissionsClient>();
        AddClient<IUserClient>();

        collection.AddSingleton<IQueueClient, QueueClient>();

        return collection;

        void AddClient<TClient>() where TClient : class
        {
            JsonSerializerSettings serializerSettings = ConfigurationBuilder
                .Build(new PresentationSerializationConfiguration(), new DtoSerializationConfiguration())
                .AsNewtonsoftSerializationSettings();

            collection
                .AddRefitClient<TClient>(new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer(serializerSettings),
                })
                .ConfigureHttpClient((sp, client) =>
                {
                    IOptions<GatewayOptions> options = sp.GetRequiredService<IOptions<GatewayOptions>>();
                    client.BaseAddress = options.Value.Uri;
                })
                .AddHttpMessageHandler<AuthenticationHandler>();
        }
    }
}
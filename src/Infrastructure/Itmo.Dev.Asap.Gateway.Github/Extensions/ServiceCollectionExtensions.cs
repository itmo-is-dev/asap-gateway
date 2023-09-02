using Grpc.Net.ClientFactory;
using Itmo.Dev.Asap.Gateway.Application.Abstractions.Enrichment;
using Itmo.Dev.Asap.Gateway.Application.Abstractions.Enrichment.Builders;
using Itmo.Dev.Asap.Gateway.Application.Dto.SubjectCourses;
using Itmo.Dev.Asap.Gateway.Application.Dto.Users;
using Itmo.Dev.Asap.Gateway.Github.Enrichers;
using Itmo.Dev.Asap.Gateway.Grpc.Interceptors;
using Itmo.Dev.Asap.Gateway.Grpc.Tools;
using Itmo.Dev.Asap.Github;
using Itmo.Dev.Asap.Github.Search;
using Itmo.Dev.Asap.Github.SubjectCourses;
using Itmo.Dev.Asap.Github.Users;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Itmo.Dev.Asap.Gateway.Github.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGithubGrpcClients(this IServiceCollection collection)
    {
        collection.TryAddEnumerable(
            ServiceDescriptor.Scoped<IEntityEnricher<string, UserDtoBuilder, UserDto>, GithubUserEnricher>());

        collection.TryAddEnumerable(
            ServiceDescriptor.Scoped<IEntityEnricher<string, StudentDtoBuilder, StudentDto>, GithubStudentEnricher>());

        collection.TryAddEnumerable(ServiceDescriptor
            .Scoped<IEntityEnricher<string, SubjectCourseDtoBuilder, SubjectCourseDto>, GithubSubjectCourseEnricher>());

        AddClient<GithubManagementService.GithubManagementServiceClient>();
        AddClient<GithubSubjectCourseService.GithubSubjectCourseServiceClient>();
        AddClient<GithubUserService.GithubUserServiceClient>();
        AddClient<GithubSubjectCourseService.GithubSubjectCourseServiceClient>();
        AddClient<GithubSearchService.GithubSearchServiceClient>();

        return collection;

        void AddClient<TClient>() where TClient : class
        {
            collection
                .AddGrpcClient<TClient>((sp, o) =>
                {
                    IOptionsMonitor<GrpcClientOptions> monitor = sp
                        .GetRequiredService<IOptionsMonitor<GrpcClientOptions>>();

                    GrpcClientOptions options = monitor.Get("asap-github");

                    o.Address = options.Uri;
                })
                .AddInterceptor<AuthenticationInterceptor>(InterceptorScope.Client);
        }
    }
}
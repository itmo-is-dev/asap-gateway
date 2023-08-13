using Grpc.Net.ClientFactory;
using Itmo.Dev.Asap.Gateway.Application.Abstractions.Enrichment;
using Itmo.Dev.Asap.Gateway.Application.Abstractions.Enrichment.Builders;
using Itmo.Dev.Asap.Gateway.Application.Dto.SubjectCourses;
using Itmo.Dev.Asap.Gateway.Application.Dto.Users;
using Itmo.Dev.Asap.Gateway.Github.Enrichers;
using Itmo.Dev.Asap.Gateway.Grpc.Interceptors;
using Itmo.Dev.Asap.Gateway.Grpc.Tools;
using Itmo.Dev.Asap.Github;
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
        static void ConfigureAddress(IServiceProvider sp, GrpcClientFactoryOptions o)
        {
            IOptionsMonitor<GrpcClientOptions> monitor = sp
                .GetRequiredService<IOptionsMonitor<GrpcClientOptions>>();

            GrpcClientOptions options = monitor.Get("asap-github");

            o.Address = options.Uri;
        }

        collection.TryAddEnumerable(
            ServiceDescriptor.Scoped<IEntityEnricher<string, UserDtoBuilder, UserDto>, GithubUserEnricher>());

        collection.TryAddEnumerable(
            ServiceDescriptor.Scoped<IEntityEnricher<string, StudentDtoBuilder, StudentDto>, GithubStudentEnricher>());

        collection.TryAddEnumerable(ServiceDescriptor
            .Scoped<IEntityEnricher<string, SubjectCourseDtoBuilder, SubjectCourseDto>, GithubSubjectCourseEnricher>());

        collection
            .AddGrpcClient<GithubManagementService.GithubManagementServiceClient>(ConfigureAddress)
            .AddInterceptor<AuthenticationInterceptor>(InterceptorScope.Client);

        collection
            .AddGrpcClient<GithubSubjectCourseService.GithubSubjectCourseServiceClient>(ConfigureAddress)
            .AddInterceptor<AuthenticationInterceptor>(InterceptorScope.Client);

        collection
            .AddGrpcClient<GithubUserService.GithubUserServiceClient>(ConfigureAddress)
            .AddInterceptor<AuthenticationInterceptor>(InterceptorScope.Client);

        return collection;
    }
}
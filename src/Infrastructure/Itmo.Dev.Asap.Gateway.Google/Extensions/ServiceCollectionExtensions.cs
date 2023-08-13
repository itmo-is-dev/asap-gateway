using Grpc.Net.ClientFactory;
using Itmo.Dev.Asap.Gateway.Application.Abstractions.Enrichment;
using Itmo.Dev.Asap.Gateway.Application.Abstractions.Enrichment.Builders;
using Itmo.Dev.Asap.Gateway.Application.Dto.SubjectCourses;
using Itmo.Dev.Asap.Gateway.Google.Enrichers;
using Itmo.Dev.Asap.Gateway.Grpc.Interceptors;
using Itmo.Dev.Asap.Gateway.Grpc.Tools;
using Itmo.Dev.Asap.Google;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Itmo.Dev.Asap.Gateway.Google.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGoogleGrpcClients(this IServiceCollection collection)
    {
        collection.TryAddEnumerable(ServiceDescriptor
            .Scoped<IEntityEnricher<string, SubjectCourseDtoBuilder, SubjectCourseDto>, GoogleSubjectCourseEnricher>());

        collection
            .AddGrpcClient<GoogleSubjectCourseService.GoogleSubjectCourseServiceClient>((sp, o) =>
            {
                IOptionsMonitor<GrpcClientOptions> monitor = sp
                    .GetRequiredService<IOptionsMonitor<GrpcClientOptions>>();

                GrpcClientOptions options = monitor.Get("asap-google");

                o.Address = options.Uri;
            })
            .AddInterceptor<AuthenticationInterceptor>(InterceptorScope.Client);

        return collection;
    }
}
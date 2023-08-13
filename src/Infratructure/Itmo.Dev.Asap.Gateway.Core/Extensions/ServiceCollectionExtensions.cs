using Grpc.Net.ClientFactory;
using Itmo.Dev.Asap.Core.Assignments;
using Itmo.Dev.Asap.Core.Permissions;
using Itmo.Dev.Asap.Core.StudentGroups;
using Itmo.Dev.Asap.Core.Students;
using Itmo.Dev.Asap.Core.SubjectCourseGroups;
using Itmo.Dev.Asap.Core.SubjectCourses;
using Itmo.Dev.Asap.Core.Subjects;
using Itmo.Dev.Asap.Core.Users;
using Itmo.Dev.Asap.Gateway.Grpc.Interceptors;
using Itmo.Dev.Asap.Gateway.Grpc.Tools;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Itmo.Dev.Asap.Gateway.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCoreGrpcClients(this IServiceCollection collection)
    {
        static void ConfigureAddress(IServiceProvider sp, GrpcClientFactoryOptions o)
        {
            IOptionsMonitor<GrpcClientOptions> monitor = sp
                .GetRequiredService<IOptionsMonitor<GrpcClientOptions>>();

            GrpcClientOptions options = monitor.Get("asap-core");

            o.Address = options.Uri;
        }

        collection
            .AddGrpcClient<AssignmentsService.AssignmentsServiceClient>(ConfigureAddress)
            .AddInterceptor<AuthenticationInterceptor>(InterceptorScope.Client);

        collection
            .AddGrpcClient<PermissionService.PermissionServiceClient>(ConfigureAddress)
            .AddInterceptor<AuthenticationInterceptor>(InterceptorScope.Client);

        collection
            .AddGrpcClient<StudentGroupService.StudentGroupServiceClient>(ConfigureAddress)
            .AddInterceptor<AuthenticationInterceptor>(InterceptorScope.Client);

        collection
            .AddGrpcClient<StudentService.StudentServiceClient>(ConfigureAddress)
            .AddInterceptor<AuthenticationInterceptor>(InterceptorScope.Client);

        collection
            .AddGrpcClient<SubjectCourseGroupService.SubjectCourseGroupServiceClient>(ConfigureAddress)
            .AddInterceptor<AuthenticationInterceptor>(InterceptorScope.Client);

        collection
            .AddGrpcClient<SubjectCourseService.SubjectCourseServiceClient>(ConfigureAddress)
            .AddInterceptor<AuthenticationInterceptor>(InterceptorScope.Client);

        collection
            .AddGrpcClient<SubjectService.SubjectServiceClient>(ConfigureAddress)
            .AddInterceptor<AuthenticationInterceptor>(InterceptorScope.Client);

        collection
            .AddGrpcClient<UserService.UserServiceClient>(ConfigureAddress)
            .AddInterceptor<AuthenticationInterceptor>(InterceptorScope.Client);

        return collection;
    }
}
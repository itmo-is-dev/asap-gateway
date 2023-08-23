using Grpc.Net.ClientFactory;
using Itmo.Dev.Asap.Core.Assignments;
using Itmo.Dev.Asap.Core.Permissions;
using Itmo.Dev.Asap.Core.Queue;
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
        AddClient<AssignmentsService.AssignmentsServiceClient>();
        AddClient<PermissionService.PermissionServiceClient>();
        AddClient<StudentGroupService.StudentGroupServiceClient>();
        AddClient<StudentService.StudentServiceClient>();
        AddClient<SubjectCourseGroupService.SubjectCourseGroupServiceClient>();
        AddClient<SubjectCourseService.SubjectCourseServiceClient>();
        AddClient<SubjectService.SubjectServiceClient>();
        AddClient<UserService.UserServiceClient>();
        AddClient<QueueService.QueueServiceClient>();

        return collection;

        void AddClient<TClient>() where TClient : class
        {
            collection
                .AddGrpcClient<TClient>((sp, o) =>
                {
                    IOptionsMonitor<GrpcClientOptions> monitor = sp
                        .GetRequiredService<IOptionsMonitor<GrpcClientOptions>>();

                    GrpcClientOptions options = monitor.Get("asap-core");

                    o.Address = options.Uri;
                })
                .AddInterceptor<AuthenticationInterceptor>(InterceptorScope.Client);
        }
    }
}
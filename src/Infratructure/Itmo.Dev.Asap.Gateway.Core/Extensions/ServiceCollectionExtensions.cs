using Grpc.Net.ClientFactory;
using Itmo.Dev.Asap.Core.Assignments;
using Itmo.Dev.Asap.Core.Permissions;
using Itmo.Dev.Asap.Core.Queue;
using Itmo.Dev.Asap.Core.StudentGroups;
using Itmo.Dev.Asap.Core.Students;
using Itmo.Dev.Asap.Core.SubjectCourseGroups;
using Itmo.Dev.Asap.Core.SubjectCourses;
using Itmo.Dev.Asap.Core.Subjects;
using Itmo.Dev.Asap.Core.Submissions;
using Itmo.Dev.Asap.Core.Users;
using Itmo.Dev.Asap.Gateway.Application.Abstractions.Enrichment;
using Itmo.Dev.Asap.Gateway.Application.Abstractions.Enrichment.Builders;
using Itmo.Dev.Asap.Gateway.Application.Dto.Checking;
using Itmo.Dev.Asap.Gateway.Core.Enrichers;
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
        AddClient<SubmissionService.SubmissionServiceClient>();
        AddClient<UserService.UserServiceClient>();
        AddClient<QueueService.QueueServiceClient>();

        collection.TryAddEnricher<
            CheckingResultStudentEnricher,
            CheckingResultKey,
            CheckingResultBuilder,
            CheckingResultDto>();

        collection.TryAddEnricher<
            CheckingResultAssignmentEnricher,
            CheckingResultKey,
            CheckingResultBuilder,
            CheckingResultDto>();

        collection.TryAddEnricher<
            CheckingResultSubmissionEnricher,
            CheckingResultKey,
            CheckingResultBuilder,
            CheckingResultDto>();

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
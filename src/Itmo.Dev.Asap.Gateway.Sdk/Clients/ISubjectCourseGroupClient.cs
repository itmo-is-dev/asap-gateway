using Itmo.Dev.Asap.Gateway.Application.Dto.SubjectCourses;
using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.SubjectCourseGroups;
using Refit;

namespace Itmo.Dev.Asap.Gateway.Sdk.Clients;

public interface ISubjectCourseGroupClient
{
    [Post("/api/subjectCourseGroup")]
    Task<IApiResponse<SubjectCourseGroupDto>> CreateAsync(
        [Body] CreateSubjectCourseGroupRequest request,
        CancellationToken cancellationToken);

    [Post("/api/subjectCourseGroup/bulk")]
    Task<IApiResponse<IReadOnlyCollection<SubjectCourseGroupDto>>> CreateAsync(
        [Body] BulkCreateSubjectCourseGroupsRequest request,
        CancellationToken cancellationToken);

    [Delete("/api/subjectCourseGroup")]
    Task<IApiResponse> DeleteAsync([Body] DeleteSubjectCourseGroupRequest request, CancellationToken cancellationToken);

    [Post("/api/subjectCourseGroup/query")]
    Task<IApiResponse<IEnumerable<SubjectCourseGroupDto>>> QueryAsync(
        [Body] QuerySubjectCourseGroupsRequest request,
        CancellationToken cancellationToken);
}
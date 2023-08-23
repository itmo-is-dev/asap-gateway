using Itmo.Dev.Asap.Gateway.Application.Dto.Study;
using Itmo.Dev.Asap.Gateway.Application.Dto.SubjectCourses;
using Itmo.Dev.Asap.Gateway.Application.Dto.Tables;
using Itmo.Dev.Asap.Gateway.Application.Dto.Users;
using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.SubjectCourses;
using Refit;

namespace Itmo.Dev.Asap.Gateway.Sdk.Clients;

public interface ISubjectCourseClient
{
    [Post("/api/subjectCourse")]
    Task<IApiResponse<SubjectCourseDto>> CreateAsync(
        [Body] CreateSubjectCourseRequest request,
        CancellationToken cancellationToken);

    [Get("/api/subjectCourse/{subjectCourseId}")]
    Task<IApiResponse<SubjectCourseDto>> GetByIdAsync(Guid subjectCourseId, CancellationToken cancellationToken);

    [Put("/api/subjectCourse/{subjectCourseId}")]
    Task<IApiResponse<SubjectCourseDto>> UpdateAsync(
        Guid subjectCourseId,
        [Body] UpdateSubjectCourseRequest request,
        CancellationToken cancellationToken);

    [Get("/api/subjectCourse/{subjectCourseId}/students")]
    Task<IApiResponse<IReadOnlyCollection<StudentDto>>> GetStudentsAsync(
        Guid subjectCourseId,
        CancellationToken cancellationToken);

    [Get("/api/subjectCourse/{subjectCourseId}/assignments")]
    Task<IApiResponse<IReadOnlyCollection<AssignmentDto>>> GetAssignmentsAsync(
        Guid subjectCourseId,
        CancellationToken cancellationToken);

    [Get("/api/subjectCourse/{subjectCourseId}/groups")]
    Task<IApiResponse<IReadOnlyCollection<SubjectCourseGroupDto>>> GetGroupsAsync(
        Guid subjectCourseId,
        CancellationToken cancellationToken);

    [Get("/api/subjectCourse/{subjectCourseId}/groups/{studentGroupId}/queue")]
    Task<IApiResponse<SubmissionsQueueDto>> GetStudentGroupQueueAsync(
        Guid subjectCourseId,
        Guid studentGroupId,
        CancellationToken cancellationToken);

    [Post("/api/subjectCourse/{subjectCourseId}/deadline/fraction")]
    Task<IApiResponse> AddDeadlineAsync(
        Guid subjectCourseId,
        [Body] AddFractionPolicyRequest request,
        CancellationToken cancellationToken);

    [Post("/api/subjectCourse/{subjectCourseId}/points/forceSync")]
    Task<IApiResponse> ForceSyncPointsAsync(Guid subjectCourseId, CancellationToken cancellationToken);

    [Put("/api/subjectCourse/{subjectCourseId}/github/mentorTeam")]
    Task<IApiResponse> UpdateGithubMentorTeamAsync(
        Guid subjectCourseId,
        [Body] UpdateMentorsTeamNameRequest request,
        CancellationToken cancellationToken);
}
using Itmo.Dev.Asap.Gateway.Application.Dto.Study;
using Itmo.Dev.Asap.Gateway.Application.Dto.SubjectCourses;
using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Subjects;
using Refit;

namespace Itmo.Dev.Asap.Gateway.Sdk.Clients;

public interface ISubjectClient
{
    [Post("/api/subject")]
    Task<IApiResponse<SubjectDto>> CreateAsync(
        [Body] CreateSubjectRequest request,
        CancellationToken cancellationToken);

    [Get("/api/subject")]
    Task<IApiResponse<IReadOnlyCollection<SubjectDto>>> GetAllAsync(CancellationToken cancellationToken);

    [Get("/api/subject/{subjectId}")]
    Task<IApiResponse<SubjectDto>> GetByIdAsync(Guid subjectId, CancellationToken cancellationToken);

    [Put("/api/subject/{subjectId}")]
    Task<IApiResponse<SubjectDto>> UpdateAsync(
        Guid subjectId,
        [Body] UpdateSubjectRequest request,
        CancellationToken cancellationToken);

    [Get("/api/subject/{subjectId}/courses")]
    Task<IApiResponse<IReadOnlyCollection<SubjectCourseDto>>> GetCoursesAsync(
        Guid subjectId,
        CancellationToken cancellationToken);
}
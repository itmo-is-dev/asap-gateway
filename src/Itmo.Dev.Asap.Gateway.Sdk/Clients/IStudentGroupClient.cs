using Itmo.Dev.Asap.Gateway.Application.Dto.Study;
using Itmo.Dev.Asap.Gateway.Application.Dto.Users;
using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.StudentGroups;
using Refit;

namespace Itmo.Dev.Asap.Gateway.Sdk.Clients;

public interface IStudentGroupClient
{
    [Post("/api/studentGroup")]
    Task<IApiResponse<StudentGroupDto>> CreateAsync(
        [Body] CreateStudentGroupRequest request,
        CancellationToken cancellationToken);

    [Get("/api/studentGroup/{groupId}")]
    Task<IApiResponse<StudentGroupDto>> GetByIdAsync(Guid groupId, CancellationToken cancellationToken);

    [Get("/api/studentGroup/bulk")]
    Task<IApiResponse<IReadOnlyCollection<StudentGroupDto>>> GetByIdsAsync(
        [Query(CollectionFormat.Multi), AliasAs("ids")]
        IEnumerable<Guid> groupIds,
        CancellationToken cancellationToken);

    [Put("/api/studentGroup/{groupId}")]
    Task<IApiResponse<StudentGroupDto>> UpdateAsync(
        Guid groupId,
        [Body] UpdateStudentGroupRequest request,
        CancellationToken cancellationToken);

    [Get("/api/studentGroup/{groupId}/student")]
    Task<IApiResponse<IReadOnlyCollection<StudentDto>>> GetStudentsAsync(
        Guid groupId,
        CancellationToken cancellationToken);

    [Post("/api/studentGroup/query")]
    Task<IApiResponse<QueryStudentGroupResponse>> QueryAsync(
        [Body] QueryStudentGroupRequest request,
        CancellationToken cancellationToken);
}
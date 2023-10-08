using Itmo.Dev.Asap.Gateway.Application.Dto.Study;
using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Assignments;
using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.GroupAssignments;
using Refit;

namespace Itmo.Dev.Asap.Gateway.Sdk.Clients;

public interface IAssignmentClient
{
    [Post("/api/assignments")]
    Task<IApiResponse<AssignmentDto>> CreateAsync(
        [Body] CreateAssignmentRequest request,
        CancellationToken cancellationToken);

    [Get("/api/assignments/{assignmentId}")]
    Task<IApiResponse<AssignmentDto>> GetByIdAsync(Guid assignmentId, CancellationToken cancellationToken);

    [Patch("/api/assignments/{assignmentId}")]
    Task<IApiResponse<AssignmentDto>> UpdatePointsAsync(
        Guid assignmentId,
        [Query] double? minPoints,
        [Query] double? maxPoints,
        CancellationToken cancellationToken);

    [Get("/api/assignments/{assignmentId}/groups")]
    Task<IApiResponse<IReadOnlyCollection<GroupAssignmentDto>>> GetGroupAssignmentsAsync(
        Guid assignmentId,
        CancellationToken cancellationToken);

    [Put("/api/assignments/{assignmentId}/groups/{groupId}")]
    Task<IApiResponse<GroupAssignmentDto>> UpdateGroupAssignmentAsync(
        Guid assignmentId,
        Guid groupId,
        [Body] UpdateGroupAssignmentRequest request,
        CancellationToken cancellationToken);

    [Put("/api/assignments/{assignmentId}/groups/deadline")]
    Task<IApiResponse<IReadOnlyCollection<GroupAssignmentDto>>> UpdateGroupAssignmentDeadlinesAsync(
        Guid assignmentId,
        [Body] UpdateGroupAssignmentDeadlinesRequest request,
        CancellationToken cancellationToken);
}
using Google.Protobuf.WellKnownTypes;
using Itmo.Dev.Asap.Core.Assignments;
using Itmo.Dev.Asap.Gateway.Application.Dto.Study;
using Itmo.Dev.Asap.Gateway.Core.Mapping;
using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models;
using Itmo.Dev.Asap.Gateway.Presentation.Authorization;
using Itmo.Dev.Asap.Gateway.Presentation.Controllers.Mapping;
using Microsoft.AspNetCore.Mvc;
using UpdateGroupAssignmentRequest =
    Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.GroupAssignments.UpdateGroupAssignmentRequest;

namespace Itmo.Dev.Asap.Gateway.Presentation.Controllers;

[Route("/api/[controller]")]
[ApiController]
public class AssignmentsController : ControllerBase
{
    private const string Scope = "Assignments";

    private readonly AssignmentsService.AssignmentsServiceClient _assignmentsClient;

    public AssignmentsController(AssignmentsService.AssignmentsServiceClient assignmentsClient)
    {
        _assignmentsClient = assignmentsClient;
    }

    [HttpPost]
    [AuthorizeFeature(Scope, nameof(Create))]
    public async Task<ActionResult<AssignmentDto>> Create(
        [FromBody] CreateAssignmentRequest request,
        CancellationToken cancellationToken)
    {
        CreateRequest grpcRequest = request.ToProto();

        CreateResponse response = await _assignmentsClient
            .CreateAssignmentAsync(grpcRequest, cancellationToken: cancellationToken);

        AssignmentDto assignment = response.Assignment.ToDto();

        return Ok(assignment);
    }

    [HttpGet("{id:guid}")]
    [AuthorizeFeature(Scope, nameof(GetById))]
    public async Task<ActionResult<AssignmentDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var grpcRequest = new GetByIdRequest
        {
            Id = id.ToString(),
        };

        GetByIdResponse response = await _assignmentsClient
            .GetByIdAsync(grpcRequest, cancellationToken: cancellationToken);

        AssignmentDto assignment = response.Assignment.ToDto();

        return Ok(assignment);
    }

    [HttpPatch("{id:guid}")]
    [AuthorizeFeature(Scope, nameof(UpdatePoints))]
    public async Task<ActionResult<AssignmentDto>> UpdatePoints(
        Guid id,
        double minPoints,
        double maxPoints,
        CancellationToken cancellationToken)
    {
        var request = new UpdatePointsRequest
        {
            AssignmentId = id.ToString(),
            MinPoints = minPoints,
            MaxPoints = maxPoints,
        };

        UpdatePointsResponse response = await _assignmentsClient
            .UpdatePointsAsync(request, cancellationToken: cancellationToken);

        AssignmentDto assignment = response.Assignment.ToDto();

        return Ok(assignment);
    }

    [HttpGet("{assignmentId:guid}/groups")]
    [AuthorizeFeature(Scope, nameof(GetGroups))]
    public async Task<ActionResult<IEnumerable<GroupAssignmentDto>>> GetGroups(
        Guid assignmentId,
        CancellationToken cancellationToken)
    {
        var request = new GetGroupAssignmentsRequest
        {
            AssignmentId = assignmentId.ToString(),
        };

        GetGroupAssignmentsResponse response = await _assignmentsClient
            .GetGroupAssignmentsAsync(request, cancellationToken: cancellationToken);

        IEnumerable<GroupAssignmentDto> groupAssignments = response.GroupAssignments
            .Select(x => x.ToDto());

        return Ok(groupAssignments);
    }

    [HttpPut("{assignmentId:guid}/groups/{groupId:guid}")]
    [AuthorizeFeature(Scope, nameof(UpdateGroupAssignment))]
    public async Task<ActionResult<GroupAssignmentDto>> UpdateGroupAssignment(
        Guid assignmentId,
        Guid groupId,
        UpdateGroupAssignmentRequest request,
        CancellationToken cancellationToken)
    {
        var grpcRequest = new Asap.Core.Assignments.UpdateGroupAssignmentRequest
        {
            AssignmentId = assignmentId.ToString(),
            GroupId = groupId.ToString(),
            Deadline = Timestamp.FromDateTime(request.Deadline),
        };

        UpdateGroupAssignmentResponse response = await _assignmentsClient
            .UpdateGroupAssignmentAsync(grpcRequest, cancellationToken: cancellationToken);

        GroupAssignmentDto groupAssignment = response.GroupAssignment.ToDto();

        return Ok(groupAssignment);
    }
}
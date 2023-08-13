using Itmo.Dev.Asap.Core.SubjectCourseGroups;
using Itmo.Dev.Asap.Gateway.Application.Dto.SubjectCourses;
using Itmo.Dev.Asap.Gateway.Core.Mapping;
using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.SubjectCourseGroups;
using Itmo.Dev.Asap.Gateway.Presentation.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Itmo.Dev.Asap.Gateway.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SubjectCourseGroupController : ControllerBase
{
    private const string Scope = "SubjectCourseGroups";

    private readonly SubjectCourseGroupService.SubjectCourseGroupServiceClient _client;

    public SubjectCourseGroupController(SubjectCourseGroupService.SubjectCourseGroupServiceClient client)
    {
        _client = client;
    }

    [HttpPost]
    [AuthorizeFeature(Scope, nameof(Create))]
    public async Task<ActionResult<SubjectCourseGroupDto>> Create(
        [FromBody] CreateSubjectCourseGroupRequest request,
        CancellationToken cancellationToken)
    {
        var grpcRequest = new CreateRequest
        {
            SubjectCourseId = request.SubjectCourseId.ToString(),
            StudentGroupIds = { request.GroupId.ToString() },
        };

        CreateResponse response = await _client.CreateAsync(grpcRequest, cancellationToken: cancellationToken);

        SubjectCourseGroupDto group = response.SubjectCourseGroups.Single().ToDto();

        return Ok(group);
    }

    [HttpPost("bulk")]
    [AuthorizeFeature(Scope, nameof(BulkCreate))]
    public async Task<ActionResult<IEnumerable<SubjectCourseGroupDto>>> BulkCreate(
        [FromBody] BulkCreateSubjectCourseGroupsRequest request,
        CancellationToken cancellationToken)
    {
        var grpcRequest = new CreateRequest
        {
            SubjectCourseId = request.SubjectCourseId.ToString(),
            StudentGroupIds = { request.GroupIds.Select(x => x.ToString()) },
        };

        CreateResponse response = await _client.CreateAsync(grpcRequest, cancellationToken: cancellationToken);

        IEnumerable<SubjectCourseGroupDto> groups = response.SubjectCourseGroups.Select(x => x.ToDto());

        return Ok(groups);
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [AuthorizeFeature(Scope, nameof(Delete))]
    public async Task<IActionResult> Delete(
        [FromBody] DeleteSubjectCourseGroupRequest request,
        CancellationToken cancellationToken)
    {
        var grpcRequest = new DeleteRequest
        {
            StudentGroupId = request.GroupId.ToString(),
            SubjectCourseId = request.SubjectCourseId.ToString(),
        };

        await _client.DeleteAsync(grpcRequest, cancellationToken: cancellationToken);

        return NoContent();
    }
}
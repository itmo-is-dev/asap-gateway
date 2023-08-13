using Itmo.Dev.Asap.Core.StudentGroups;
using Itmo.Dev.Asap.Gateway.Application.Abstractions.Enrichment;
using Itmo.Dev.Asap.Gateway.Application.Abstractions.Enrichment.Builders;
using Itmo.Dev.Asap.Gateway.Application.Dto.Study;
using Itmo.Dev.Asap.Gateway.Application.Dto.Users;
using Itmo.Dev.Asap.Gateway.Core.Mapping;
using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.StudentGroups;
using Itmo.Dev.Asap.Gateway.Presentation.Authorization;
using Itmo.Dev.Asap.Gateway.Presentation.Controllers.Mapping;
using Microsoft.AspNetCore.Mvc;
using QueryStudentGroupRequest =
    Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.StudentGroups.QueryStudentGroupRequest;
using QueryStudentGroupResponse = Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.StudentGroups.QueryStudentGroupResponse;

namespace Itmo.Dev.Asap.Gateway.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentGroupController : ControllerBase
{
    private const string Scope = "StudentGroup";

    private readonly StudentGroupService.StudentGroupServiceClient _studentGroupClient;
    private readonly IEnrichmentProcessor<string, StudentDtoBuilder, StudentDto> _studentProcessor;

    public StudentGroupController(
        StudentGroupService.StudentGroupServiceClient studentGroupClient,
        IEnrichmentProcessor<string, StudentDtoBuilder, StudentDto> studentProcessor)
    {
        _studentGroupClient = studentGroupClient;
        _studentProcessor = studentProcessor;
    }

    [HttpPost]
    [AuthorizeFeature(Scope, nameof(Create))]
    public async Task<ActionResult<StudentGroupDto>> Create(
        CreateStudentGroupRequest request,
        CancellationToken cancellationToken)
    {
        var grpcRequest = new CreateRequest { Name = request.Name };

        CreateResponse response = await _studentGroupClient
            .CreateAsync(grpcRequest, cancellationToken: cancellationToken);

        StudentGroupDto group = response.StudentGroup.ToDto();

        return Ok(group);
    }

    [HttpGet("{id:guid}")]
    [AuthorizeFeature(Scope, nameof(GetById))]
    public async Task<ActionResult<StudentGroupDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var request = new FindByIdsRequest { Ids = { id.ToString() } };

        FindByIdsResponse response = await _studentGroupClient
            .FindByIdsAsync(request, cancellationToken: cancellationToken);

        StudentGroupDto? group = response.StudentGroups.SingleOrDefault()?.ToDto();

        return group is null ? NotFound() : Ok(group);
    }

    [HttpGet("bulk")]
    [AuthorizeFeature(Scope, nameof(BulkGetByIds))]
    public async Task<ActionResult<IEnumerable<StudentGroupDto>>> BulkGetByIds(
        [FromQuery] IReadOnlyCollection<Guid> ids,
        CancellationToken cancellationToken)
    {
        var request = new FindByIdsRequest { Ids = { ids.Select(x => x.ToString()) } };

        FindByIdsResponse response = await _studentGroupClient
            .FindByIdsAsync(request, cancellationToken: cancellationToken);

        IEnumerable<StudentGroupDto> groups = response.StudentGroups.Select(x => x.ToDto());

        return Ok(groups);
    }

    [HttpPut("{id:guid}")]
    [AuthorizeFeature(Scope, nameof(Update))]
    public async Task<ActionResult<StudentGroupDto>> Update(
        Guid id,
        [FromBody] UpdateStudentGroupRequest request,
        CancellationToken cancellationToken)
    {
        var grpcRequest = new UpdateRequest { Id = id.ToString(), Name = request.Name };

        UpdateResponse response = await _studentGroupClient
            .UpdateAsync(grpcRequest, cancellationToken: cancellationToken);

        StudentGroupDto group = response.StudentGroup.ToDto();

        return Ok(group);
    }

    [HttpGet("{id:guid}/students")]
    [AuthorizeFeature(Scope, nameof(GetStudents))]
    public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudents(
        Guid id,
        CancellationToken cancellationToken)
    {
        var request = new GetStudentsRequest { GroupId = id.ToString() };

        GetStudentsResponse response = await _studentGroupClient
            .GetStudentsAsync(request, cancellationToken: cancellationToken);

        IEnumerable<StudentDtoBuilder> builders = response.Students.Select(x => x.MapToBuilder());
        IEnumerable<StudentDto> students = await _studentProcessor.EnrichAsync(builders, cancellationToken);

        return Ok(students);
    }

    [HttpPost("query")]
    [AuthorizeFeature(Scope, nameof(Query))]
    public async Task<ActionResult<QueryStudentGroupResponse>> Query(
        [FromBody] QueryStudentGroupRequest request,
        CancellationToken cancellationToken)
    {
        Asap.Core.StudentGroups.QueryStudentGroupRequest grpcRequest = request.ToProto();

        Asap.Core.StudentGroups.QueryStudentGroupResponse grpcResponse = await _studentGroupClient
            .QueryAsync(grpcRequest, cancellationToken: cancellationToken);

        IEnumerable<StudentGroupDto> groups = grpcResponse.StudentGroups.Select(x => x.ToDto());

        var response = new QueryStudentGroupResponse(grpcResponse.PageToken, groups);

        return Ok(response);
    }
}
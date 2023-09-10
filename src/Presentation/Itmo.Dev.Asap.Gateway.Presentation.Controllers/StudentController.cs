using Itmo.Dev.Asap.Core.Students;
using Itmo.Dev.Asap.Gateway.Application.Abstractions.Enrichment;
using Itmo.Dev.Asap.Gateway.Application.Abstractions.Enrichment.Builders;
using Itmo.Dev.Asap.Gateway.Application.Dto.Users;
using Itmo.Dev.Asap.Gateway.Presentation.Authorization;
using Itmo.Dev.Asap.Gateway.Presentation.Controllers.Mapping;
using Microsoft.AspNetCore.Mvc;
using CreateStudentsRequest = Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Students.CreateStudentsRequest;
using QueryStudentRequest = Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Students.QueryStudentRequest;
using QueryStudentResponse = Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Students.QueryStudentResponse;
using TransferStudentRequest = Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Students.TransferStudentRequest;

namespace Itmo.Dev.Asap.Gateway.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentController : ControllerBase
{
    private const string Scope = "Student";

    private readonly StudentService.StudentServiceClient _studentClient;
    private readonly IEnrichmentProcessor<string, StudentDtoBuilder, StudentDto> _enrichmentProcessor;

    public StudentController(
        StudentService.StudentServiceClient studentClient,
        IEnrichmentProcessor<string, StudentDtoBuilder, StudentDto> enrichmentProcessor)
    {
        _studentClient = studentClient;
        _enrichmentProcessor = enrichmentProcessor;
    }

    [HttpPost]
    [AuthorizeFeature(Scope, nameof(Create))]
    public async Task<ActionResult<IEnumerable<StudentDto>>> Create(
        CreateStudentsRequest request,
        CancellationToken cancellationToken)
    {
        Asap.Core.Students.CreateStudentsRequest grpcRequest = request.ToProto();

        CreateStudentsResponse response = await _studentClient
            .CreateAsync(grpcRequest, cancellationToken: cancellationToken);

        StudentDtoBuilder[] builders = { response.Student.MapToBuilder() };
        IEnumerable<StudentDto> students = await _enrichmentProcessor.EnrichAsync(builders, cancellationToken);

        return Ok(students);
    }

    [HttpPut("{id:guid}/dismiss")]
    [AuthorizeFeature(Scope, nameof(DismissFromGroup))]
    public async Task<ActionResult<StudentDto>> DismissFromGroup(Guid id, CancellationToken cancellationToken)
    {
        var request = new DismissFromGroupRequest { StudentId = id.ToString() };

        DismissFromGroupResponse response = await _studentClient
            .DismissFromGroupAsync(request, cancellationToken: cancellationToken);

        StudentDtoBuilder[] builders = { response.Student.MapToBuilder() };
        IEnumerable<StudentDto> students = await _enrichmentProcessor.EnrichAsync(builders, cancellationToken);

        return Ok(students.Single());
    }

    [HttpPut("{id:guid}/group")]
    [AuthorizeFeature(Scope, nameof(Transfer))]
    public async Task<ActionResult<StudentDto>> Transfer(
        Guid id,
        [FromBody] TransferStudentRequest request,
        CancellationToken cancellationToken)
    {
        var grpcRequest = new Asap.Core.Students.TransferStudentRequest
        {
            StudentId = id.ToString(),
            GroupId = request.NewGroupId.ToString(),
        };

        TransferStudentResponse response = await _studentClient
            .TransferAsync(grpcRequest, cancellationToken: cancellationToken);

        StudentDtoBuilder[] builders = { response.Student.MapToBuilder() };
        IEnumerable<StudentDto> students = await _enrichmentProcessor.EnrichAsync(builders, cancellationToken);

        return Ok(students.Single());
    }

    [HttpPost("query")]
    [AuthorizeFeature(Scope, nameof(Query))]
    public async Task<ActionResult<QueryStudentResponse>> Query(
        [FromBody] QueryStudentRequest request,
        CancellationToken cancellationToken)
    {
        var result = new List<StudentDto>();

        Asap.Core.Students.QueryStudentRequest grpcRequest = request.ToProto();

        string[] githubPatterns = request.GithubUsernamePatterns.ToArray();

        while (result.Count < request.PageSize)
        {
            Asap.Core.Students.QueryStudentResponse grpcResponse = await _studentClient
                .QueryAsync(grpcRequest, cancellationToken: cancellationToken);

            grpcRequest.PageToken = grpcResponse.PageToken;

            IEnumerable<StudentDtoBuilder> builders = grpcResponse.Students.Select(x => x.MapToBuilder());
            IEnumerable<StudentDto> students = await _enrichmentProcessor.EnrichAsync(builders, cancellationToken);

            if (githubPatterns is not [])
            {
                students = students.Where(
                    s => githubPatterns.Any(
                        u => s.User.GithubUsername?.Contains(u, StringComparison.OrdinalIgnoreCase) is true));
            }

            result.AddRange(students);

            if (grpcResponse.PageToken is null)
                break;
        }

        var response = new QueryStudentResponse(grpcRequest.PageToken, result);

        return Ok(response);
    }
}
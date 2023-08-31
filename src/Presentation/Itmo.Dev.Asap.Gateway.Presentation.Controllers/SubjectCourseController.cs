using Google.Protobuf.WellKnownTypes;
using Itmo.Dev.Asap.Core.SubjectCourses;
using Itmo.Dev.Asap.Gateway.Application.Abstractions.Enrichment;
using Itmo.Dev.Asap.Gateway.Application.Abstractions.Enrichment.Builders;
using Itmo.Dev.Asap.Gateway.Application.Dto.Study;
using Itmo.Dev.Asap.Gateway.Application.Dto.SubjectCourses;
using Itmo.Dev.Asap.Gateway.Application.Dto.Tables;
using Itmo.Dev.Asap.Gateway.Application.Dto.Users;
using Itmo.Dev.Asap.Gateway.Core.Mapping;
using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.SubjectCourses;
using Itmo.Dev.Asap.Gateway.Presentation.Authorization;
using Itmo.Dev.Asap.Gateway.Presentation.Controllers.Mapping;
using Itmo.Dev.Asap.Github.SubjectCourses;
using Microsoft.AspNetCore.Mvc;

namespace Itmo.Dev.Asap.Gateway.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SubjectCourseController : ControllerBase
{
    private const string Scope = "SubjectCourse";

    private readonly SubjectCourseService.SubjectCourseServiceClient _subjectCourseClient;
    private readonly IEnrichmentProcessor<string, SubjectCourseDtoBuilder, SubjectCourseDto> _enrichmentProcessor;
    private readonly IEnrichmentProcessor<string, StudentDtoBuilder, StudentDto> _studentProcessor;
    private readonly GithubSubjectCourseService.GithubSubjectCourseServiceClient _githubSubjectCourseClient;

    public SubjectCourseController(
        SubjectCourseService.SubjectCourseServiceClient subjectCourseClient,
        IEnrichmentProcessor<string, SubjectCourseDtoBuilder, SubjectCourseDto> enrichmentProcessor,
        IEnrichmentProcessor<string, StudentDtoBuilder, StudentDto> studentProcessor,
        GithubSubjectCourseService.GithubSubjectCourseServiceClient githubSubjectCourseClient)
    {
        _subjectCourseClient = subjectCourseClient;
        _enrichmentProcessor = enrichmentProcessor;
        _studentProcessor = studentProcessor;
        _githubSubjectCourseClient = githubSubjectCourseClient;
    }

    [HttpPost]
    public async Task<ActionResult<SubjectCourseDto>> Create(
        [FromBody] CreateSubjectCourseRequest request,
        CancellationToken cancellationToken)
    {
        string correlationId = Guid.NewGuid().ToString();

        if (request.Args is CreateSubjectCourseGithubArgs githubArgs)
        {
            var githubRequest = new ProvisionSubjectCourseRequest
            {
                CorrelationId = correlationId,
                MentorTeamId = githubArgs.MentorTeamId,
                OrganizationId = githubArgs.OrganizationId,
                TemplateRepositoryId = githubArgs.TemplateRepositoryId,
            };

            await _githubSubjectCourseClient
                .ProvisionSubjectCourseAsync(githubRequest, cancellationToken: cancellationToken);
        }

        var grpcRequest = new CreateRequest
        {
            CorrelationId = correlationId,
            SubjectId = request.SubjectId.ToString(),
            Title = request.Name,
            WorkflowType = request.WorkflowType.ToProto(),
        };

        CreateResponse response = await _subjectCourseClient
            .CreateAsync(grpcRequest, cancellationToken: cancellationToken);

        SubjectCourseDtoBuilder[] builders = { response.SubjectCourse.MapToBuilder() };

        IEnumerable<SubjectCourseDto> subjectCourses = await _enrichmentProcessor
            .EnrichAsync(builders, cancellationToken);

        return Ok(subjectCourses.Single());
    }

    [HttpGet("{id:guid}")]
    [AuthorizeFeature(Scope, nameof(GetById))]
    public async Task<ActionResult<SubjectCourseDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var request = new GetByIdRequest { Id = id.ToString() };

        GetByIdResponse response = await _subjectCourseClient
            .GetByIdAsync(request, cancellationToken: cancellationToken);

        SubjectCourseDtoBuilder[] builders = { response.SubjectCourse.MapToBuilder() };

        IEnumerable<SubjectCourseDto> subjectCourses = await _enrichmentProcessor
            .EnrichAsync(builders, cancellationToken);

        return Ok(subjectCourses.Single());
    }

    [HttpPut("{id:guid}")]
    [AuthorizeFeature(Scope, nameof(Update))]
    public async Task<ActionResult<SubjectCourseDto>> Update(
        Guid id,
        [FromBody] UpdateSubjectCourseRequest request,
        CancellationToken cancellationToken)
    {
        var grpcRequest = new UpdateRequest
        {
            Id = id.ToString(),
            Title = request.Name,
        };

        UpdateResponse response = await _subjectCourseClient
            .UpdateAsync(grpcRequest, cancellationToken: cancellationToken);

        SubjectCourseDtoBuilder[] builders = { response.SubjectCourse.MapToBuilder() };

        IEnumerable<SubjectCourseDto> subjectCourses = await _enrichmentProcessor
            .EnrichAsync(builders, cancellationToken);

        return Ok(subjectCourses.Single());
    }

    [HttpGet("{id:guid}/students")]
    [AuthorizeFeature(Scope, nameof(GetStudents))]
    public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudents(
        Guid id,
        CancellationToken cancellationToken)
    {
        var request = new GetStudentsRequest { SubjectCourseId = id.ToString() };

        GetStudentsResponse response = await _subjectCourseClient
            .GetStudentsAsync(request, cancellationToken: cancellationToken);

        IEnumerable<StudentDtoBuilder> builders = response.Students.Select(x => x.MapToBuilder());
        IEnumerable<StudentDto> students = await _studentProcessor.EnrichAsync(builders, cancellationToken);

        return Ok(students);
    }

    [HttpGet("{id:guid}/assignments")]
    [AuthorizeFeature(Scope, nameof(GetAssignments))]
    public async Task<ActionResult<IEnumerable<AssignmentDto>>> GetAssignments(
        Guid id,
        CancellationToken cancellationToken)
    {
        var request = new GetAssignmentsRequest { SubjectCourseId = id.ToString() };

        GetAssignmentsResponse response = await _subjectCourseClient
            .GetAssignmentsAsync(request, cancellationToken: cancellationToken);

        IEnumerable<AssignmentDto> assignments = response.Assignments.Select(x => x.ToDto());

        return Ok(assignments);
    }

    [HttpGet("{id:guid}/groups")]
    [AuthorizeFeature(Scope, nameof(GetGroups))]
    public async Task<ActionResult<IEnumerable<SubjectCourseGroupDto>>> GetGroups(
        Guid id,
        CancellationToken cancellationToken)
    {
        var request = new GetGroupsRequest { SubjectCourseId = id.ToString() };

        GetGroupsResponse response = await _subjectCourseClient
            .GetGroupsAsync(request, cancellationToken: cancellationToken);

        IEnumerable<SubjectCourseGroupDto> groups = response.Groups.Select(x => x.ToDto());

        return Ok(groups);
    }

    [HttpGet("{subjectCourseId:guid}/groups/{studentGroupId:guid}/queue")]
    [AuthorizeFeature(Scope, nameof(GetStudentGroupQueue))]
    public async Task<ActionResult<SubmissionsQueueDto>> GetStudentGroupQueue(
        Guid subjectCourseId,
        Guid studentGroupId,
        CancellationToken cancellationToken)
    {
        var request = new GetStudentGroupQueueRequest
        {
            SubjectCourseId = subjectCourseId.ToString(),
            StudentGroupId = studentGroupId.ToString(),
        };

        GetStudentGroupQueueResponse response = await _subjectCourseClient
            .GetStudentGroupQueueAsync(request, cancellationToken: cancellationToken);

        IEnumerable<StudentDtoBuilder> studentBuilders = response.Queue.Students
            .DistinctBy(x => x.User.Id)
            .Select(x => x.MapToBuilder());

        IEnumerable<StudentDto> students = await _studentProcessor.EnrichAsync(studentBuilders, cancellationToken);
        IEnumerable<SubmissionDto> submissions = response.Queue.Submissions.Select(x => x.ToDto());

        var queue = new SubmissionsQueueDto(
            response.Queue.GroupName,
            students.ToArray(),
            submissions.ToArray());

        return Ok(queue);
    }

    [HttpPost("{id:guid}/deadline/fraction")]
    [AuthorizeFeature(Scope, nameof(AddDeadline))]
    public async Task<ActionResult> AddDeadline(
        Guid id,
        AddFractionPolicyRequest request,
        CancellationToken cancellationToken)
    {
        var grpcRequest = new AddDeadlineRequest
        {
            SubjectCourseId = id.ToString(),
            Fraction = request.Fraction,
            SpanBeforeActivation = Duration.FromTimeSpan(request.SpanBeforeActivation),
        };

        await _subjectCourseClient.AddDeadlineAsync(grpcRequest, cancellationToken: cancellationToken);

        return Ok();
    }

    [HttpPost("{id:guid}/points/forceSync")]
    [AuthorizeFeature(Scope, nameof(ForceSyncPoints))]
    public async Task<IActionResult> ForceSyncPoints(Guid id, CancellationToken cancellationToken)
    {
        var request = new ForceSyncPointsRequest { SubjectCourseId = id.ToString() };
        await _subjectCourseClient.ForceSyncPointsAsync(request, cancellationToken: cancellationToken);

        return Ok();
    }

    [HttpPut("{subjectCourseId:guid}/github/mentorTeam")]
    [AuthorizeFeature(Scope, nameof(GithubUpdateMentorsTeam))]
    public async Task<ActionResult> GithubUpdateMentorsTeam(
        Guid subjectCourseId,
        [FromBody] UpdateMentorsTeamNameRequest request,
        CancellationToken cancellationToken)
    {
        var grpcRequest = new UpdateMentorTeamRequest
        {
            SubjectCourseId = subjectCourseId.ToString(),
            MentorTeamId = request.TeamId,
        };

        await _githubSubjectCourseClient.UpdateMentorTeamAsync(grpcRequest, cancellationToken: cancellationToken);

        return Ok();
    }
}
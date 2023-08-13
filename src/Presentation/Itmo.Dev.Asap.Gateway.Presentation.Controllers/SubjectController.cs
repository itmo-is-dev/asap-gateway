using Itmo.Dev.Asap.Core.Subjects;
using Itmo.Dev.Asap.Gateway.Application.Abstractions.Enrichment;
using Itmo.Dev.Asap.Gateway.Application.Abstractions.Enrichment.Builders;
using Itmo.Dev.Asap.Gateway.Application.Dto.Study;
using Itmo.Dev.Asap.Gateway.Application.Dto.SubjectCourses;
using Itmo.Dev.Asap.Gateway.Core.Mapping;
using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Subjects;
using Itmo.Dev.Asap.Gateway.Presentation.Authorization;
using Itmo.Dev.Asap.Gateway.Presentation.Controllers.Mapping;
using Microsoft.AspNetCore.Mvc;
using CreateSubjectRequest = Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Subjects.CreateSubjectRequest;

namespace Itmo.Dev.Asap.Gateway.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SubjectController : ControllerBase
{
    private const string Scope = "Subjects";

    private readonly SubjectService.SubjectServiceClient _subjectClient;
    private readonly IEnrichmentProcessor<string, SubjectCourseDtoBuilder, SubjectCourseDto> _enrichmentProcessor;

    public SubjectController(
        SubjectService.SubjectServiceClient subjectClient,
        IEnrichmentProcessor<string, SubjectCourseDtoBuilder, SubjectCourseDto> enrichmentProcessor)
    {
        _subjectClient = subjectClient;
        _enrichmentProcessor = enrichmentProcessor;
    }

    [HttpPost]
    [AuthorizeFeature(Scope, nameof(Create))]
    public async Task<ActionResult<SubjectDto>> Create(
        [FromBody] CreateSubjectRequest request,
        CancellationToken cancellationToken)
    {
        var grpcRequest = new Itmo.Dev.Asap.Core.Subjects.CreateSubjectRequest
        {
            Title = request.Name,
        };

        CreateSubjectResponse response = await _subjectClient
            .CreateSubjectAsync(grpcRequest, cancellationToken: cancellationToken);

        SubjectDto subject = response.Subject.ToDto();

        return Ok(subject);
    }

    [HttpGet]
    [AuthorizeFeature(Scope, nameof(GetAll))]
    public async Task<ActionResult<IEnumerable<SubjectDto>>> GetAll(CancellationToken cancellationToken)
    {
        var request = new GetAllRequest();
        GetAllResponse response = await _subjectClient.GetAllAsync(request, cancellationToken: cancellationToken);

        IEnumerable<SubjectDto> subjects = response.Subjects.Select(x => x.ToDto());

        return Ok(subjects);
    }

    [HttpGet("{id:guid}")]
    [AuthorizeFeature(Scope, nameof(GetById))]
    public async Task<ActionResult<SubjectDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var request = new GetByIdRequest { Id = id.ToString() };
        GetByIdResponse response = await _subjectClient.GetByIdAsync(request, cancellationToken: cancellationToken);

        SubjectDto subject = response.Subject.ToDto();

        return Ok(subject);
    }

    [HttpPut("{id:guid}")]
    [AuthorizeFeature(Scope, nameof(Update))]
    public async Task<ActionResult<SubjectDto>> Update(
        Guid id,
        [FromBody] UpdateSubjectRequest request,
        CancellationToken cancellationToken)
    {
        var grpcRequest = new UpdateRequest
        {
            Id = id.ToString(),
            Name = request.Name,
        };

        UpdateResponse response = await _subjectClient
            .UpdateAsync(grpcRequest, cancellationToken: cancellationToken);

        SubjectDto subject = response.Subject.ToDto();

        return Ok(subject);
    }

    [HttpGet("{id:guid}/courses")]
    [AuthorizeFeature(Scope, nameof(GetSubjectCourses))]
    public async Task<ActionResult<IEnumerable<SubjectCourseDto>>> GetSubjectCourses(
        Guid id,
        CancellationToken cancellationToken)
    {
        var request = new GetCoursesRequest { SubjectId = id.ToString() };

        GetCoursesResponse response = await _subjectClient
            .GetCoursesAsync(request, cancellationToken: cancellationToken);

        IEnumerable<SubjectCourseDtoBuilder> builders = response.SubjectCourses.Select(x => x.MapToBuilder());
        IEnumerable<SubjectCourseDto> courses = await _enrichmentProcessor.EnrichAsync(builders, cancellationToken);

        return Ok(courses);
    }
}
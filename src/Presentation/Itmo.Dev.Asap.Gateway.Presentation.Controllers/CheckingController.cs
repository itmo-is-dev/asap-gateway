using Itmo.Dev.Asap.Checker;
using Itmo.Dev.Asap.Gateway.Application.Abstractions.Enrichment;
using Itmo.Dev.Asap.Gateway.Application.Abstractions.Enrichment.Builders;
using Itmo.Dev.Asap.Gateway.Application.Dto.Checking;
using Itmo.Dev.Asap.Gateway.Checker.Mapping;
using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models;
using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Checking;
using Itmo.Dev.Asap.Gateway.Presentation.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Itmo.Dev.Asap.Gateway.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CheckingController : ControllerBase
{
    private const string Scope = "Checking";

    private readonly CheckingService.CheckingServiceClient _client;

    private readonly IEnrichmentProcessor<CheckingResultKey, CheckingResultBuilder, CheckingResultDto> _resultEnricher;

    public CheckingController(
        CheckingService.CheckingServiceClient client,
        IEnrichmentProcessor<CheckingResultKey, CheckingResultBuilder, CheckingResultDto> resultEnricher)
    {
        _client = client;
        _resultEnricher = resultEnricher;
    }

    [HttpPost]
    [AuthorizeFeature(Scope, nameof(GetCheckings))]
    public async Task<ActionResult<GetCheckingsResponse>> GetCheckings(
        [FromBody] GetCheckingsRequest request,
        CancellationToken cancellationToken)
    {
        var grpcRequest = new GetTasksRequest
        {
            SubjectCourseId = request.SubjectCourseId.ToString(),
            PageSize = request.PageSize,
            PageToken = request.PageToken,
        };

        GetTasksResponse grpcResponse = await _client
            .GetTasksAsync(grpcRequest, cancellationToken: cancellationToken);

        var response = new GetCheckingsResponse(
            grpcResponse.Tasks.Select(x => x.MapToDto()),
            grpcResponse.PageToken);

        return Ok(response);
    }

    [HttpPost("results")]
    [AuthorizeFeature(Scope, nameof(GetCheckingResults))]
    public async Task<ActionResult<GetCheckingResultsResponse>> GetCheckingResults(
        [FromBody] GetCheckingResultsRequest request,
        CancellationToken cancellationToken)
    {
        var grpcRequest = new GetResultsRequest
        {
            TaskId = request.CheckingId,
            AssignmentIds = { request.AssignmentIds.Select(x => x.ToString()) },
            GroupIds = { request.GroupIds.Select(x => x.ToString()) },
            PageSize = request.PageSize,
            PageToken = request.PageToken,
        };

        GetResultsResponse grpcResponse = await _client
            .GetResultsAsync(grpcRequest, cancellationToken: cancellationToken);

        IEnumerable<CheckingResultBuilder> builders = grpcResponse.Results.Select(result => new CheckingResultBuilder(
            Guid.Parse(result.AssignmentId),
            result.FirstSubmission.MapToDto(),
            result.SecondSubmission.MapToDto(),
            result.SimilarityScore));

        IEnumerable<CheckingResultDto> results = await _resultEnricher.EnrichAsync(builders, cancellationToken);

        return Ok(new GetCheckingResultsResponse(results, grpcResponse.PageToken));
    }

    [HttpPost("codeBlocks")]
    [AuthorizeFeature(Scope, nameof(GetCheckingCodeBlocks))]
    public async Task<ActionResult<GetCheckingCodeBlocksResponse>> GetCheckingCodeBlocks(
        [FromBody] GetCheckingCodeBlocksRequest request,
        CancellationToken cancellationToken)
    {
        var grpcRequest = new GetResultCodeBlocksRequest
        {
            TaskId = request.CheckingId,
            FirstSubmissionId = request.FirstSubmissionId.ToString(),
            SecondSubmissionId = request.SecondSubmissionId.ToString(),
            PageSize = request.PageSize,
            Cursor = request.Cursor,
        };

        GetResultCodeBlocksResponse grpcResponse = await _client
            .GetResultCodeBlocksAsync(grpcRequest, cancellationToken: cancellationToken);

        IEnumerable<SimilarCodeBlocksDto> codeBlocks = grpcResponse.CodeBlocks
            .Select(x => x.MapToDto());

        var response = new GetCheckingCodeBlocksResponse(
            codeBlocks,
            grpcResponse.CodeBlocks.Count,
            grpcResponse.HasNext);

        return Ok(response);
    }

    [HttpPost("start")]
    [AuthorizeFeature(Scope, nameof(Start))]
    public async Task<ActionResult<CheckingDto>> Start(
        [FromBody] StartCheckingRequest request,
        CancellationToken cancellationToken)
    {
        var grpcRequest = new StartRequest
        {
            SubjectCourseId = request.SubjectCourseId.ToString(),
        };

        StartResponse grpcResponse = await _client
            .StartAsync(grpcRequest, cancellationToken: cancellationToken);

        return grpcResponse.ResultCase switch
        {
            StartResponse.ResultOneofCase.Success => Ok(grpcResponse.Success.Checking.MapToDto()),

            StartResponse.ResultOneofCase.SubjectCourseNotFound
                => NotFound(new ErrorDetails("Subject course not found")),

            StartResponse.ResultOneofCase.AlreadyInProgress
                => Conflict(new ErrorDetails("Checking task already in progress")),

            _ or StartResponse.ResultOneofCase.None
                => throw new UnreachableException("Operation yielded unexpected result"),
        };
    }
}
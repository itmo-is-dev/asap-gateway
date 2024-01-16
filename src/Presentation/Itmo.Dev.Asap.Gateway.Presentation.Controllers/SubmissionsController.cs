using Itmo.Dev.Asap.Core.Submissions;
using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Submissions;
using Itmo.Dev.Asap.Gateway.Presentation.Authorization;
using Itmo.Dev.Asap.Gateway.Presentation.Controllers.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Itmo.Dev.Asap.Gateway.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubmissionsController : ControllerBase
{
    private const string Scope = "Submissions";

    private readonly SubmissionService.SubmissionServiceClient _client;

    public SubmissionsController(SubmissionService.SubmissionServiceClient client)
    {
        _client = client;
    }

    [HttpPost("ban")]
    [AuthorizeFeature(Scope, nameof(Ban))]
    public async Task<ActionResult> Ban([FromBody] BanSubmissionRequest request, CancellationToken cancellationToken)
    {
        var grpcRequest = new BanRequest
        {
            StudentId = request.StudentId.ToString(),
            AssignmentId = request.AssignmentId.ToString(),
            IssuerId = HttpContext.User.GetId().ToString(),
        };

        await _client.BanAsync(grpcRequest, cancellationToken: cancellationToken);

        return Ok();
    }
}
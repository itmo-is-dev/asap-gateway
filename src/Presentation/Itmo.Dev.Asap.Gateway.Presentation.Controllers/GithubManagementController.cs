using Itmo.Dev.Asap.Gateway.Presentation.Authorization;
using Itmo.Dev.Asap.Github;
using Microsoft.AspNetCore.Mvc;

namespace Itmo.Dev.Asap.Gateway.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GithubManagementController : ControllerBase
{
    private const string Scope = "Github";

    private readonly GithubManagementService.GithubManagementServiceClient _client;

    public GithubManagementController(GithubManagementService.GithubManagementServiceClient client)
    {
        _client = client;
    }

    [HttpPost("force-update")]
    [AuthorizeFeature(Scope, nameof(ForceOrganizationUpdate))]
    public IActionResult ForceOrganizationUpdate(
        [FromQuery] Guid? subjectCourseId,
        CancellationToken cancellationToken)
    {
        if (subjectCourseId is null)
        {
            var request = new ForceAllOrganizationsUpdateRequest();
            _client.ForceAllOrganizationsUpdateAsync(request);
        }
        else
        {
            var request = new ForceOrganizationUpdateRequest { SubjectCourseId = subjectCourseId.ToString() };
            _client.ForceOrganizationUpdateAsync(request);
        }

        return Accepted();
    }

    [HttpPost("force-mentor-sync")]
    [AuthorizeFeature(Scope, nameof(ForceMentorSync))]
    public async Task<ActionResult> ForceMentorSync(long organizationId, CancellationToken cancellationToken)
    {
        var request = new ForceMentorSyncRequest { OrganizationId = organizationId };
        await _client.ForceMentorSyncAsync(request, cancellationToken: cancellationToken);

        return Ok();
    }
}
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
    public async Task<IActionResult> ForceOrganizationUpdate(
        [FromQuery] Guid? subjectCourseId,
        CancellationToken cancellationToken)
    {
        if (subjectCourseId is null)
        {
            var request = new ForceAllOrganizationsUpdateRequest();
            await _client.ForceAllOrganizationsUpdateAsync(request, cancellationToken: cancellationToken);
        }
        else
        {
            var request = new ForceOrganizationUpdateRequest { SubjectCourseId = subjectCourseId.ToString() };
            await _client.ForceOrganizationUpdateAsync(request, cancellationToken: cancellationToken);
        }

        return Ok();
    }

    [HttpPost("force-mentor-sync")]
    [AuthorizeFeature(Scope, nameof(ForceMentorSync))]
    public async Task<ActionResult> ForceMentorSync(string organizationName, CancellationToken cancellationToken)
    {
        var request = new ForceMentorSyncRequest { OrganizationName = organizationName };
        await _client.ForceMentorSyncAsync(request, cancellationToken: cancellationToken);

        return Ok();
    }
}
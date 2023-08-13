using Refit;

namespace Itmo.Dev.Asap.Gateway.Sdk.Clients;

public interface IGithubManagementClient
{
    [Post("/api/githubManagement/force-update")]
    Task<IApiResponse> ForceOrganizationUpdateAsync([Query] Guid subjectCourseId, CancellationToken cancellationToken);

    [Post("/api/githubManagement/force-mentor-sync")]
    Task<IApiResponse> ForceMentorSyncAsync(string organizationName, CancellationToken cancellationToken);
}
using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Submissions;
using Refit;

namespace Itmo.Dev.Asap.Gateway.Sdk.Clients;

public interface ISubmissionsClient
{
    [Post("/api/submissions/ban")]
    Task<IApiResponse> BanAsync([Body]BanSubmissionRequest request, CancellationToken cancellationToken);
}
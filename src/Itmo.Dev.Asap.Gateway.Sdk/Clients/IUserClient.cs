using Itmo.Dev.Asap.Gateway.Application.Dto.Users;
using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Users;
using Refit;

namespace Itmo.Dev.Asap.Gateway.Sdk.Clients;

public interface IUserClient
{
    [Get("/api/user")]
    Task<IApiResponse<UserDto>> FindByUniversityIdAsync([Query] int universityId, CancellationToken cancellationToken);

    [Get("/api/user")]
    Task<IApiResponse<UserDto>> FindCurrentUserAsync(CancellationToken cancellationToken);

    [Put("/api/user/{userId}/universityId")]
    Task<IApiResponse<UserDto>> UpdateUniversityIdAsync(
        Guid userId,
        [Query] int universityId,
        CancellationToken cancellationToken);

    [Put("/api/user/{userId}/name")]
    Task<IApiResponse<UserDto>> UpdateNameAsync(
        Guid userId,
        [Body] UpdateNameRequest request,
        CancellationToken cancellationToken);

    [Put("/api/user/{userId}/github/username")]
    Task<IApiResponse> UpdateGithubUsernameAsync(Guid userId, string username, CancellationToken cancellationToken);

    [Post("/api/user/identityInfo/query")]
    Task<IApiResponse<QueryUserIdentityInfoResponse>> QueryIdentityInfoAsync(
        [Body] QueryUserIdentityInfoRequest request,
        CancellationToken cancellationToken);
}
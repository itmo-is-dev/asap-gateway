using Itmo.Dev.Asap.Gateway.Application.Dto.Identity;
using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Identity;
using Refit;

namespace Itmo.Dev.Asap.Gateway.Sdk.Clients;

public interface IIdentityClient
{
    [Post("/api/identity/login")]
    Task<IApiResponse<LoginResponse>> LoginAsync(
        [Body] LoginRequest request,
        CancellationToken cancellationToken);

    [Post("/api/identity/{username}/role")]
    Task<IApiResponse> ChangeUserRoleAsync(
        string username,
        [Query] string roleName,
        CancellationToken cancellationToken);

    [Post("/api/identity/{userId}/account")]
    Task<IApiResponse> CreateUserAccountAsync(
        Guid userId,
        [Body] CreateUserAccountRequest request,
        CancellationToken cancellationToken);

    [Put("/api/identity/username")]
    Task<IApiResponse<UpdateUsernameResponse>> UpdateUsernameAsync(
        [Body] UpdateUsernameRequest request,
        CancellationToken cancellationToken);

    [Put("/api/identity/password")]
    Task<IApiResponse<UpdatePasswordResponse>> UpdatePasswordAsync(
        [Body] UpdatePasswordRequest request,
        CancellationToken cancellationToken);

    [Get("/api/identity/password/options")]
    Task<IApiResponse<PasswordOptionsDto>> GetPasswordOptionsAsync(CancellationToken cancellationToken);

    [Get("/api/identity/roles")]
    Task<IApiResponse<IReadOnlyCollection<string>>> GetRolesAsync(CancellationToken cancellationToken);
}
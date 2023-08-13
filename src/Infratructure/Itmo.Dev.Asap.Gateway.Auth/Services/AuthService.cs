using Itmo.Dev.Asap.Auth;
using Itmo.Dev.Asap.Gateway.Application.Abstractions.Services;

namespace Itmo.Dev.Asap.Gateway.Auth.Services;

public class AuthService : IAuthService
{
    private readonly IdentityService.IdentityServiceClient _identityClient;

    public AuthService(IdentityService.IdentityServiceClient identityClient)
    {
        _identityClient = identityClient;
    }

    public async Task<bool> IsTokenValid(string token, CancellationToken cancellationToken)
    {
        var validationRequest = new ValidateTokenRequest
        {
            Token = token,
        };

        ValidateTokenResponse validationResponse = await _identityClient.ValidateTokenAsync(validationRequest);

        return validationResponse.IsTokenValid;
    }
}
using Itmo.Dev.Asap.Gateway.Application.Abstractions.Services;
using Itmo.Dev.Asap.Gateway.Presentation.Authentication.Providers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Itmo.Dev.Asap.Gateway.Presentation.Authentication;

internal class AuthAuthenticationHandler : AuthenticationHandler<AuthAuthenticationOptions>
{
    public AuthAuthenticationHandler(
        IOptionsMonitor<AuthAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock) : base(options, logger, encoder, clock) { }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        IServiceProvider services = Request.HttpContext.RequestServices;

        IAuthService authService = Request.HttpContext.RequestServices
            .GetRequiredService<IAuthService>();

        TokenProvider tokenProvider = services.GetRequiredService<TokenProvider>();

        string? authorizationHeader = Request.Headers.Authorization.ToString();

        string? queryAccessToken = Request.Query["access_token"];

        if (string.IsNullOrEmpty(authorizationHeader))
            authorizationHeader = queryAccessToken;

        if (string.IsNullOrEmpty(authorizationHeader))
            return AuthenticateResult.NoResult();

        string token = authorizationHeader[(Scheme.Name.Length + 1)..];

        if (string.IsNullOrEmpty(token))
            return AuthenticateResult.NoResult();

        if (await authService.IsTokenValid(token, default) is false)
        {
            return AuthenticateResult.Fail("Invalid token");
        }

        tokenProvider.Token = token;

        JwtSecurityToken securityToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
        var principal = new ClaimsPrincipal(new ClaimsIdentity(securityToken.Claims, "Jwt"));

        return AuthenticateResult.Success(new AuthenticationTicket(principal, Scheme.Name));
    }
}
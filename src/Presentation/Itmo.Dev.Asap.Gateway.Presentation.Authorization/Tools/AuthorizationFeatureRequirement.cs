using Microsoft.AspNetCore.Authorization;

namespace Itmo.Dev.Asap.Gateway.Presentation.Authorization.Tools;

public class AuthorizationFeatureRequirement : IAuthorizationRequirement
{
    public AuthorizationFeatureRequirement(string scope, string feature)
    {
        Scope = scope;
        Feature = feature;
    }

    public string Scope { get; }

    public string Feature { get; }
}
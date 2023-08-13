using Microsoft.AspNetCore.Authorization;

namespace Itmo.Dev.Asap.Gateway.Presentation.Authorization;

public sealed class AuthorizeFeatureAttribute : AuthorizeAttribute
{
    internal const string Prefix = "Feature_";

    public AuthorizeFeatureAttribute(string scope, string feature)
    {
        Scope = scope;
        Feature = feature;
        Policy = $"{Prefix}{Scope}:{Feature}";
    }

    public string Scope { get; }

    public string Feature { get; }
}
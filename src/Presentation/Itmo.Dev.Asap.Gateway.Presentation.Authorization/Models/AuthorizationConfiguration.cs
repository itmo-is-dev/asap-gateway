namespace Itmo.Dev.Asap.Gateway.Presentation.Authorization.Models;

internal class AuthorizationConfiguration
{
    public FeatureScopes FeatureScopes { get; init; } = new FeatureScopes();
}
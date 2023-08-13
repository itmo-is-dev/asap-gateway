namespace Itmo.Dev.Asap.Gateway.Presentation.Authorization.Models;

internal class FeatureScopes : Dictionary<string, FeatureScope>
{
    public FeatureScopes() : base(StringComparer.OrdinalIgnoreCase) { }
}

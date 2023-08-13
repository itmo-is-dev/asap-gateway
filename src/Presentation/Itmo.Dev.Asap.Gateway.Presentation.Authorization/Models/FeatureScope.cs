namespace Itmo.Dev.Asap.Gateway.Presentation.Authorization.Models;

internal class FeatureScope : Dictionary<string, FeatureRoles>
{
    public FeatureScope() : base(StringComparer.OrdinalIgnoreCase) { }
}
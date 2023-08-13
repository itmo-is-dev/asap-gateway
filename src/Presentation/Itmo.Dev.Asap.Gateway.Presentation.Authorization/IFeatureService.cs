using System.Security.Claims;

namespace Itmo.Dev.Asap.Gateway.Presentation.Authorization;

public interface IFeatureService
{
    bool HasFeature(ClaimsPrincipal principal, string scope, string feature);
}
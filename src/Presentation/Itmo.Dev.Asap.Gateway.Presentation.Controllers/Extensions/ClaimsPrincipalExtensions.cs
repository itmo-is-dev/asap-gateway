using System.Security.Claims;

namespace Itmo.Dev.Asap.Gateway.Presentation.Controllers.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetId(this ClaimsPrincipal principal)
    {
        Claim claim = principal.Claims
            .Single(x => x.Type.Equals(ClaimTypes.NameIdentifier, StringComparison.OrdinalIgnoreCase));

        return Guid.TryParse(claim.Value, out Guid id)
            ? id
            : throw new InvalidOperationException("User is not authenticated");
    }
}
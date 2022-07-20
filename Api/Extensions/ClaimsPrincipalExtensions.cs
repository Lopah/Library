using System.Security.Claims;
using JetBrains.Annotations;

namespace Api.Extensions;

[PublicAPI]
public static class ClaimsPrincipalExtensions
{
    public static IEnumerable<string> GetUserRoles(this ClaimsPrincipal principal)
    {
        if (principal is null)
        {
            throw new ArgumentNullException(nameof(principal));
        }

        return principal.Claims.Where(e => e.Type == "role").Select(x => x.Value);
    }

    public static Guid GetUserId(this ClaimsPrincipal principal)
    {
        if (principal is null)
        {
            throw new ArgumentNullException(nameof(principal));
        }

        return Guid.Parse(principal.GetUserIdClaim().Value);
    }

    public static Claim GetUserIdClaim(this ClaimsPrincipal principal)
    {
        if (principal is null)
        {
            throw new ArgumentNullException(nameof(principal));
        }

        return principal.FindFirst("sub") ?? principal.FindFirst(ClaimTypes.NameIdentifier);
    }
}
using System;
using System.Linq;
using System.Security.Claims;

namespace Etch.OrchardCore.ContentPermissions
{
    public static class CanAccessUserExtension
    {
        public static bool CanAccess(this ClaimsPrincipal user, string[] roles)
        {
            if (roles.Contains("Anonymous"))
            {
                return true;
            }

            if (user == null)
            {
                return false;
            }

            if (roles.Contains("Authenticated") && user.Identity.IsAuthenticated)
            {
                return true;
            }

            foreach (var role in roles)
            {
                if (user.IsInRole(role))
                {
                    return true;
                }
            }

            return false;
        }
    }
}

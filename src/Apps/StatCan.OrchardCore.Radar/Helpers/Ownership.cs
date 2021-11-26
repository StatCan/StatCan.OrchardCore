using System.Security.Claims;
using OrchardCore.ContentManagement;

namespace StatCan.OrchardCore.Radar.Helpers
{
    public static class Ownership
    {
        public static bool IsOwner(ContentItem contentItem, ClaimsPrincipal user)
        {
            if(user == null)
            {
                return false;
            }

            // Assume admin owns everything
            if (user.IsInRole("Administrator"))
            {
                return true;
            }

            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier).ToString();

            return userId == contentItem.Owner;
        }
    }
}

namespace Etch.OrchardCore.ContentPermissions.Models
{
    public class ContentPermissionsPartSettings
    {
        public string RedirectUrl { get; set; }

        public bool HasRedirectUrl
        {
            get { return !string.IsNullOrWhiteSpace(RedirectUrl); }
        }
    }
}

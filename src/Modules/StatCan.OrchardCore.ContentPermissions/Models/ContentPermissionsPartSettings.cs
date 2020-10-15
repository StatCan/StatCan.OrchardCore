namespace Etch.OrchardCore.ContentPermissions.Models
{
    public class ContentPermissionsPartSettings
    {
        public bool DisableRedirect { get; set; }
        public string RedirectUrl { get; set; }

        public bool HasRedirectUrl
        {
            get { return !string.IsNullOrWhiteSpace(RedirectUrl); }
        }
    }
}

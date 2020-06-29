namespace StatCan.OrchardCore.GCCollab.Configuration
{
    public static class GCCollabDefaults
    {
        public const string AuthenticationScheme = "GCCollab";
        public static readonly string DisplayName = "GCCollab";
        public static readonly string AuthorizationEndpoint = "https://account.gccollab.ca/oauth/v2/authorize";
        public static readonly string TokenEndpoint = "https://account.gccollab.ca/oauth/v2/token";
        public static readonly string UserInformationEndpoint = "https://account.gccollab.ca/api/users/me";
    }
}

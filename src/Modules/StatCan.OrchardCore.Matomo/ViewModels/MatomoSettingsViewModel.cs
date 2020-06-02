using System.ComponentModel.DataAnnotations;

namespace StatCan.OrchardCore.Matomo.ViewModels
{
    public class MatomoSettingsViewModel
    {
        [Required(AllowEmptyStrings = false)]
        public string SiteID { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string ServerUri { get; set; }
    }
}

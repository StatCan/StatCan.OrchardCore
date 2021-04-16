using System.ComponentModel.DataAnnotations;

namespace StatCan.OrchardCore.SaaSConfiguration.ViewModels
{
    public class SaasConfigurationSettingsViewModel
    {
        [Required(ErrorMessage = "ClientId is required")]
        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string Authority { get; set; }
    }
}

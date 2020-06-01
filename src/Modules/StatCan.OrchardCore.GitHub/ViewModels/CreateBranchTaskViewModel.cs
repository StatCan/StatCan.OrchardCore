using System.ComponentModel.DataAnnotations;

namespace StatCan.OrchardCore.GitHub.ViewModels
{
    public class CreateBranchTaskViewModel
    {
        [Required]
        public string TokenName { get; set; }
        [Required]
        public string Owner { get; set; }
        [Required]
        public string Repo { get; set; }
        [Required]
        public string ReferenceName { get; set; }
        [Required]
        public string TargetBranchName { get; set; }
    }
}

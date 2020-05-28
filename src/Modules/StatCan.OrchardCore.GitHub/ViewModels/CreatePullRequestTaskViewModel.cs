using System.ComponentModel.DataAnnotations;

namespace StatCan.OrchardCore.GitHub.ViewModels
{
    public class CreatePullRequestTaskViewModel
    {
        [Required]
        public string TokenName { get; set; }
        [Required]
        public string Owner { get; set; }
        [Required]
        public string Repo { get; set; }
        [Required]
        public string SourceBranch { get; set; }
        [Required]
        public string TargetBranch { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StatCan.OrchardCore.GitHub.ViewModels
{
    public class CreateBranchTaskViewModel
    {
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

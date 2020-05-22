using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StatCan.OrchardCore.GitHub.ViewModels
{
    public class CreateIssueTaskViewModel
    {
        [Required]
        public string Owner { get; set; }
        [Required]
        public string Repo { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public string Labels { get; set; }
    }
}

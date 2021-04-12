using System.ComponentModel.DataAnnotations;

namespace StatCan.OrchardCore.EmailTemplates.ViewModels
{
    public class EmailTemplateViewModel
    {
        #region Template Properties

        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        #endregion

        #region Email Properties

        public string AuthorExpression { get; set; }
        public string SenderExpression { get; set; }
        public string ReplyToExpression { get; set; }

        [Required]
        public string RecipientsExpression { get; set; }
        public string CCExpression { get; set; }
        public string BCCExpression { get; set; }
        public string SubjectExpression { get; set; }
        [Required]
        public string Body { get; set; }
        public bool IsBodyHtml { get; set; }

        #endregion
    }
}

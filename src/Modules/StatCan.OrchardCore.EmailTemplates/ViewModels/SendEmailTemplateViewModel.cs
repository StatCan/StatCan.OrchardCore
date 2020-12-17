using System.ComponentModel.DataAnnotations;

namespace StatCan.OrchardCore.EmailTemplates.ViewModels
{
    public class SendEmailTemplateViewModel
    {
        #region Template Properties

        public string Name { get; set; }

        #endregion

        #region Email Properties

        public string Author { get; set; }
        public string Sender { get; set; }
        public string ReplyTo { get; set; }

        [Required]
        public string Recipients { get; set; }
        //public string CC { get; set; }
        //public string BCC { get; set; }
        public string Subject { get; set; }
        [Required]
        public string Body { get; set; }
        public bool IsBodyHtml { get; set; }

        #endregion
    }
}

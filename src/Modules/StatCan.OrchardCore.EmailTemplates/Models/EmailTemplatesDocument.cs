using System;
using System.Collections.Generic;
using OrchardCore.Data.Documents;

namespace StatCan.OrchardCore.EmailTemplates.Models
{
    public class EmailTemplatesDocument : Document
    {
        public Dictionary<string, EmailTemplate> Templates { get; set; } = new Dictionary<string, EmailTemplate>(StringComparer.OrdinalIgnoreCase);
    }

    public class EmailTemplate
    {
        #region Template Properties

        public string Description { get; set; }

        #endregion

        #region Email Properties

        public string AuthorExpression { get; set; }
        public string SenderExpression { get; set; }
        public string ReplyToExpression { get; set; }
        public string RecipientsExpression { get; set; }
        public string SubjectExpression { get; set; }
        public string Body { get; set; }
        public bool IsBodyHtml { get; set; }

        #endregion
    }
}

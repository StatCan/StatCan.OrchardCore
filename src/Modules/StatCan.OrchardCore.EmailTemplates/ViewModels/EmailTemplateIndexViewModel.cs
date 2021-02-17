using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using StatCan.OrchardCore.EmailTemplates.Models;

namespace StatCan.OrchardCore.EmailTemplates.ViewModels
{
    public class EmailTemplateIndexViewModel
    {
        public IList<TemplateEntry> Templates { get; set; }
        public dynamic Pager { get; set; }
        public ContentOptions Options { get; set; } = new ContentOptions();
    }

    public class TemplateEntry
    {
        public string Id { get; set; }
        public EmailTemplate Template { get; set; }
        public bool IsChecked { get; set; }
    }

    public class ContentOptions
    {
        public string Search { get; set; }
        public ContentsBulkAction BulkAction { get; set; }

        #region Lists to populate

        [BindNever]
        public List<SelectListItem> ContentsBulkAction { get; set; }

        #endregion Lists to populate
    }

    public enum ContentsBulkAction
    {
        None,
        Remove
    }
}

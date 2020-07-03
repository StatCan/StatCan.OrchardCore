using StatCan.OrchardCore.LocalizedText.Fields;
using OrchardCore.ContentManagement;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;
using System.Collections.Generic;

namespace StatCan.OrchardCore.LocalizedText.ViewModels
{
    public class EditLocalizedTextFieldViewModel
    {
        public string Data { get; set; }
        [BindNever]
        public ContentItem ContentItem { get; set; }
        [BindNever]
        public LocalizedTextPart Part { get; set; }
        [BindNever]
        public string SupportedCultures { get; set; }

    }
}

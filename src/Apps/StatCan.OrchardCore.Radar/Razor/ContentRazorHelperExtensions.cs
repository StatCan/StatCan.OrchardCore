using Microsoft.Extensions.DependencyInjection;
using OrchardCore;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class ContentRazorHelperExtensions
{
    public static IList<ContentTypeDefinitionRecord> GetContentTypeByContentPartAsync(this IOrchardHelper orchardHelper, string partName = "")
    {
        var contentDefinitionStore = orchardHelper.HttpContext.RequestServices.GetService<IContentDefinitionStore>();

        var document = contentDefinitionStore.GetContentDefinitionAsync().GetAwaiter().GetResult().ContentTypeDefinitionRecords;

        return document.Where(contentType =>
            contentType.ContentTypePartDefinitionRecords.Where(part => part.Name == partName).Count() > 0
        ).ToList();
    }
}

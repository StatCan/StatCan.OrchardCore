using System;
using OrchardCore.ContentManagement;
using YesSql.Indexes;
using Etch.OrchardCore.ContentPermissions.Models;

namespace Etch.OrchardCore.ContentPermissions.Indexing
{
    public class ContentPermissionsPartIndex : MapIndex
    {
        public string ContentItemId { get; set; }

        public string ContentType { get; set; }

        public bool Published { get; set; }

        public DateTime? PublishedUtc { get; set; }

        public string Roles { get; set; }

        public bool Enabled { get; set; }
    }

    public class ContentPermissionsPartIndexProvider : IndexProvider<ContentItem>
    {
        public override void Describe(DescribeContext<ContentItem> context)
        {
            context.For<ContentPermissionsPartIndex>()
                .Map(contentItem =>
                {
                    var contentPermissionsPart = contentItem.As<ContentPermissionsPart>();

                    // Ignore if does not use content permission
                    if (contentPermissionsPart == null)
                    {
                        return null;
                    }

                    // Ignore previous versions of the content item
                    if (!contentItem.Latest)
                    {
                        return null;
                    }

                    return new ContentPermissionsPartIndex
                    {
                        ContentItemId = contentItem.ContentItemId,
                        ContentType = contentItem.ContentType,
                        Published = contentItem.Published,
                        PublishedUtc = contentItem.PublishedUtc,
                        Roles = String.Join(",", contentPermissionsPart.Roles), // Sqlite does not support string[] as a data type
                        Enabled = contentPermissionsPart.Enabled
                    };
                });
        }
    }
}

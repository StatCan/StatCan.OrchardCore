using System;
using OrchardCore.ContentManagement;
using YesSql.Indexes;
using StatCan.OrchardCore.Radar.Models;

namespace StatCan.OrchardCore.Radar.Indexes
{
    public class RadarFormPartIndex : MapIndex
    {
        public string DisplayText { get; set; }

        public string ContentItemId { get; set; }

        public string ContentType { get; set; }

        public bool Published { get; set; }

        public DateTime? PublishedUtc { get; set; }
    }

    public class RadarFormPartIndexProvider : IndexProvider<ContentItem>
    {
        public override void Describe(DescribeContext<ContentItem> context)
        {
            context.For<RadarFormPartIndex>()
                .Map(contentItem =>
                {
                    var radarFormPart = contentItem.As<RadarFormPart>();

                    // Ignore if does not use RadarFormPart
                    if (radarFormPart == null)
                    {
                        return null;
                    }

                    // Ignore previous versions of the content item
                    if (!contentItem.Latest)
                    {
                        return null;
                    }

                    return new RadarFormPartIndex
                    {
                        DisplayText = contentItem.DisplayText,
                        ContentItemId = contentItem.ContentItemId,
                        ContentType = contentItem.ContentType,
                        Published = contentItem.Published,
                        PublishedUtc = contentItem.PublishedUtc
                    };
                });
        }
    }
}

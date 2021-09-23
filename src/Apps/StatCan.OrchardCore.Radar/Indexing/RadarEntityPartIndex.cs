using System;
using OrchardCore.ContentManagement;
using YesSql.Indexes;
using StatCan.OrchardCore.Radar.Models;

/**
 * This index is needed for quering all radar entities. This query would provide possibily 
 * to add any radar entity and avoid hard-coding contenty types in the views.
 */
namespace StatCan.OrchardCore.Radar.Indexing
{
    public class RadarEntityPartIndex : MapIndex
    {
        public string ContentItemId { get; set; }

        public string ContentType { get; set; }

        public bool Published { get; set; }
    }

    public class RadarEntityPartIndexProvider : IndexProvider<ContentItem>
    {
        public override void Describe(DescribeContext<ContentItem> context)
        {
            context.For<RadarEntityPartIndex>()
                .Map(contentItem =>
                {
                    var radarEntityPart = contentItem.As<RadarEntityPart>();

                    // Ignore if not a radar entity
                    if (radarEntityPart == null)
                    {
                        return null;
                    }

                    // Ignore previous versions of the content item
                    if (!contentItem.Latest)
                    {
                        return null;
                    }

                    return new RadarEntityPartIndex
                    {
                        ContentItemId = contentItem.ContentItemId,
                        ContentType = contentItem.ContentType,
                        Published = contentItem.Published
                    };
                });
        }
    }
}

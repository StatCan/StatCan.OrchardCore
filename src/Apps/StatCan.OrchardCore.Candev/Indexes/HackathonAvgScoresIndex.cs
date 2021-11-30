using System;
using System.Linq;
using OrchardCore.ContentManagement;
using OrchardCore.Data;
using YesSql.Indexes;

namespace StatCan.OrchardCore.Candev.Indexes
{
    public class HackathonAvgScoresIndex : ReduceIndex
    {
        public string TeamContentItemId { get; set; }
        public double Score { get; set; }
    }
    public class HackathonAvgScoresIndexProvider : IndexProvider<ContentItem>
    {
        public override void Describe(DescribeContext<ContentItem> context)
        {
            context.For<HackathonAvgScoresIndex>()
                .Map(contentItem =>
                {
                    if (contentItem.ContentType != "Score")
                    {
                        return new HackathonAvgScoresIndex()
                        {
                            TeamContentItemId = ""
                        };
                    }

                    return new HackathonAvgScoresIndex()
                    {
                        TeamContentItemId = contentItem.Content.Score.Team.ContentItemIds.First,
                        Score = Convert.ToDouble(contentItem.Content.Score.Score.Value)
                    };
                })
                .Group(index => index.TeamContentItemId)
                .Reduce(group => new HackathonAvgScoresIndex()
                {
                    TeamContentItemId = group.Key,
                    Score = group.Average(x => x.Score)
                })
                .Delete((index, map) =>
                {
                    index.Score = map.Average(x => x.Score);
                    return index.Score > 0 ? index : null;
                });
        }
    }
}

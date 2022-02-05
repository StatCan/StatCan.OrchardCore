using System;
using System.Linq;
using OrchardCore.ContentManagement;
using OrchardCore.Data;
using YesSql.Indexes;

namespace StatCan.OrchardCore.Candev.Indexes
{
    public class HackathonAvgScoresIndex : ReduceIndex
    {
        public string ScoreIndexId { get; set; }
        public double Score { get; set; }
        public double Count { get; set; }
    }
    public class HackathonAvgScoresIndexProvider : IndexProvider<ContentItem>
    {
        public override void Describe(DescribeContext<ContentItem> context)
        {
            context.For<HackathonAvgScoresIndex>()
                .Map(contentItem =>
                {
                    if (contentItem.ContentType == "Score")
                    {
                        if (contentItem.Content.Score.Round.Value != 1)
                        {
                            return new HackathonAvgScoresIndex()
                            {
                                ScoreIndexId = contentItem.Content.Score.Team.ContentItemIds.First.ToString().Remove(25, 1) + contentItem.Content.JudgeType.Type.Values.First.ToString().Remove(1),
                                Score = contentItem.Content.Score.Score.Value,
                                Count = 1
                            };
                        }
                    }

                    return new HackathonAvgScoresIndex()
                    {
                        ScoreIndexId = "",
                    };
                })
                .Group(index => index.ScoreIndexId)           
                .Reduce(group => new HackathonAvgScoresIndex()
                {
                    ScoreIndexId = group.Key,
                    Score = group.Sum(x => x.Score),
                    Count = group.Sum(x => x.Count),
                })
                .Delete((index, map) =>
                {
                    index.Score -= map.Sum(x => x.Score);
                    index.Count -= map.Sum(x => x.Count);
                    return index.Score > 0 ? index : null;                 
                });
        }
    }
}

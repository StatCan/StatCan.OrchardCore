using System;
using System.Linq;
using OrchardCore.ContentManagement;
using OrchardCore.Data;
using YesSql.Indexes;

// namespace StatCan.OrchardCore.Candev.Indexes {
    namespace StatCan.OrchardCore.Hackathon.Indexes {
    

    public class HackathonAvgScoresIndex : ReduceIndex
    {
        public string ContentItemId { get; set; }
        public int? Score { get; set; }
        public int? Count { get; set; }
     }
        public class HackathonAvgScoresIndexProvider : IndexProvider<ContentItem>, IScopedIndexProvider {

        private readonly IServiceProvider _serviceProvider;

        public HackathonAvgScoresIndexProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public override void Describe(DescribeContext<ContentItem> context) {
            context.For<HackathonAvgScoresIndex, string>()
                .Map(contentItem => {
                    var indexValue = new HackathonAvgScoresIndex
                    {
                        ContentItemId = contentItem.ContentItemId,
                        Count = 1
                    };
                    if (contentItem.ContentType == "Score")
                    {
                        indexValue.Score = contentItem.Content.Score.Score.Value;
                        if (indexValue.Score == null) {
                            return null;
                        }
                    }
                    return indexValue;
                })

                .Group(x => x.Score)
                .Reduce( group => new HackathonAvgScoresIndex() {
                    Score = group.Key,
                    Count = group.Sum(x => x.Count)
                })

                .Delete((index, map) => {
                   index.Count -= map.Sum(x => x.Count);
                   return index.Count > 0 ? index : null;
               })
               
               
               ;
                
                
                




            }
        
        }

}
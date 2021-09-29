using OrchardCore.ContentManagement;
using YesSql.Indexes;
using System;

namespace StatCan.OrchardCore.Hackathon.Indexes {

    public class HackathonChallengeIndex : MapIndex
    {
        public string ChallengeName { get; set; }
        public string ChallengeShortDescription { get; set; }
        public string ChallengeMarkdownBody { get; set; }
        public string TeamName { get; set; }
        public string TeamDescription { get; set; }
        public string TeamContentItemId { get; set; }
         public string TeamMemberFirstName { get; set; }
         public string TeamMemberLastName { get; set; }
    }

        public class HackathonChallengeIndexProvider : IndexProvider<ContentItem>
    {
        private readonly IServiceProvider _serviceProvider;

        public HackathonChallengeIndexProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override void Describe(DescribeContext<ContentItem> context)
        {
            context.For<HackathonChallengeIndex>()
                .Map(contentItem =>
                {
                    var hackathonChallengeIndex = new HackathonChallengeIndex
                    {
                        // ChallengeName = contentItem.ChallengeName,
                        // ChallengeShortDescription = 
                        // ChallengeMarkdownBody = 
                    };



                    

                    return hackathonChallengeIndex;
                });
        }
    }

}
using System;
using OrchardCore.ContentManagement;
using OrchardCore.Data;
using YesSql.Indexes;

namespace StatCan.OrchardCore.Hackathon.Indexes {

    public class HackathonChallengesSolutionsIndex : MapIndex
    {
        public string ContentItemId { get; set; }
        public string ContentType { get; set; }
        public string TeamName { get; set; }
        public string TeamDescription { get; set; }
        public string TeamChallenge { get; set; }
        public string TeamChallengeName { get; set; }
        public string TeamChallengeShortDescription { get; set; }
        public string TeamChallengeMarkdown { get; set; }
        public string TeamCaptain { get; set; }
        public string TeamSolutionName{ get; set; }
        public string TeamSolutionDescription{ get; set; }
        public string TeamSolutionRepositoryUrl{ get; set; }
     }
        public class HackathonChallengesSolutionsIndexProvider : IndexProvider<ContentItem>, IScopedIndexProvider {

        private readonly IServiceProvider _serviceProvider;

        public HackathonChallengesSolutionsIndexProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public override void Describe(DescribeContext<ContentItem> context) {
            context.For<HackathonChallengesSolutionsIndex>()
                .Map(contentItem => {
                    
                    var indexValue = new HackathonChallengesSolutionsIndex {
                        ContentItemId = contentItem.ContentItemId,
                        ContentType = contentItem.ContentType,
                    };

                    if (contentItem.ContentType == "Team") {
                        indexValue.TeamName= contentItem.Content.Team.Name.Text;
                    }
                    if (contentItem.ContentType == "Team") {
                        indexValue.TeamDescription = contentItem.Content.Team.Description.Text;
                    }
                    if (contentItem.ContentType =="Team") {
                        indexValue.TeamChallenge = contentItem.Content.Team.Challenge?.ContentItemIds.First;
                    }
                    if (contentItem.ContentType =="Challenge") {
                        indexValue.TeamChallengeName = contentItem.Content.Challenge.Name.Text;
                    }
                    if (contentItem.ContentType =="Challenge") {
                        indexValue.TeamChallengeName = contentItem.Content.Challenge.Name.Text;
                    }
                    if (contentItem.ContentType =="Challenge") {
                        indexValue.TeamChallengeShortDescription = contentItem.Content.Challenge.ShortDescription.Text;
                    }
                    if (contentItem.ContentType =="Challenge") {
                        indexValue.TeamChallengeMarkdown = contentItem.Content.MarkdownBodyPart.Markdown;
                    }
                    if (contentItem.ContentType == "Team") {
                        indexValue.TeamCaptain = contentItem.Content.Team.TeamCaptain?.UserNames.First;
                    }
                    if (contentItem.ContentType == "Team") {
                        indexValue.TeamSolutionName = contentItem.Content.TeamSolutionPart.Name.Text;
                    }
                    if (contentItem.ContentType == "Team") {
                        indexValue.TeamSolutionDescription = contentItem.Content.TeamSolutionPart.Description.Text;
                    }
                    if (contentItem.ContentType == "Team") {
                        indexValue.TeamSolutionRepositoryUrl = contentItem.Content.TeamSolutionPart.RepositoryUrl.Text;
                    }

                    return indexValue;
                });

                


            }
        
        }

}
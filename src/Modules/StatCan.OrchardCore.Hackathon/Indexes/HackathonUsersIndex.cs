using OrchardCore.ContentManagement;
using OrchardCore.Users.Models;
using System;
using YesSql.Indexes;

namespace StatCan.OrchardCore.Hackathon.Indexes
{
    public class HackathonUsersIndex : MapIndex
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string ContactEmail { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Language { get; set; }
        public string TeamContentItemId { get; set; }
    }

    public class HackathonUsersIndexProvider : IndexProvider<User>
    {
        private readonly IServiceProvider _serviceProvider;

        public HackathonUsersIndexProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override void Describe(DescribeContext<User> context)
        {
            context.For<HackathonUsersIndex>()
                .Map(user =>
                {
                    var hackathonUsersIndex = new HackathonUsersIndex
                    {
                        UserId = user.UserId,
                        UserName = user.NormalizedUserName,
                        Email = user.NormalizedEmail
                    };

                    if (user.Properties.TryGetValue("ParticipantProfile", out var property))
                    {
                        var hacker = property.ToObject<ContentItem>();
                        hackathonUsersIndex.FirstName = hacker.Content.ParticipantProfile.FirstName.Text;
                        hackathonUsersIndex.LastName = hacker.Content.ParticipantProfile.LastName.Text;
                        hackathonUsersIndex.ContactEmail = hacker.Content.ParticipantProfile.Email.Text;
                        hackathonUsersIndex.Language = hacker.Content.ParticipantProfile.Language.Text;
                    }

                    if (user.Properties.TryGetValue("Hacker", out property))
                    {
                        var hacker = property.ToObject<ContentItem>();
                        if(hacker.Content.Hacker.Team.ContentItemIds.Count != 0)
                            hackathonUsersIndex.TeamContentItemId = hacker.Content.Hacker.Team.ContentItemIds[0];
                        else
                            hackathonUsersIndex.TeamContentItemId = string.Empty;
                    }

                    return hackathonUsersIndex;
                });
        }
    }
}

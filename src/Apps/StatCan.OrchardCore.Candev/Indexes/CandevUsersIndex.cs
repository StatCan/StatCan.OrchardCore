using OrchardCore.ContentManagement;
using OrchardCore.Users.Models;
using System;
using YesSql.Indexes;

namespace StatCan.OrchardCore.Candev.Indexes
{
    public class CandevUsersIndex : MapIndex
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string ContactEmail { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Language { get; set; }
        public string TeamContentItemId { get; set; }
        public string Roles { get; set; }
        public bool WillAttend { get; set; }
        public bool CheckIn { get; set; }
    }

    public class CandevUsersIndexProvider : IndexProvider<User>
    {
        private readonly IServiceProvider _serviceProvider;

        public CandevUsersIndexProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override void Describe(DescribeContext<User> context)
        {
            context.For<CandevUsersIndex>()
                .Map(user =>
                {
                    var hackathonUsersIndex = new CandevUsersIndex
                    {
                        UserId = user.UserId,
                        UserName = user.NormalizedUserName,
                        Email = user.NormalizedEmail,
                        Roles = string.Join(",", user.RoleNames)
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
                        hackathonUsersIndex.WillAttend = hacker.Content.Hacker.Attendance.Value;
                        hackathonUsersIndex.CheckIn = hacker.Content.Hacker.CheckIn.Value;
                    }

                    return hackathonUsersIndex;
                });
        }
    }
}
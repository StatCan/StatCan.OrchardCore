using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using OrchardCore.ContentManagement;
using OrchardCore.Users.Models;

namespace StatCan.OrchardCore.Hackathon
{
    public static class HackathonExtensions
    {
        public static bool In<T>(this T t, params T[] values)
        {
            return values.Contains(t);
        }
        public static bool HasTeam(this User user)
        {
            return !string.IsNullOrEmpty(user.GetTeamId());
        }
        public static string GetTeamId(this User user)
        {
            if (user.Properties.TryGetValue("Hacker", out var property))
            {
                var hacker = property.ToObject<ContentItem>();
                if (hacker.Content.Hacker.Team.ContentItemIds.Count != 0)
                    return hacker.Content.Hacker.Team.ContentItemIds[0];
            }

            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using OrchardCore.ContentManagement;

namespace StatCan.OrchardCore.Hackathon
{
    public static class HackathonExtensions
    {
        public static bool In<T>(this T t, params T[] values)
        {
            return values.Contains(t);
        }
        public static bool HasTeam(this ContentItem participant)
        {
            return !string.IsNullOrEmpty(participant.GetTeamId());
        }
        public static string GetTeamId(this ContentItem participant)
        {
            return ((JArray)participant.Content.Hacker?.Team?.ContentItemIds)?.FirstOrDefault()?.Value<string>();
        }
    }
}

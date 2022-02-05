using Microsoft.Extensions.Localization;
using OrchardCore.Environment.Commands;
using StatCan.OrchardCore.Candev.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatCan.OrchardCore.Candev.Commands
{
    public class CandevCommands : DefaultCommandHandler
    {
        private readonly ICandevService _candevService;

        public CandevCommands(
            ICandevService candevService,
            IStringLocalizer<CandevCommands> localizer) : base(localizer)
        {
            _candevService = candevService;
        }

        [OrchardSwitch]
        public string TeamName { get; set; }

        [OrchardSwitch]
        public string TeamTopics { get; set; }

        [OrchardSwitch]
        public string ChallengeTitle { get; set; }

        [OrchardSwitch]
        public string TopicName { get; set; }

        [OrchardSwitch]
        public string TopicChallenge { get; set; }

        [OrchardSwitch]
        public string TeamId { get; set; }

        [OrchardSwitch]
        public string UserName { get; set; }

        [CommandName("createTeam")]
        [CommandHelp("createTeam /TeamName:<teamName> /TeamTopics:{topicId,topicId,...}\r\n\t" + "Creates a new Team")]
        [OrchardSwitches("TeamName,TeamTopics")]
        public async Task<string> CreateTeamAsync()
        {
            var teamTopics = (TeamTopics ?? "").Split(',', StringSplitOptions.RemoveEmptyEntries).ToArray();

            var teamId = await _candevService.CreateTeam(TeamName, teamTopics);

            if (teamId != null)
            {
                Context.Output.WriteLine(S["Team created successfully"]);
                return teamId;
            }

            Context.Output.WriteLine(S["Error during CreateTeam Command"]);
            return null;
        }

        [CommandName("createChallenge")]
        [CommandHelp("createChallenge /ChallengeTitle:<challengeTitle>\r\n\t" + "Creates a new Challenge")]
        [OrchardSwitches("ChallengeTitle")]
        public async Task<string> CreateChallengeAsync()
        {
            var challengeId = await _candevService.CreateChallenge(ChallengeTitle);

            if (challengeId != null)
            {
                Context.Output.WriteLine(S["Challenge created successfully"]);
                return challengeId;
            }

            Context.Output.WriteLine(S["Error during CreateChallenge Command"]);
            return null;
        }

        [CommandName("createTopic")]
        [CommandHelp("createTopic /TopicName:<topicName> /TopicChallenge:<challengeId>\r\n\t" + "Creates a new Topic")]
        [OrchardSwitches("TopicName,TopicChallenge")]
        public async Task<string> CreateTopicAsync()
        {
            var topicId = await _candevService.CreateTopic(TopicName, TopicChallenge);

            if (topicId != null)
            {
                Context.Output.WriteLine(S["Topic created successfully"]);
                return topicId;
            }

            Context.Output.WriteLine(S["Error during CreateTopic Command"]);
            return null;
        }

        [CommandName("joinTeam")]
        [CommandHelp("joinTeam /TeamId:<teamId> /UserName:<username>\r\n\t" + "Makes a hacker join a Team")]
        [OrchardSwitches("TeamId,UserName")]
        public async Task JoinTeamAsync()
        {
            var team = await _candevService.JoinTeam(TeamId, UserName);

            if (team != null)
            {
                Context.Output.WriteLine(S["Topic created successfully"]);
            }

            Context.Output.WriteLine(S["Error during JoinTeam Command"]);
        }
    }
}

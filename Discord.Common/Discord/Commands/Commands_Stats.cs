using System.ComponentModel;
using Discord.Common.Discord.Extensions;
using DSharpPlus.Commands;
using DSharpPlus.Commands.ContextChecks;
using DSharpPlus.Commands.Processors.TextCommands;
using DSharpPlus.Commands.Trees.Metadata;
using Newtonsoft.Json;

namespace Discord.Common.Discord.Commands {
#pragma warning disable CA1822 // Mark members as static
    public class Commands_Stats {
        [Command("stats")]
        [Description("Get statistics of the bot's guilds.")]
        [RequireApplicationOwner]
        [AllowedProcessors(typeof(TextCommandProcessor))]
        public async Task StatsAsync(CommandContext ctx) {
            await ctx.Channel.TriggerTypingAsync();

            var guildStats = new List<List<string>>();
            foreach (var guild in ctx.Client.Guilds.OrderBy(x => x.Value.MemberCount).Reverse()) {
                var owner = await guild.Value.GetGuildOwnerAsync();
                guildStats.Add([$"{guild.Value.Name}({guild.Key})", $"{owner.Username}({guild.Value.OwnerId})", guild.Value.MemberCount.ToString()]);
            }

            var response = string.Join("\r\n", guildStats.Select(JsonConvert.SerializeObject));
            await ctx.SendFileAsync("stats.json", response);
        }
    }
#pragma warning restore CA1822 // Mark members as static
}
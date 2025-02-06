using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using Discord.Common.Discord.Extensions;
using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Commands.ContextChecks;
using DSharpPlus.Commands.Processors.TextCommands;
using DSharpPlus.Commands.Trees.Metadata;
using DSharpPlus.Net;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Discord.Common.Discord.Commands {
#pragma warning disable CA1822 // Mark members as static
    public class Commands_Admin {
        [Command("leaveguild")]
        [Description("Leave Guild.")]
        [RequireApplicationOwner]
        [AllowedProcessors(typeof(TextCommandProcessor))]
        public async Task LeaveGuildAsync(CommandContext ctx, ulong guildId) {
            await ctx.Channel.TriggerTypingAsync();

            try {
                var guild = await ctx.Client.GetGuildAsync(guildId, true);
                var guildJson = JsonConvert.SerializeObject(guild, Formatting.Indented);
                await ctx.SendFileAsync($"Guild_{guildId}.json", guildJson, $"Got Guild: {guildId}");

                await guild.LeaveAsync();
            }
            catch (Exception ex) {
                ctx.Client.Logger.LogWarning(ex, "Error leaving guild P1: {guildId}", guildId);
                await ctx.RespondAsync($"Unable to get guild, trying leave anyways");

                var field = typeof(BaseDiscordClient).GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                    .Single(x => x.Name.StartsWith("<ApiClient>"));

                DiscordApiClient apiClient = (DiscordApiClient)field.GetValue(ctx.Client);

                try {
                    await LeaveGuildAsync(apiClient, guildId);
                }
                catch (Exception ex2) {
                    ctx.Client.Logger.LogWarning(ex2, "Error leaving guild P2: {guildId}", guildId);
                    await ctx.RespondAsync($"Unable to leave guild: {guildId}");
                    return;
                }
            }

            await ctx.RespondAsync($"Left Guild: {guildId}");
        }

        [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "LeaveGuildAsync")]
        public static extern ValueTask LeaveGuildAsync(DiscordApiClient client, ulong guildId);
    }
#pragma warning restore CA1822 // Mark members as static
}
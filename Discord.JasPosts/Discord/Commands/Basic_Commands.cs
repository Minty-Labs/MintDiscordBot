using System.ComponentModel;
using Discord.JasPosts.Discord.Checks;
using Discord.JasPosts.Tasks.Jobs;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.Metadata;
using DSharpPlus.Commands.Trees.Metadata;
using DSharpPlus.Entities;

namespace Discord.JasPosts.Discord.Commands {
    public class Basic_Commands {
#pragma warning disable CA1822 // Mark members as static
        [Command("manualrun"), Description("Manually run the Twitter post fetcher"), CheckIfLilyOrJas]
        [InteractionAllowedContexts(DiscordInteractionContextType.Guild, DiscordInteractionContextType.PrivateChannel, DiscordInteractionContextType.BotDM)]
        [AllowedProcessors(typeof(SlashCommandProcessor))]
        public async Task ManualRun(SlashCommandContext ctx, 
            [Parameter("delayed"),
             Description("Whether to delay the post fetching")]
            bool delayed = false,
            [Parameter("force-last-tweet"),
             Description("Whether to force the post fetching")]
            bool forceLastTweet = false) {
            if (delayed) {
                await ctx.DeferResponseAsync(true);
                await Task.Delay(TimeSpan.FromSeconds(10));
            }
            
            var job = new GetTwitterPostsJob();
            await job.Run(forceLastTweet);

            var text = job.OutputAction switch {
                TwitterOutputActionEnum.Finished => "Successfully fetched the latest Twitter posts.",
                TwitterOutputActionEnum.FailedWithErrors => "Failed to fetch the latest Twitter posts.",
                TwitterOutputActionEnum.FinishedWithNoNewTweet => "Successfully fetched the latest Twitter posts, but there are no new tweets.",
                TwitterOutputActionEnum.ForcedLastTweet => "Successfully fetched the latest Twitter posts, and forced the last tweet.",
                _ => "Unknown output action."
            };

            await ctx.RespondAsync($"Manually ran the Twitter post fetcher.\n{text}", ephemeral: true);
        }
    }
}
#pragma warning restore CA1822 // Mark members as static
using DSharpPlus.Entities;
using Quartz;

namespace Discord.JasPosts.Tasks.Jobs {
    public class DiscordStatusJob : IJob {
        public async Task Execute(IJobExecutionContext context)
            => await Program.DiscordBot.Client.UpdateStatusAsync(new DiscordActivity("Jasmine's Social Media", DiscordActivityType.Watching));
    }
}
using DSharpPlus.Entities;

namespace Discord.JasPosts.Tasks.Jobs {
    public class DiscordStatusJob {
        public async Task Execute()
            => await Program.DiscordBot.Client.UpdateStatusAsync(new DiscordActivity("Jasmine's Social Media", DiscordActivityType.Watching));
    }
}
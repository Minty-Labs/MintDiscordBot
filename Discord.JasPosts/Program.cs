using Discord.JasPosts.Config;
using Discord.JasPosts.Discord;

namespace Discord.JasPosts;

internal class Program {
    internal static DiscordBot DiscordBot;

    public static async Task Start() {
        DiscordBot = new DiscordBot();
        DiscordConfig.Config.Load();
        BotConfig.Config.Load();
        await DiscordBot.Start();
        await Task.Delay(Timeout.Infinite);
    }
}
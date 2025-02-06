using Discord.HeadPat.Discord;

namespace Discord.HeadPat;

internal class Program {
    private static DiscordBot _discordBot;

    public static async Task Start() {
        _discordBot = new DiscordBot();
        await _discordBot.Start();
        await Task.Delay(-1);
    }
}
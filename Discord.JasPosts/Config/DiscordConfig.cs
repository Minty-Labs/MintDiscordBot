using Discord.Common.Configs;

namespace Discord.JasPosts.Config {
    public class DiscordConfig {
        public static Config<DiscordConfig> Config { get; } = new("JasminePosts.discord.config.json");

        public string DiscordToken { get; set; } = string.Empty;
    }
}
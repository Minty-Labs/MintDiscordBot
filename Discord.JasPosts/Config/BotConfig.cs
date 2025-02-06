using Discord.Common.Configs;

namespace Discord.JasPosts.Config {
    public class BotConfig {
        public static Config<BotConfig> Config { get; } = new("JasminePosts.bot.config.json");
        
        public string TargetingTwitterUser { get; set; } = "JasmineLovingVR";
        public string TwitterUsername { get; set; } = string.Empty;
        public string TwitterPassword { get; set; } = string.Empty;
        public string TwitterScriptOutputFile { get; set; } = string.Empty;
        public string TwitterScriptPreviousXIdFile { get; set; } = string.Empty;
        public ulong TwitterChannelId { get; set; } = 0;
        public ulong TwitterPostsPingRoleId { get; set; } = 0;
        
        public ulong MlLoggingChannelId { get; set; } = 0;
    }
}
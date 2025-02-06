namespace Discord.Common.Configs
{
    public class CommonBotConfig
    {
        public static Config<CommonBotConfig> Config { get; } = new("CommonBotConfig.json");

        public List<ulong> OwnerIDs { get; set; } = [];
        public List<ulong> BlacklistGuildIDs { get; set; } = [];
        public ulong ErrorDumpChannel { get; set; } = 0;
    }
}

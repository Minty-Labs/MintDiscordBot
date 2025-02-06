using DSharpPlus.Entities;

namespace Discord.Common.Discord.Extensions
{
    public static class DiscordMemberExtensions
    {
        public static bool GetIsTimedOut(this DiscordMember channel)
        {
            return channel.CommunicationDisabledUntil.HasValue && channel.CommunicationDisabledUntil > DateTimeOffset.UtcNow.AddSeconds(30);
        }
    }
}

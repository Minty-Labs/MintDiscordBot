using DSharpPlus.Entities;

namespace Discord.Common.Discord.Extensions
{
    public static class DiscordChannelExtensions
    {
        public static async Task<List<DiscordMessage>> GetAllMessages(this DiscordChannel channel)
        {
            List<DiscordMessage> messages = [];
            int limit = 100;
            int cnt = limit;

            var msgs = channel.GetMessagesAsync(limit);
            while (cnt == limit)
            {
                cnt = 0;

                await foreach (var msg in msgs)
                {
                    messages.Add(msg);
                    cnt++;
                }

                if (cnt == limit)
                {
                    msgs = channel.GetMessagesBeforeAsync(messages.Last().Id, limit);
                }
            }

            return messages;
        }
    }
}

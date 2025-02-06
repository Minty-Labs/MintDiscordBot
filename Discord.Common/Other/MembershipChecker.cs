using DSharpPlus;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Entities;
using Microsoft.Extensions.Logging;

namespace Discord.Common.Other
{
    public class MembershipCheckerCache
    {
        private readonly Dictionary<ulong, DateTime> userCache = [];
        private readonly TimeSpan cacheTime = new(1, 0, 0);

        public MembershipCheckerCache(TimeSpan? cacheTime = null)
        {
            if (cacheTime != null)
                this.cacheTime = cacheTime.Value;
        }

        private void RemoveExpired()
        {
            lock (userCache)
            {
                var expired = userCache.Where(x => x.Value < DateTime.UtcNow - cacheTime).Select(x => x.Key).ToList();
                foreach (var id in expired)
                    userCache.Remove(id);
            }
        }

        private bool IsMember(ulong id)
        {
            RemoveExpired();
            lock (userCache)
            {
                return userCache.TryGetValue(id, out var time) && time > DateTime.UtcNow - cacheTime;
            }
        }

        private void AddToCache(ulong id)
        {
            RemoveExpired();
            lock (userCache)
            {
                if (userCache.ContainsKey(id))
                    userCache[id] = DateTime.UtcNow;
                else
                    userCache.Add(id, DateTime.UtcNow);
            }
        }

        public async Task<Dictionary<ulong, DiscordGuild>?> IsMember(SlashCommandContext ctx)
        {
            if (!IsMember(ctx.User.Id))
            {
                Dictionary<ulong, DiscordGuild>? guilds = [];
                foreach (var guild in ctx.Client.Guilds)
                {
                    try
                    {
                        var member = await guild.Value.GetMemberAsync(ctx.User.Id);

                        guilds.Add(guild.Key, guild.Value);

                        if (member.IsPending == false)
                        {
                            AddToCache(ctx.User.Id);
                            guilds = null;
                            break;
                        }
                    }
                    catch (DSharpPlus.Exceptions.NotFoundException) { }
                    await Task.Delay(500);
                }

                if (guilds != null)
                    ctx.Client.Logger.LogInformation($"Membership Check failed: {ctx.User.Username} ({ctx.User.Id})");

                return guilds;
            }
            return null;
        }


        public async Task<Dictionary<ulong, DiscordGuild>?> IsMember(DiscordUser? user, DiscordClient client)
        {
            if (user != null && !IsMember(user.Id))
            {
                Dictionary<ulong, DiscordGuild>? guilds = [];
                foreach (var guild in client.Guilds)
                {
                    try
                    {
                        var member = await guild.Value.GetMemberAsync(user.Id, true);
                        guilds.Add(guild.Key, guild.Value);

                        if (member.IsPending == false)
                        {
                            AddToCache(user.Id);
                            guilds = null;
                            break;
                        }
                    }
                    catch (DSharpPlus.Exceptions.NotFoundException) { }
                }

                if (guilds != null)
                    client.Logger.LogInformation($"Membership Check failed: {user?.Username} ({user?.Id})");

                return guilds;
            }
            return null;
        }
    }
}

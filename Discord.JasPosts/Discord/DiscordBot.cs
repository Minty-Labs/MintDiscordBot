using Discord.Common.Discord;
using Discord.Common.Discord.CustomObjects;
using Discord.Common.Discord.Extensions;
using Discord.Common.Helpers;
using Discord.JasPosts.Config;
using Discord.JasPosts.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using Microsoft.Extensions.Logging;
using static System.DateTime;
using TimestampFormat = Discord.Common.Helpers.TimestampFormat;

namespace Discord.JasPosts.Discord {
    internal static class Vars {
        public const string Version = "1.0.1";
        public static bool IsWindows;
        // public static DateTime StartTime;
        
        public const string DSharpVersion = "5.0.0-nightly-02453";
    }
    
    public class DiscordBot : BotBase {
        private const string Prefix = "j.";
        
        public DiscordBot() : base(
            "Jasmine Posts",
            DiscordConfig.Config.Instance.DiscordToken,
            new CustomValueHolderExtension<AboutInfoItem> {
                Value = new AboutInfoItem(
                    new AboutInfoItem.InfoBase(new DiscordColor("C61219"), "Jasmine's Twitter/X auto status poaster", "https://discord.gg/Qg9eVB34sq",
                        "https://mintylabs.dev/JasPosts", "null", "https://mintylabs.dev/JasPosts/privacy", "https://mintylabs.dev/JasPosts"),
                    new AboutInfoItem.InviteUnavailable("This bot is only available for the Napping Grove."))
            },
            DiscordIntents.Guilds | DiscordIntents.GuildMessages | DiscordIntents.GuildMembers | DiscordIntents.MessageContents | DiscordIntents.AllUnprivileged,
            enableText: true,
            prefix: Prefix,
            enableSlash: false
        ) { }

        public new async Task Start() {
            Vars.IsWindows = Environment.OSVersion.ToString().Contains("windows", StringComparison.CurrentCultureIgnoreCase);
            Client.Logger.LogInformation("Starting bot...");
            await Scheduler.Initialize();
            await base.Start();
            await CommonHelper.RunAsync(DelayedTask);
        }

        private async Task DelayedTask() {
            await Task.Delay(1000);
            // Vars.StartTime = UtcNow;
            DiscordEmbedBuilder builder = new() {
                Color = new DiscordColor("C61219"),
                Footer = new DiscordEmbedBuilder.EmbedFooter {
                    Text = $"v{Vars.Version}",
                    IconUrl = Client.CurrentUser.GetAvatarUrl(ImageFormat.Png)
                },
                Timestamp = Now
            };
            builder.AddField("OS", Vars.IsWindows ? "Windows" : "Linux", true);
            // builder.AddField("Guilds", Client.Guilds.Count.ToString(), true);
            // builder.AddField("Users", Client.Guilds.Values.Sum(x => x.MemberCount).ToString(), true);
            builder.AddField("Start Time", $"{UtcNow.ConvertToDiscordTimestamp(TimestampFormat.LongDateTime)}\n{UtcNow.ConvertToDiscordTimestamp(TimestampFormat.RelativeTime)}");
            builder.AddField("Target .NET Version", "9.0.1", true);
            builder.AddField("System .NET Version", Environment.Version.ToString(), true);
            builder.AddField("DSharpPlus Version", Vars.DSharpVersion, true);

            if (BotConfig.Config.Instance.MlLoggingChannelId is not 0) {
                var channel = await Client.GetChannelAsync(BotConfig.Config.Instance.MlLoggingChannelId);
                await channel.SendMessageAsync(builder.Build());
            }

            if (Vars.IsWindows)
                Console.Title = "Jasmine Posts - Discord Bot - v" + Vars.Version;
            
            await Task.CompletedTask;
        }
    }
}
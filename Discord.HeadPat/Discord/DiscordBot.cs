using Discord.Common.Discord;
using Discord.Common.Discord.CustomObjects;
using Discord.Common.Discord.Extensions;
using DSharpPlus;
using DSharpPlus.Entities;
using Microsoft.Extensions.Logging;

namespace Discord.HeadPat.Discord {
    public class DiscordBot : BotBase {
        public DiscordBot() : base(
            "HeadPat",
            "TOKEN",
            new CustomValueHolderExtension<AboutInfoItem> {
                Value = new AboutInfoItem(
                    new AboutInfoItem.InfoBase(new DiscordColor(255, 255, 255), "About", "Home",
                        "Website", "ToS", "Privacy", "Commands"),
                    new AboutInfoItem.InviteAvailable(DiscordInvite: "https://discord.gg/invite", HasButtonInvite: true))
            },
            DiscordIntents.None,
            enableText: true,
            shardCount: 1,
            enableSlash: true
        ) { }

        public new async Task Start() {
            Client.Logger.LogInformation("Starting bot...");
            await Task.CompletedTask;
        }

        private async Task Loop() {
            Client.Logger.LogInformation("Looping...");
            await Task.CompletedTask;
        }
    }
}
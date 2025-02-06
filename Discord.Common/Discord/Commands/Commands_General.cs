using System.ComponentModel;
using System.Text;
using Discord.Common.Discord.CustomObjects;
using Discord.Common.Discord.Extensions;
using Discord.Common.Helpers;
using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.Metadata;
using DSharpPlus.Commands.Processors.TextCommands;
using DSharpPlus.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Discord.Common.Discord.Commands {
#pragma warning disable CA1822 // Mark members as static
    public class Commands_General {
        [Command("info")]
        [Description("Get information & invite links of the bot.")]
        [InteractionAllowedContexts(DiscordInteractionContextType.Guild, DiscordInteractionContextType.PrivateChannel, DiscordInteractionContextType.BotDM)]
        public async Task InfoAsync(CommandContext ctx) {
            if (ctx.CanRespondHere() == false) {
                return;
            }

            await ctx.RespondAsync(Internal_Commands_General.GetInfo(ctx.Client));
        }

        [Command("invite")]
        [Description("Get invite links & information of the bot.")]
        [InteractionAllowedContexts(DiscordInteractionContextType.Guild, DiscordInteractionContextType.PrivateChannel, DiscordInteractionContextType.BotDM)]
        public async Task InviteAsync(CommandContext ctx) {
            if (ctx.CanRespondHere() == false) {
                return;
            }

            await ctx.RespondAsync(Internal_Commands_General.GetInfo(ctx.Client));
        }

        [Command("ping")]
        [Description("Pings the bot and returns its ping in ms.")]
        [InteractionAllowedContexts(DiscordInteractionContextType.Guild, DiscordInteractionContextType.PrivateChannel, DiscordInteractionContextType.BotDM)]
        public async Task PingAsync(CommandContext ctx) {
            if (ctx.CanRespondHere() == false) {
                return;
            }

            bool canEdit = ctx.Channel.IsPrivate || ctx.Channel.PermissionsFor(ctx.Guild.CurrentMember).HasAllPermissions(DiscordPermission.ReadMessageHistory, DiscordPermission.ViewChannel);

            if (ctx is SlashCommandContext slashCommandContext) {
                await Internal_Commands_General.PingAsync(slashCommandContext, canEdit);
            }
            else if (ctx is TextCommandContext textCommandContext) {
                await Internal_Commands_General.PingAsync(textCommandContext, canEdit);
            }
        }
    }
#pragma warning restore CA1822 // Mark members as static

    internal class Internal_Commands_General {
        public static async Task PingAsync(SlashCommandContext ctx, bool canEdit) {
            var now = DateTimeOffset.UtcNow;
            var commandLat = (now - ctx.Interaction.CreationTimestamp).TotalMilliseconds;

            var builder = Internal_Commands_General.GetBasePingEmbedBuilder(ctx.Client, commandLat, ctx.Guild);

            if (!canEdit) {
                builder.AddField("→ Interaction Latency ←", "```cs\n" + "N/A".PadLeft(10, '⠀') + "```", true);
                await ctx.RespondAsync(builder.Build());
                return;
            }

            builder.AddField("→ Interaction Latency ←", "```cs\n" + "Fetching..".PadLeft(15, '⠀') + "```", true);

            var builtEmbed = builder.Build();
            var now2 = DateTimeOffset.UtcNow;

            await ctx.RespondAsync(builtEmbed);
            var message = await ctx.Interaction.GetOriginalResponseAsync();

            var msgLat = (message.CreationTimestamp - now2).TotalMilliseconds;

            builder.RemoveFieldAt(builder.Fields.Count - 1);
            builder.AddField("→ Interaction Latency ←", "```cs\n" + $"{msgLat:N0} ms".PadLeft(10, '⠀') + "```", true);
            await message.ModifyAsync(builder.Build());
        }

        public static async Task PingAsync(TextCommandContext ctx, bool canEdit) {
            var now = DateTimeOffset.UtcNow;
            var commandLat = (now - (ctx.Message.EditedTimestamp != null ? ctx.Message.EditedTimestamp.Value : ctx.Message.Timestamp)).TotalMilliseconds;

            var builder = Internal_Commands_General.GetBasePingEmbedBuilder(ctx.Client, commandLat, ctx.Guild);

            if (!canEdit) {
                builder.AddField("→ Message Latency ←", "```cs\n" + "N/A".PadLeft(10, '⠀') + "```", true);
                await ctx.RespondAsync(builder.Build());
                return;
            }

            builder.AddField("→ Message Latency ←", "```cs\n" + "Fetching..".PadLeft(15, '⠀') + "```", true);

            var builtEmbed = builder.Build();
            var now2 = DateTimeOffset.UtcNow;

            await ctx.RespondAsync(builtEmbed);
            var message = await ctx.GetResponseAsync();

            var msgLat = (message!.CreationTimestamp - now2).TotalMilliseconds;

            builder.RemoveFieldAt(builder.Fields.Count - 1);
            builder.AddField("→ Message Latency ←", "```cs\n" + $"{msgLat:N0} ms".PadLeft(10, '⠀') + "```", true);
            await message.ModifyAsync(builder.Build());
        }

        public static DiscordEmbedBuilder GetBasePingEmbedBuilder(DiscordClient discordClient, double commandLat, DiscordGuild discordGuild) {
            DiscordEmbedBuilder builder = new() {
                Color = DiscordColor.Blurple
            };

            builder.AddField("→ Websocket Latency ←", "```cs\n" + $"{discordClient.GetConnectionLatency(discordGuild?.Id ?? 0).TotalMilliseconds:N0} ms".PadLeft(10, '⠀') + "```", true);
            builder.AddField("→ Command Latency ←", "```cs\n" + $"{commandLat:N0} ms".PadLeft(11, '⠀') + "```", true);
            return builder;
        }

        public static DiscordEmbed GetInfo(DiscordClient discordClient) {
            var aboutInfo = discordClient.ServiceProvider.GetRequiredService<CustomValueHolderExtension<AboutInfoItem>>().Value;

            DiscordEmbedBuilder builder = new() {
                Color = aboutInfo.Info.MainColor
            };

            builder.WithTitle(MarkdownUtils.ToUnderline($"About {discordClient.CurrentApplication.Name}"));

            if (aboutInfo.Invite is AboutInfoItem.InviteAvailable inviteAvailable) {
                if (!string.IsNullOrEmpty(inviteAvailable.DiscordInvite)) {
                    builder.AddFieldSafe("Invite Link", inviteAvailable.DiscordInvite);
                }

                if (!string.IsNullOrEmpty(inviteAvailable.WebsiteInvite)) {
                    builder.AddFieldSafe("Short Invite Link", inviteAvailable.WebsiteInvite);
                }

                if (inviteAvailable.HasButtonInvite) {
                    builder.AddFieldSafe("Invite me via Discord", "You can invite me by clicking on my profile then clicking on `Add App`");
                }
            }
            else if (aboutInfo.Invite is AboutInfoItem.InviteUnavailable inviteUnavailable) {
                builder.AddFieldSafe("Can not invite this bot", inviteUnavailable.UnavailableString);
            }

            var sb = new StringBuilder();
            if (aboutInfo.Info.CommandsUrl != "null")
                sb.Append(MarkdownUtils.MakeLink("Commands", aboutInfo.Info.CommandsUrl));

            if (aboutInfo.Info.TermsOfServiceUrl != "null")
                sb.Append(MarkdownUtils.MakeLink("Terms of Service", aboutInfo.Info.TermsOfServiceUrl));

            if (aboutInfo.Info.PrivacyPolicyUrl != "null")
                sb.Append(MarkdownUtils.MakeLink("Privacy Policy", aboutInfo.Info.PrivacyPolicyUrl));

            // insert separator where one is null and the other is not
            if (aboutInfo.Info.TermsOfServiceUrl != "null" ^ aboutInfo.Info.PrivacyPolicyUrl != "null")
                sb.Append(" • ");

            if (!string.IsNullOrWhiteSpace(sb.ToString())) {
                builder.AddFieldSafe("More", sb.ToString());
            }

            builder.AddFieldSafe("Support", $"[Website]({aboutInfo.Info.HomeWebsite}) • [Discord]({aboutInfo.Info.HomeDiscord})");

            //builder.WithFooter("Made by the RubyEdge Team + Minty Labs", "https://files.mili.lgbt/MintyLabs.webp");
            builder.WithFooter("Made by MintLily", "https://mintlily.lgbt/assets/img/Lily.png");

            return builder.Build();
        }
    }
}
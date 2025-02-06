using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.TextCommands;
using DSharpPlus.Entities;
using Microsoft.Extensions.Logging;

namespace Discord.Common.Helpers {
    public static class CommonHelper {
        public static bool CanRespondHere(this CommandContext context) {
            if (context is TextCommandContext textCommandContext) {
                return textCommandContext.Channel.IsPrivate ||
                       (!textCommandContext.Channel.IsPrivate && textCommandContext.Channel.IsThread && textCommandContext.Channel.PermissionsFor(textCommandContext.Guild.CurrentMember).HasPermission(DiscordPermission.SendThreadMessages)) ||
                       (!textCommandContext.Channel.IsPrivate && !textCommandContext.Channel.IsThread && textCommandContext.Channel.PermissionsFor(textCommandContext.Guild.CurrentMember).HasPermission(DiscordPermission.SendMessages));
            }

            return true;
        }

        public static async Task SlashCommandWithFallback(SlashCommandContext ctx, Func<Task<DiscordMessageBuilder>> task) {
            bool deferred = true;
            try {
                await ctx.DeferResponseAsync();
            }
            catch (DSharpPlus.Exceptions.DiscordException discordException) when (discordException is DSharpPlus.Exceptions.NotFoundException || discordException is DSharpPlus.Exceptions.ServerErrorException) {
                await ctx.Interaction.Channel.SendMessageAsync(ctx.Interaction.User?.Mention + " Discord Interaction failed, deploying jank fallback system");
                deferred = false;
            }

            DiscordMessageBuilder builder = await task();

            if (deferred) {
                var response = new DiscordWebhookBuilder().WithContent(builder.Content).AddComponents(builder.Components).AddEmbeds(builder.Embeds);

                foreach (var file in builder.Files) {
                    response.AddFile(file.FileName, file.Stream);
                }

                await ctx.EditResponseAsync(response);

                foreach (var file in builder.Files) {
                    file.Stream.Dispose();
                }
            }
            else {
                await ctx.Interaction.Channel.SendMessageAsync(builder);
            }
        }

        public static Task RunAsync(Func<Task?> function, CancellationToken cancellationToken = default) {
            _ = Task.Run(async () => {
                try {
                    await function();
                }
                catch (Exception ex) {
                    LoggerHelper.GlobalLogger.LogError("[Runner] Failure run \n{ex}", ex.ToString());
                }
            }, cancellationToken);
            return Task.CompletedTask;
        }

        public static string NewLine(List<string> init) {
            string output = "";
            int i = 0;
            foreach (string text in init) {
                i++;
                output += text;
                if (i != init.Count) {
                    output += "\n";
                }
            }

            return TrimString(output);
        }

        public static string FormatNumber(int number) {
            return string.Format("{0:n0}", number);
        }

        public static string TrimString(string item, int cutAt = 1024) {
            if (item.Length > cutAt) {
                item = item[..(cutAt - 4)] + "...";
            }

            return item;
        }

        public static Optional<T> FromNullableValue<T>(T value) {
            if (value == null)
                return Optional.FromNoValue<T>();
            else
                return Optional.FromValue(value);
        }

        public static string FormatFileSize(long filesize) {
            if (filesize < 0L) {
                return string.Empty;
            }

            long num = 1024L;
            long num2 = num * 1024L;
            long num3 = num2 * 1024L;
            long num4 = num3 * 1024L;
            long num5;
            string str;
            if (filesize > num4) {
                num5 = num4;
                str = "TB";
            }
            else if (filesize > num3) {
                num5 = num3;
                str = "GB";
            }
            else if (filesize > num2) {
                num5 = num2;
                str = "MB";
            }
            else if (filesize > num) {
                num5 = num;
                str = "KB";
            }
            else {
                num5 = 1L;
                str = "B";
            }

            return (filesize / (float)num5).ToString("F2") + " " + str;
        }

        public static DiscordEmbedBuilder GetDefaultEmbed() {
            DiscordEmbedBuilder builder = new();
            builder.WithFooter("Made by loukylor and DubyaDude, edits by MintLily");
            builder.WithTimestamp(DateTime.Now);
            return builder;
        }

        public static DiscordEmbedBuilder AddFieldSafe(this DiscordEmbedBuilder builder, string key, string value, bool inline = false) {
            builder.AddField(string.IsNullOrWhiteSpace(key) ? "\\NULL\\" : key, string.IsNullOrWhiteSpace(value) ? "\\NULL\\" : value, inline);
            return builder;
        }
    }
}
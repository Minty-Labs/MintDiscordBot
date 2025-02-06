using DSharpPlus.Commands;
using DSharpPlus.Entities;

namespace Discord.Common.Discord.Extensions
{
    public static class CommandContextExtensions
    {
        public static async Task SendFileAsync(this CommandContext? ctx, string fileName, string fileContents, string message = "")
        {
            if (ctx == null)
                return;

            DiscordMessageBuilder builder = new();
            builder.WithContent(message);
            using MemoryStream stream = new();
            using StreamWriter writer = new(stream);

            await writer.WriteAsync(fileContents);
            await writer.FlushAsync();
            stream.Seek(0, SeekOrigin.Begin);
            builder.AddFile(fileName, stream);
            await ctx.RespondAsync(builder);
        }
    }
}

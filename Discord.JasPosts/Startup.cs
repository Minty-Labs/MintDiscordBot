using Discord.Common.Discord;

namespace Discord.JasPosts;
internal static class Startup {
    private static async Task Main() {
        MobilePatch.CreateMobilePatch();
        await Program.Start();
    }
}
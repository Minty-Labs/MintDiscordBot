using System.Reflection;
using Discord.Common.Helpers;
using DSharpPlus.Entities;
using HarmonyLib;
using Microsoft.Extensions.Logging;

namespace Discord.Common.Discord {
    public static class MobilePatch {
        private static MethodInfo patch;

        public static void CreateMobilePatch() {
            if (patch == null) {
                try {
                    LoggerHelper.GlobalLogger.LogInformation("Jankingly Loading DSharpPlus: " + DiscordChannelType.Text);
                    var harmony = new Harmony("mobilePatch");
                    var mOriginal = AppDomain.CurrentDomain.GetAssemblies().Single(assembly => assembly.GetName().Name == "DSharpPlus").GetTypes().Single(x => x.Name == "ClientProperties").GetProperty("Browser").GetGetMethod();
                    var mPostfix = new HarmonyMethod(AccessTools.Method(typeof(MobilePatch), nameof(PatchMethod)));
                    patch = harmony.Patch(mOriginal, postfix: mPostfix);
                }
                catch (Exception e) {
                    LoggerHelper.GlobalLogger.LogError("Mobile Patch Failed\n" + e);
                }
            }
            else {
                LoggerHelper.GlobalLogger.LogInformation("Mobile Patch already done");
            }
        }

        public static void PatchMethod(ref string __result)
            => __result = "Discord iOS";
    }
}
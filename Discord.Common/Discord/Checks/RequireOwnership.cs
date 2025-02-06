using Discord.Common.Configs;
using DSharpPlus.Commands;
using DSharpPlus.Commands.ContextChecks;

namespace PublicBot.DiscordBot.Checks
{
    public class RequireOwnership : ContextCheckAttribute;

    public class RequireOwnershipCheck : IContextCheck<RequireOwnership>
    {
        public ValueTask<string?> ExecuteCheckAsync(RequireOwnership attribute, CommandContext context)
        {
            string? returnValue = null;

            if (!CommonBotConfig.Config.Instance.OwnerIDs.Contains(context.User.Id))
            {
                returnValue = "You do not have permission to run this command.";
            }

            return ValueTask.FromResult(returnValue);
        }
    }
}

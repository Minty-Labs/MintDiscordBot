using DSharpPlus.Commands;
using DSharpPlus.Commands.ContextChecks;

namespace Discord.JasPosts.Discord.Checks;

public class CheckIfLilyOrJasCheck : IContextCheck<CheckIfLilyOrJasAttribute> {
    public ValueTask<string?> ExecuteCheckAsync(CheckIfLilyOrJasAttribute attribute, CommandContext context)
        => ValueTask.FromResult(context.User.Id is 167335587488071682 or 599706352620929037 ? null :
            "You do not have permission to run this command." );
}
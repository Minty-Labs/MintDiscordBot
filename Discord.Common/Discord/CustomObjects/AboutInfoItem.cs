using DSharpPlus.Entities;

namespace Discord.Common.Discord.CustomObjects
{
    public class AboutInfoItem
    {
        public InfoBase Info { get; set; }
        public InviteBase Invite { get; set; }

        public AboutInfoItem(InfoBase info, InviteBase invite)
        {
            Info = info;
            Invite = invite;
        }

        public record InfoBase(DiscordColor MainColor, string About, string HomeDiscord, string HomeWebsite, string TermsOfServiceUrl, string PrivacyPolicyUrl, string CommandsUrl);

        public record InviteAvailable(string WebsiteInvite = null, string DiscordInvite = null, bool HasButtonInvite = true) : InviteBase();
        public record InviteUnavailable(string UnavailableString) : InviteBase();
        public abstract record InviteBase();
    }
}

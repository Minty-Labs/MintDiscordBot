using System.Reflection;
using Discord.Common.Configs;
using Discord.Common.Discord.CustomObjects;
using Discord.Common.Discord.Extensions;
using Discord.Common.Helpers;
using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Commands.EventArgs;
using DSharpPlus.Commands.Exceptions;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.TextCommands;
using DSharpPlus.Commands.Processors.TextCommands.Parsing;
using DSharpPlus.Commands.Processors.UserCommands;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Discord.Common.Discord;

/// <summary>
/// Represents an abstraction of a high-level bot functions.
/// </summary>
public class BotBase {
    private readonly bool _enableText,
        _enableSlash,
        _enableUserCommands;

    private readonly string? _prefix;


    /// <summary>
    /// Gets the associated Discord client for this bot.
    /// </summary>
    /// <remarks>
    /// If ShardCount is greater than 1, this make a sharded client, otherwise it will make a normal client.
    /// </remarks>
    public DiscordClient Client { get; private set; }

    public BotBase
    (
        string name,
        string token,
        CustomValueHolderExtension<AboutInfoItem> aboutExtension,
        DiscordIntents discordIntents = DiscordIntents.AllUnprivileged,
        string? prefix = null,
        bool enableText = false,
        bool enableSlash = true,
        bool enableUserCommands = false,
        uint? shardCount = 1,
        Action<DiscordConfiguration>? customConfigure = null,
        Action<EventHandlingBuilder>? customEvents = null
    ) {
        DiscordClientBuilder builder;

        if (shardCount is null || shardCount == 1) {
            builder = DiscordClientBuilder.CreateDefault(token, discordIntents);
        }
        else {
            builder = DiscordClientBuilder.CreateSharded(token, discordIntents, shardCount.Value);
        }

        var clientBuilder = builder
            .ConfigureLogging(loggingBuilder => loggingBuilder.AddCommonSerilog(name))
            .ConfigureExtraFeatures(extrasConfiguration => {
                extrasConfiguration.SlidingMessageCacheExpiration = TimeSpan.FromMinutes(10);
                extrasConfiguration.AbsoluteMessageCacheExpiration = TimeSpan.FromMinutes(50);
                extrasConfiguration.LogUnknownEvents = false;
                extrasConfiguration.LogUnknownAuditlogs = false;
                customConfigure?.Invoke(extrasConfiguration);
            })
            .ConfigureEventHandlers(eventHandlingBuilder => {
                customEvents?.Invoke(eventHandlingBuilder);
                eventHandlingBuilder.HandleSessionCreated(async (sender, args) => await OnSessionCreated(sender, args));
                eventHandlingBuilder.HandleMessageCreated(async (sender, args) => await OnMessageCreated(sender, args));
                eventHandlingBuilder.HandleGuildCreated(async (sender, args) => await OnGuildCreated(sender, args));
                eventHandlingBuilder.HandleGuildDeleted(async (sender, args) => await OnGuildDeleted(sender, args));
            })
            .ConfigureServices
            (
                services => {
                    services.AddSingleton(aboutExtension);

                    services.AddInteractivityExtension
                    (
                        new InteractivityConfiguration {
                            PaginationBehaviour = PaginationBehaviour.Ignore,
                            Timeout = TimeSpan.FromMinutes(2)
                        }
                    );

                    services.AddCommandsExtension(ConfigureCommands, new CommandsConfiguration { UseDefaultCommandErrorHandler = false });
                }
            );

        Client = clientBuilder.Build();

        _enableText = enableText;
        _enableSlash = enableSlash;
        _enableUserCommands = enableUserCommands;
        _prefix = prefix;

        if (_enableText) {
            ArgumentException.ThrowIfNullOrWhiteSpace(prefix, nameof(prefix));
        }
    }

    public async Task Start() {
        await Client.InitializeAsync();

        Client.Logger.LogDebug("Command configuration complete. Registering events...");

        await Client.ConnectAsync();
    }

    private void ConfigureCommands(IServiceProvider serviceProvider, CommandsExtension extension) {
        var assemblies = new Assembly[] {
            Assembly.GetExecutingAssembly(),
            Assembly.GetEntryAssembly()
        };


        LoggerHelper.GlobalLogger.LogInformation("Attempting to configure commands");

        if (_enableText) {
            var textCommands = new TextCommandProcessor() {
                Configuration = new TextCommandConfiguration {
                    PrefixResolver = new DefaultPrefixResolver(true, _prefix!).ResolvePrefixAsync,
                },
            };

            foreach (var assembly in assemblies) {
                textCommands.AddConverters(assembly);
            }

            extension.AddProcessor(textCommands);
        }

        if (_enableSlash) {
            var slashCommands = new SlashCommandProcessor();

            foreach (var assembly in assemblies) {
                slashCommands.AddConverters(assembly);
            }

            extension.AddProcessor(slashCommands);
        }

        if (_enableUserCommands) {
            var userCommands = new UserCommandProcessor();

            extension.AddProcessor(userCommands);
        }

        LoggerHelper.GlobalLogger.LogDebug("Registering command hooks...");

        foreach (var assembly in assemblies) {
            extension.AddCommands(assembly);
            extension.AddChecks(assembly);
            extension.AddParameterChecks(assembly);
        }

        extension.CommandErrored += OnCommandErrored;

        extension.CommandExecuted += (sender, e) => {
            sender.Client.Logger.LogDebug
            (
                "{User} executed {Command} in {GuildNameOrDM}",
                e.Context.User.Username,
                e.Context.Command.Name,
                e.Context.Guild?.Name ?? "DM"
            );

            return Task.CompletedTask;
        };
    }

    private static Task OnSessionCreated(DiscordClient sender, SessionCreatedEventArgs _) {
        LoggerHelper.GlobalLogger.LogInformation("Session created for bot ({GuildCount} guilds) ({UserInstallCount} users)", sender.Guilds.Count, sender.CurrentApplication.ApproximateUserInstallCount);
        return Task.CompletedTask;
    }

    private static async Task OnGuildDeleted(DiscordClient sender, GuildDeletedEventArgs args) {
        var currentApplication = sender.CurrentApplication;

        if (args.Guild.Name != null) {
            try {
                currentApplication = await sender.GetCurrentApplicationAsync();
            }
            catch (Exception ex) {
                sender.Logger.LogWarning("Failed to get current application: {Exception}", ex);
            }
        }

        sender.Logger.LogInformation
        (
            "[GUILD_DEL] Left guild {GuildName} ({GuildID}) | Bot now has {GuildCount} guild(s) | Installed by {UserInstallCount} user(s)",
            args.Guild.Name,
            args.Guild.Id,
            sender.Guilds.Count,
            currentApplication.ApproximateUserInstallCount
        );
    }

    private static async Task OnGuildCreated(DiscordClient sender, GuildCreatedEventArgs args) {
        var currentApplication = sender.CurrentApplication;

        try {
            currentApplication = await sender.GetCurrentApplicationAsync();
        }
        catch (Exception ex) {
            sender.Logger.LogWarning("Failed to get current application: {Exception}", ex);
        }

        sender.Logger.LogInformation
        (
            "[GUILD_ADD] Joined guild {GuildName} ({GuildID}) | Bot now has {GuildCount} guild(s) | Installed by {UserInstallCount} user(s)",
            args.Guild.Name,
            args.Guild.Id,
            sender.Guilds.Count,
            currentApplication.ApproximateUserInstallCount
        );
    }

    private static async Task OnMessageCreated(DiscordClient sender, MessageCreatedEventArgs args) {
        if (args.Guild is null || !CommonBotConfig.Config.Instance.BlacklistGuildIDs.Contains(args.Guild.Id)) {
            return;
        }

        sender.Logger.LogWarning
        (
            "[BLACKLIST] Detection of blacklisted guild {GuildId} ({GuildName})\n{Author}\n{Message}",
            args.Guild.Id,
            args.Guild.Name,
            args.Author,
            args.Message
        );

        // Debatably cheaper, but more importantly, it's easier to read.
        // We love .NET 8 APIs <3
        await args.Guild.LeaveAsync().ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing);
    }

    private static async Task OnCommandErrored(CommandsExtension sender, CommandErroredEventArgs e) {
        // DSP.Commands doesn't have a command module type.
        if (e.Exception is ChecksFailedException or CommandNotFoundException or ArgumentParseException) {
            return;
        }

        // TODO? Waiting on input about whether this object is the command object itself, but it seems like it.
        if (e.CommandObject != null) {
            await e.Context.RespondAsync(await ErrorHelper.OopsieDoopsyIMwadeAMwistakeOwO(sender.Client, $"Error during command {e.Context.Command.FullName}", e.Exception));
        }
        else if (e.Context is TextCommandContext tcc) {
            await e.Context.RespondAsync(await ErrorHelper.OopsieDoopsyIMwadeAMwistakeOwO(sender.Client, $"Error while processing input {tcc.Message.Content}", e.Exception));
        }
    }
}
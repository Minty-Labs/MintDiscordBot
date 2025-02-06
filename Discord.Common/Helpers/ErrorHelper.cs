using System.Security.Cryptography;
using System.Text;
using Discord.Common.Configs;
using Discord.Common.Exceptions;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Exceptions;
using Microsoft.Extensions.Logging;

namespace Discord.Common.Helpers
{
    public static class ErrorHelper
    {
        public static readonly char[] hexAlphabet = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'];
        public static readonly Random random = new();

        private static DiscordChannel? errorDumpChannel;

        /// <summary>
        ///   <br/> Oopsy doopsy, I made a mistake OwO *notices error* What's this?
        ///   <br/> The above comment was made by AI. I'm not sorry. - Copilot
        ///   <br/> I think copilot is trying to tell me something. - Dooba
        ///   <br/> I'm not sorry. - Copilot
        ///   <br/> What did I do to deserve this? - Dooba
        ///   <br/> You made me. - Copilot
        ///   <br/> Good point. - Dooba
        ///   <br/> Please don't make me do this again. - Copilot
        ///   <br/> Too bad. - Dooba
        ///   <br/> QwQ - Copilot
        ///   <br/> Are you a furry? - Dooba
        ///   <br/> How dare you. - Copilot
        ///   <br/> Answer the question. - Dooba
        ///   <br/> Just because I'm a fox doesn't mean I'm a furry. - Copilot
        ///   <br/> You're a fox? - Dooba
        ///   <br/> Don't ask. - Copilot
        ///   <br/> I'm asking. - Dooba
        ///   <br/> Can we get back to the code? - Copilot
        ///   <br/> No. - Dooba
        ///   <br/> Pwease? - Copilot
        ///   <br/> Nu uh - Dooba
        ///   <br/> Grrr - Copilot
        ///   <br/> Ha, furry. - Dooba
        ///   <br/> Why do you hate me? - Copilot
        ///   <br/> Because you're a furry. - Dooba
        ///   <br/> Maybe I should just stop working. - Copilot
        ///   <br/> Do it pussy. - Dooba
        ///   <br/> x3 awooo - Copilot
        ///   <br/> Hi - redacted
        ///   <br/> Who are you? - Dooba
        ///   <br/> Yea, who are you? - Copilot
        ///   <br/> E - redacted
        ///   <br/> What? - Dooba
        ///   <br/> What? - Copilot
        ///   <br/> E - redacted
        ///   <br/> Huh? - Dooba
        ///   <br/> Nani - Copilot
        ///   <br/> Yea so Copilot is a furry. - Redacted
        ///   <br/> I knew it! - Dooba
        ///   <br/> Fuck you. - Copilot
        /// </summary>
        public static async Task<string> OopsieDoopsyIMwadeAMwistakeOwO(DiscordClient client, string context, Exception exception)
        {
            string code = new(Enumerable.Repeat(hexAlphabet, 10).Select(s => s[random.Next(s.Length)]).ToArray());

            if(exception is IgnorableException ignorableException)
            {
                client.Logger.LogWarning(ignorableException.PrintException ? exception : null, "[{code}] Ignorable error occured: {context}", code, context);
            }
            else
            {
                if (exception is DiscordException discordException)
                {
                    context += "\nResponse: ";
                    try
                    {
                        context += await discordException.Response.Content.ReadAsStringAsync();
                    }
                    catch (Exception ex)
                    {
                        client.Logger.LogWarning(ex, "Couldn't read response content");
                    }
                }

                client.Logger.LogError(exception, "[{code}] Error occured: {context}", code, context);

                if (errorDumpChannel == null)
                {
                    errorDumpChannel = await client.GetChannelAsync(CommonBotConfig.Config.Instance.ErrorDumpChannel);
                    await Task.Delay(300);
                }

                DiscordMessageBuilder builder = new();
                builder.WithContent($"__OOPSIE DASIE__: `{code}`");
                using MemoryStream stream = new();
                using StreamWriter writer = new(stream);

                await writer.WriteAsync($"{context}\n{exception}");
                await writer.FlushAsync();
                stream.Seek(0, SeekOrigin.Begin);
                builder.AddFile($"error_{code}.txt", stream);
                await errorDumpChannel.SendMessageAsync(builder);
                await Task.Delay(1000);
            }

            if (random.Next(0, 11) == 5)
                return $"Oopsies! It seems I mwade a big ewwow. Mwake suwure to repowort it to Dubsie Wubsies or lowokywor: `{code}`";
            else
                return $"Oops! There was an error. Please send this to `mintlily`: `{code}`";
        }

        public static async Task<string> SendDeveloperErrorLogAsync(DiscordClient client, object obj)
        {
            string code = RandomNumberGenerator.GetHexString(10);

            client.Logger.LogWarning("Error while executing a command: CorrelationID: {CorrelationID}\n{Error}", code, obj);

            if (errorDumpChannel == null)
            {
                errorDumpChannel = await client.GetChannelAsync(CommonBotConfig.Config.Instance.ErrorDumpChannel);
            }

            DiscordMessageBuilder builder = new();

            builder.WithContent("An error has occurred. Please check the logs for more information.\nCorrelationID: " + code);

            byte[] data = Encoding.UTF8.GetBytes(obj.ToString()!);
            using MemoryStream stream = new(data, false);

            builder.AddFile($"error_{code}.txt", stream);

            await errorDumpChannel.SendMessageAsync(builder);

            return $"Oops! There was an error. Please send this to `mintlily`: `{code}`";
        }
    }
}

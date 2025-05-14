using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Discord.Common.Helpers;
using Discord.JasPosts.Config;
using Discord.JasPosts.Discord;
using Microsoft.Extensions.Logging;

namespace Discord.JasPosts.Tasks.Jobs {

        public int outputAction;
        // 1 - finished
        // 2 - failed with errors
        // 3 - finished, no new tweet
        // 4 - forced last tweet finished

        public async Task Run(bool forceLastTweet = false) {
            Program.DiscordBot.Client.Logger.LogInformation("Running GetTwitterPostsJob...");

            if (forceLastTweet) {
                var data = await File.ReadAllLinesAsync(BotConfig.Config.Instance.TwitterScriptPreviousXIdFile);
                if (data.Length is 0)
                    throw new Exception("Error reading TwitterScriptPreviousXIdFile");

                var lastLine = data.Last(x => !string.IsNullOrWhiteSpace(x));
                var tweet = $"https://fixupx.com/JasmineLovingVR/status/{lastLine}";

                var channel = await Program.DiscordBot.Client.GetChannelAsync(BotConfig.Config.Instance.TwitterChannelId);
                var twitterPostRole = await channel.Guild.GetRoleAsync(BotConfig.Config.Instance.TwitterPostsPingRoleId);

                Program.DiscordBot.Client.Logger.LogInformation("New Twitter post: {0}", tweet);
                await channel.SendMessageAsync($"New Twitter post: {tweet}\n{MarkdownUtils.ToSubText(twitterPostRole.Mention)}");
                outputAction = 4;
            }
            else {
                await Try.Catch(async () => {
                    Process process;

                    if (Vars.IsWindows) {
                        process = new Process {
                            StartInfo = new ProcessStartInfo {
                                FileName = "cmd.exe",
                                Arguments = @"/C .\Twscrape.py",
                                UseShellExecute = false,
                                CreateNoWindow = true,
                                RedirectStandardOutput = true,
                                RedirectStandardError = true
                            }
                        };
                    }
                    else {
                        var tempProcess = new Process {
                            StartInfo = new ProcessStartInfo {
                                FileName = "/bin/bash",
                                Arguments = "-c chmod +x ./Twscrape.py",
                                UseShellExecute = false,
                                CreateNoWindow = true,
                                RedirectStandardOutput = true,
                                RedirectStandardError = true
                            }
                        };
                        tempProcess.Start();
                        await tempProcess.WaitForExitAsync();

                        process = new Process {
                            StartInfo = new ProcessStartInfo {
                                FileName = "/bin/bash",
                                Arguments = "-c python3 ./Twscrape.py",
                                UseShellExecute = false,
                                CreateNoWindow = true,
                                RedirectStandardOutput = true,
                                RedirectStandardError = true
                            }
                        };
                    }

                    process.Start();
                    var output = await process.StandardOutput.ReadToEndAsync();
                    var error = await process.StandardError.ReadToEndAsync();
                    await process.WaitForExitAsync();

                    if (!string.IsNullOrWhiteSpace(error)) {
                        outputAction = 2;
                        throw new Exception($"Error in GetTwitterPostsJob: {error}");
                    }
                }, ex => {
                    OutputAction = TwitterOutputActionEnum.FailedWithErrors;
                    throw new Exception("Error in GetTwitterPostsJob", ex);
                });

                if (BotConfig.Config.Instance.TwitterChannelId is 0) {
                    outputAction = 2;
                    throw new Exception("TwitterChannelId is not set in bot config");
                }

                var channel = await Program.DiscordBot.Client.GetChannelAsync(BotConfig.Config.Instance.TwitterChannelId);
                var twitterPostRole = await channel.Guild.GetRoleAsync(BotConfig.Config.Instance.TwitterPostsPingRoleId);
                var stringJsonData = await File.ReadAllTextAsync(BotConfig.Config.Instance.TwitterScriptOutputFile);

                var data = JsonSerializer.Deserialize<DataJson>(stringJsonData);
                if (data is null) {
                    outputAction = 2;
                    throw new Exception("Error deserializing JSON data");
                }

                if (data.Id == "0" || data.url == "None") {
                    Program.DiscordBot.Client.Logger.LogInformation("No new Twitter post");
                    outputAction = 3;
                    return;
                }

                Program.DiscordBot.Client.Logger.LogInformation("New Twitter post: {0}", data.url);
                await channel.SendMessageAsync($"New Twitter post: {data.url}\n{MarkdownUtils.ToSubText(twitterPostRole.Mention)}");
                outputAction = 1;
            }

            await Task.Delay(TimeSpan.FromSeconds(1));
            await File.WriteAllTextAsync(BotConfig.Config.Instance.TwitterScriptOutputFile, "{\"id\": \"0\", \"URL\": \"None\"}");
        }
    }
}

internal class DataJson {
    [JsonPropertyName("id")] public string? Id { get; set; }
    [JsonPropertyName("URL")] public string? url { get; set; }
}
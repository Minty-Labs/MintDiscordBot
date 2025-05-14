using FluentScheduler;
using Microsoft.Extensions.Logging;

namespace Discord.JasPosts.Tasks {
    internal class Scheduler {
        public async Task Initialize() {
            await Task.Delay(TimeSpan.FromSeconds(30));
            Program.DiscordBot.Client.Logger.LogInformation("Starting the initial Discord Status Job...");
            var statusJob = new Jobs.DiscordStatusJob();
            new Schedule(
                statusJob.Execute,
                run => run.Every(15).Minutes()
            ).Start();
            
            await Task.Delay(TimeSpan.FromSeconds(30));
            Program.DiscordBot.Client.Logger.LogInformation("Starting the initial Twitter Posts Job...");
            var twitterJob = new Jobs.GetTwitterPostsJob();
            new Schedule(
                twitterJob.Execute,
                run => run.Every(10).Minutes()
            ).Start();
        }
    }
}
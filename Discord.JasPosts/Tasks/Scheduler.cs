using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Discord.Common.Helpers;
using Discord.JasPosts;
using Discord.JasPosts.Config;
using Discord.JasPosts.Discord;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Discord.JasPosts.Tasks {
    public class Scheduler {
        public static async Task Initialize() {
            var theScheduler = await SchedulerBuilder.Create()
                .UseDefaultThreadPool(x => x.MaxConcurrency = 2)
                .BuildScheduler();
            await theScheduler.Start();

            var twitterLoopJob = JobBuilder.Create<Jobs.GetTwitterPostsJob>().Build();
            var twitterLoopTrigger = TriggerBuilder.Create()
                .WithIdentity("GetTwitterPosts", "JasPosts")
                .StartAt(DateBuilder.FutureDate(1, IntervalUnit.Minute))
                .WithSimpleSchedule(x => x
                    .WithIntervalInHours(1)
                    .RepeatForever())
                .Build();
            await theScheduler.ScheduleJob(twitterLoopJob, twitterLoopTrigger);
            
            var statusLoopJob = JobBuilder.Create<Jobs.DiscordStatusJob>().Build();
            var statusLoopTrigger = TriggerBuilder.Create()
                .WithIdentity("DiscordStatus", "JasPosts")
                .StartAt(DateBuilder.FutureDate(1, IntervalUnit.Minute))
                .WithSimpleSchedule(x => x
                    .WithIntervalInMinutes(10)
                    .RepeatForever())
                .Build();
            await theScheduler.ScheduleJob(statusLoopJob, statusLoopTrigger);
        }
    }
}
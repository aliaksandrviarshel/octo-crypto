using OctoCrypto.Core.SymbolSummary;
using Quartz;

namespace OctoCrypto.Extensions;

public static class SchedulerExtensions
{
    public static async Task StartScheduler(this WebApplication webApplication)
    {
        using var serviceScope = webApplication.Services.CreateScope();
        var services = serviceScope.ServiceProvider;

        var schedulerFactory = services.GetRequiredService<ISchedulerFactory>();
        var scheduler = await schedulerFactory.GetScheduler();

        var job = JobBuilder.Create<SymbolSummaryJob>()
            .WithIdentity(JobKeys.SymbolSummary)
            .Build();

        var trigger = TriggerBuilder.Create()
            .WithIdentity("initialTrigger", "commonTriggers")
            .StartNow()
            .WithSimpleSchedule(x => x
                .WithIntervalInSeconds(5)
                .RepeatForever())
            .Build();

        await scheduler.ScheduleJob(job, trigger);
    }
}
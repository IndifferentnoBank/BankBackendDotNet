using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace CoreService.Application.BackgroundService;

public static class QuartzScheduler
{
    public static async Task StartTransactionScheduler(IServiceProvider services)
    {
        var schedulerFactory = services.GetRequiredService<ISchedulerFactory>();
        var scheduler = await schedulerFactory.GetScheduler();
        await scheduler.Start();

        var job = JobBuilder.Create<TransactionRetryJob>()
            .WithIdentity("TransactionJob")
            .Build();

        var trigger = TriggerBuilder.Create()
            .WithIdentity("TransactionTrigger")
            .StartNow()
            .WithSimpleSchedule(x => x
                .WithIntervalInMinutes(1)
                .RepeatForever())
            .Build();

        await scheduler.ScheduleJob(job, trigger);
    }
}
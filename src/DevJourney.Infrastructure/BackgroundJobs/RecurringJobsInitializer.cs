using DevJourney.Application.Interfaces.Infrastructure.BackgroundJobs;

namespace DevJourney.Infrastructure.BackgroundJobs;

public class RecurringJobsInitializer(IBackgroundJobService backgroundJobService)
{
    public void RegisterJobs()
    {
        // backgroundJobService.AddOrUpdateRecurringJob<ICartService>(
        //     recurringJobId: "clear-expired-carts",
        //     methodCall: service => service.ClearExpiredCartsAsync(),
        //     cronExpression: "0 0 * * *" // هر شب ساعت ۱۲
        // );
    }
}
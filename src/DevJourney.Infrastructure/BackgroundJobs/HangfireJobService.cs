using System.Linq.Expressions;
using DevJourney.Application.Interfaces.Infrastructure.BackgroundJobs;
using Hangfire;

namespace DevJourney.Infrastructure.BackgroundJobs;

public class HangfireJobService(
    IBackgroundJobClient backgroundJobClient,
    IRecurringJobManager recurringJobManager) : IBackgroundJobService
{
    // --- Enqueue ---
    public string Enqueue(Expression<Action> methodCall) 
        => backgroundJobClient.Enqueue(methodCall);

    public string Enqueue<T>(Expression<Action<T>> methodCall) 
        => backgroundJobClient.Enqueue<T>(methodCall);

    public string Enqueue(Expression<Func<Task>> methodCall) 
        => backgroundJobClient.Enqueue(methodCall);

    public string Enqueue<T>(Expression<Func<T, Task>> methodCall) 
        => backgroundJobClient.Enqueue<T>(methodCall);


    // --- Schedule ---
    public string Schedule(Expression<Action> methodCall, TimeSpan delay) 
        => backgroundJobClient.Schedule(methodCall, delay);

    public string Schedule<T>(Expression<Action<T>> methodCall, TimeSpan delay) 
        => backgroundJobClient.Schedule<T>(methodCall, delay);

    public string Schedule(Expression<Func<Task>> methodCall, TimeSpan delay) 
        => backgroundJobClient.Schedule(methodCall, delay);

    public string Schedule<T>(Expression<Func<T, Task>> methodCall, TimeSpan delay) 
        => backgroundJobClient.Schedule<T>(methodCall, delay);


    // --- Recurring ---
    public void AddOrUpdateRecurringJob(string recurringJobId, Expression<Action> methodCall, string cronExpression) 
        => recurringJobManager.AddOrUpdate(recurringJobId, methodCall, cronExpression);

    public void AddOrUpdateRecurringJob<T>(string recurringJobId, Expression<Action<T>> methodCall, string cronExpression) 
        => recurringJobManager.AddOrUpdate<T>(recurringJobId, methodCall, cronExpression);

    public void AddOrUpdateRecurringJob(string recurringJobId, Expression<Func<Task>> methodCall, string cronExpression) 
        => recurringJobManager.AddOrUpdate(recurringJobId, methodCall, cronExpression);

    public void AddOrUpdateRecurringJob<T>(string recurringJobId, Expression<Func<T, Task>> methodCall, string cronExpression) 
        => recurringJobManager.AddOrUpdate<T>(recurringJobId, methodCall, cronExpression);


    // --- Management ---
    bool IBackgroundJobService.Delete(string jobId) 
        => backgroundJobClient.Delete(jobId);

    void IBackgroundJobService.RemoveIfExists(string recurringJobId) 
        => recurringJobManager.RemoveIfExists(recurringJobId);
}
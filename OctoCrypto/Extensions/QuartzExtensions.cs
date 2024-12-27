using Quartz;

namespace OctoCrypto.Extensions;

public static class QuartzExtensions
{
    public static IServiceCollection AddCustomizedQuartz(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddQuartz(q => { q.UseMicrosoftDependencyInjectionJobFactory(); });
        serviceCollection.AddQuartzHostedService(opt => { opt.WaitForJobsToComplete = true; });
        
        return serviceCollection;
    }
}
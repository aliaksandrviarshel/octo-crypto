using Quartz;

namespace OctoCrypto.Extensions;

public class JobKeys
{
    public static readonly JobKey SymbolSummary = JobKey.Create("symbolSummary", "commonJobs");

}
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace AzureCognitiveSearch.Shared.Utils.Extensions
{
    public static class LoggerExtensions
    {
        public static void LogInformationIfNotRetry(this ILogger log, DurableOrchestrationContext context, string message)
        {
            if (!context.IsReplaying)
            {
                log.LogInformation(message);
            }
        }
    }
}

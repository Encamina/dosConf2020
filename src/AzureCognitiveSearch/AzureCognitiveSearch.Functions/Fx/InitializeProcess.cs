using AzureCognitiveSearch.Shared.Model.Azure.Fx;
using AzureCognitiveSearch.Shared.Model.Enums;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AzureCognitiveSearch.Functions.Fx
{
    public static class InitializeProcess
    { 
  
        [FunctionName("InitializeProcess_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
                                                                [OrchestrationClient] DurableOrchestrationClient starter, ILogger log)
        {

            log.LogInformation($"Start orchestator {nameof(ProcessDocumentsOrchestrator) } at: {DateTime.UtcNow.ToLongDateString()}");
            var process = GetProcess();
            string instanceId = await starter.StartNewAsync(nameof(ProcessDocumentsOrchestrator), process);
            return starter.CreateCheckStatusResponse(req, instanceId);
        }

        private static OrchestrationProcess GetProcess()
        {
            return new OrchestrationProcess
            {
                Id = Guid.NewGuid().ToString(),
                ProcessDate = DateTime.UtcNow,
                Status = ProcessStatusType.InProgress
            };
        }
    }
}
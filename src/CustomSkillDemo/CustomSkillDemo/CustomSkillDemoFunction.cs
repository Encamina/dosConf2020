using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CustomSkillDemo
{
    public partial class SkillRequest
    {
        public Value[] Values { get; set; }
    }

    public partial class SkillResponse
    {        
        public ValueResponse[] Values { get; set; }
    }

    public partial class Value
    {
        public string RecordId { get; set; }
        public Data Data { get; set; }
    }

    public partial class Data
    {
        public string FilePath { get; set; }
    }

    public partial class ValueResponse
    {
        public string RecordId { get; set; }
        public Data Data { get; set; }
        public string[] Errors { get; set; }
        public string[] Warnings { get; set; }
    }


    public static class CustomSkillDemoFunction
    {
        [FunctionName("CustomSkillDemoFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<SkillRequest>(requestBody);

            log.LogInformation($"C# HTTP trigger function request: {requestBody}.");

            var result = new SkillResponse
            {
                Values = new[]
                {
                    new ValueResponse ()
                    {
                        RecordId = data?.Values[0]?.RecordId,
                        Data = new Data
                        {
                            FilePath = $"Custom{Guid.NewGuid().ToString("N")}"
                        }
                    }
                }
            };

            log.LogInformation($"C# HTTP trigger function response: {JsonConvert.SerializeObject(result)}.");

            return new OkObjectResult(result);
        }
    }
}

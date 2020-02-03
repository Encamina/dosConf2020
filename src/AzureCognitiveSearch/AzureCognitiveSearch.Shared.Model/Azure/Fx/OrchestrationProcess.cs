using Newtonsoft.Json;
using System;

namespace AzureCognitiveSearch.Shared.Model.Azure.Fx
{
    public class OrchestrationProcess
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public DateTime ProcessDate { get; set; }
        public string Status { get; set; }
    }
}

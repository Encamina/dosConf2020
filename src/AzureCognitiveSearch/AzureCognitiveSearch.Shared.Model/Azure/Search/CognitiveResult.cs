using Microsoft.Azure.Search.Models;
using System.Collections.Generic;

namespace AzureCognitiveSearch.Shared.Model.Azure.Search
{
    [SerializePropertyNamesAsCamelCase]
    public class CognitiveResult
    {
        public CognitiveResult()
        {
            KeyPhrases = new List<string>();
            Organizations = new List<string>();
            Persons = new List<string>();
            Locations = new List<string>();
        }

        [Newtonsoft.Json.JsonProperty("@search.score")]
        public float Score { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string CosmosId { get; set; }
        public string LanguageCode { get; set; }
        public List<string> KeyPhrases { get; set; }
        public List<string> Organizations { get; set; }
        public List<string> Persons { get; set; }
        public List<string> Locations { get; set; }
        public string MyOcrText { get; set; }
    }
}

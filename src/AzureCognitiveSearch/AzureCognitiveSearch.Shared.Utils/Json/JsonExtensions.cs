using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace AzureCognitiveSearch.Shared.Utils.Json
{
    public static class JsonExtensions
    {
        public static string SerializeToCamelCase<T>(T value)
        {
            return JsonConvert.SerializeObject(
                value,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Formatting = Formatting.Indented,
                    Converters = new List<JsonConverter> { new StringEnumConverter() },
                    NullValueHandling = NullValueHandling.Ignore,
                }
            );
        }
    }
}

using System;

namespace AzureCognitiveSearch.Shared.Utils.Http

{
    public class HttpClientOptions
    {
        public Uri BaseAddress { get; set; }

        public TimeSpan Timeout { get; set; }
    }
}

using System.Net;
using System.Net.Http;

namespace AzureCognitiveSearch.Shared.Utils.Http
{
    public class DefaultHttpClientHandler : HttpClientHandler
    {
        public DefaultHttpClientHandler() => AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
    }
}

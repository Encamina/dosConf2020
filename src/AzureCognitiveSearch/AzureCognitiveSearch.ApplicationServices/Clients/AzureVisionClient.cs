using AzureCognitiveSearch.ApplicationServices.Abstract.Clients;
using AzureCognitiveSearch.Shared.Model.Infrastructure;
using AzureCognitiveSearch.Shared.Utils.Extensions;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AzureCognitiveSearch.ApplicationServices.Clients
{
    public class AzureVisionClient : IAzureVisionClient
    {
        public HttpClient Client { get; }
        private readonly HttpClient client;
        private readonly IAppSettings appSettings;

        public AzureVisionClient(HttpClient client, IAppSettings appSettings)
        {
            this.client = client;
            this.appSettings = appSettings;

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/octet-stream"));
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", this.appSettings.VisioApiKey);
            client.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-azurevision");

            Client = client;
        }

        public async Task<byte[]> GetThumbnail(string docUrl)
        {
            var uri = $"{client.BaseAddress.AbsoluteUri}/generateThumbnail?width={appSettings.ThumbnailWidth}&height={appSettings.ThumbnailHeight}";
            HttpContent content = new StringContent("{'url': '" + docUrl + "'}");
            content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");
            var request = new
            {
                url = docUrl
            };

            return await client.SendRequestToByteArrayAsync(uri, request, null, null, HttpMethod.Post);
        }
    }
}

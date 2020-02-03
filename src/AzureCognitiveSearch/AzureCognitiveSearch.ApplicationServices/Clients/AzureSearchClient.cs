using AzureCognitiveSearch.ApplicationServices.Abstract.Clients;
using AzureCognitiveSearch.Shared.Model.Azure.Search.IndexerStatus;
using AzureCognitiveSearch.Shared.Model.Infrastructure;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AzureCognitiveSearch.ApplicationServices.Clients
{
    public class AzureSearchClient : IAzureSearchClient
    {
        public HttpClient Client { get; }
        private readonly HttpClient client;
        private readonly IAppSettings appSettings;
        private readonly string baseAddress;
        private readonly string apiVersion;

        public AzureSearchClient(HttpClient client, IAppSettings appSettings)
        {
            this.client = client;
            this.appSettings = appSettings;

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("api-key", appSettings.ApiKey);

            baseAddress = string.Format(appSettings.BaseUrl, appSettings.SearchName);
            apiVersion = appSettings.ApiVersion;

            Client = client;
        }

        public async Task<bool> CreateSkillSet(string skillSet, string skillSetJson)
        {
            // TODO: skillSetJson = skillSetJson.Replace("{0}", appSettings.CustomSkillUrl);
            skillSetJson = skillSetJson.Replace("{0}", appSettings.CognitiveTextApiKey);
            var uri = baseAddress + string.Format(appSettings.SearchUrl, $"skillsets/{skillSet}", apiVersion);
            var stringContent = new StringContent(skillSetJson, Encoding.UTF8, "application/json");
            var response = await client.PutAsync(uri, stringContent);
            return response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.NoContent;
        }

        public async Task<bool> CreateDataSource(string datasource, string container, string dataSourceJson)
        {
            dataSourceJson = dataSourceJson.Replace("{0}", datasource);
            dataSourceJson = dataSourceJson.Replace("{1}", appSettings.StorageKey);
            dataSourceJson = dataSourceJson.Replace("{2}", container);
            var uri = baseAddress + string.Format(appSettings.SearchUrl, $"datasources/", apiVersion);
            var stringContent = new StringContent(dataSourceJson, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(uri, stringContent);
            return response.StatusCode == HttpStatusCode.Created;
        }

        public async Task<bool> CreateIndex(string index, string indexJson)
        {
            var uri = baseAddress + string.Format(appSettings.SearchUrl, $"indexes/{index}", apiVersion);
            var stringContent = new StringContent(indexJson, Encoding.UTF8, "application/json");
            var response = await client.PutAsync(uri, stringContent);
            return response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.NoContent;
        }

        public async Task<bool> CreateIndexer(string indexer, string dataSource, string index, string skillSet, string indexerJson)
        {
            indexerJson = indexerJson.Replace("{0}", indexer);
            indexerJson = indexerJson.Replace("{1}", dataSource);
            indexerJson = indexerJson.Replace("{2}", index);
            indexerJson = indexerJson.Replace("{3}", skillSet);
            var uri = baseAddress + string.Format(appSettings.SearchUrl, $"indexers/{indexer}", apiVersion);
            var stringContent = new StringContent(indexerJson, Encoding.UTF8, "application/json");
            var response = await client.PutAsync(uri, stringContent);
            return response.StatusCode == HttpStatusCode.Created;
        }

        public async Task<bool> DeleteSkillSet(string skillSet)
        {
            var uri = baseAddress + string.Format(appSettings.SearchUrl, $"skillsets/{skillSet}", apiVersion);
            var response = await client.DeleteAsync(uri);
            return response.StatusCode == HttpStatusCode.NoContent;
        }

        public async Task<bool> DeleteDataSource(string dataSource)
        {
            var uri = baseAddress + string.Format(appSettings.SearchUrl, $"datasources/{dataSource}", apiVersion);
            var response = await client.DeleteAsync(uri);
            return response.StatusCode == HttpStatusCode.NoContent;
        }

        public async Task<bool> DeleteIndex(string index)
        {
            var uri = baseAddress + string.Format(appSettings.SearchUrl, $"indexes/{index}", apiVersion);
            var response = await client.DeleteAsync(uri);
            return response.StatusCode == HttpStatusCode.NoContent;
        }

        public async Task<bool> DeleteIndexer(string indexer)
        {
            var uri = baseAddress + string.Format(appSettings.SearchUrl, $"indexers/{indexer}", apiVersion);
            var response = await client.DeleteAsync(uri);
            return response.StatusCode == HttpStatusCode.NoContent;
        }

        public async Task<IndexerStatus> GetIndexerStatus(string indexer)
        {
            var uri = baseAddress + string.Format(appSettings.SearchUrl, $"indexers/{indexer}/status", apiVersion);
            var response = await client.GetAsync(uri);
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IndexerStatus>(json);
        }        
    }
}

using AzureCognitiveSearch.ApplicationServices.Abstract.Clients;
using AzureCognitiveSearch.Shared.Model.Azure.Fx;
using AzureCognitiveSearch.Shared.Model.Azure.Search.IndexerStatus;
using Microsoft.Azure.WebJobs;
using System.Threading.Tasks;

namespace AzureCognitiveSearch.Functions.Fx.Activities
{
    public class AzureSearchManagement
    {
        private readonly IAzureSearchClient azureSearchClient;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="azureSearchClient"></param>
        public AzureSearchManagement(IAzureSearchClient azureSearchClient)
        {
            this.azureSearchClient = azureSearchClient;
        }


        /// <summary>
        /// Create a skillset
        /// </summary>
        /// <param name="skillSetParams"></param>
        /// <returns></returns>
        [FunctionName(nameof(FtnCreateSkillSet))]
        public async Task<bool> FtnCreateSkillSet([ActivityTrigger] BaseSearchCreateParameters skillSetParams)
        {
            var result = await azureSearchClient.CreateSkillSet(skillSetParams.Name, skillSetParams.Json);
            return result;
        }

        /// <summary>
        /// Create a datasource
        /// </summary>
        /// <param name="dataSourceParams"></param>
        /// <returns></returns>
        [FunctionName(nameof(FtnCreateDataSource))]
        public async Task<bool> FtnCreateDataSource([ActivityTrigger] CreateDataSourceParameters dataSourceParams)
        {
            var result = await azureSearchClient.CreateDataSource(dataSourceParams.Name, dataSourceParams.Container, dataSourceParams.Json);
            return result;
        }

        /// <summary>
        /// Create an index
        /// </summary>
        /// <param name="indexParams"></param>
        /// <returns></returns>
        [FunctionName(nameof(FtnCreateIndex))]
        public async Task<bool> FtnCreateIndex([ActivityTrigger] BaseSearchCreateParameters indexParams)
        {
            var result = await azureSearchClient.CreateIndex(indexParams.Name, indexParams.Json);
            return result;
        }

        /// <summary>
        /// Create an indexer
        /// </summary>
        /// <param name="indexerParams"></param>
        /// <returns></returns>
        [FunctionName(nameof(FtnCreateIndexer))]
        public async Task<bool> FtnCreateIndexer([ActivityTrigger] CreateIndexerParameters indexerParams)
        {
            var result = await azureSearchClient.CreateIndexer(indexerParams.Name, indexerParams.DataSourceName, indexerParams.IndexName, indexerParams.SkillSetName, indexerParams.Json);
            return result;
        }

        /// <summary>
        /// Delete a skillset
        /// </summary>
        /// <param name="skillSet"></param>
        /// <returns></returns>
        [FunctionName(nameof(FtnDeleteSkillSet))]
        public async Task<bool> FtnDeleteSkillSet([ActivityTrigger] string skillSet)
        {
            var result = await azureSearchClient.DeleteSkillSet(skillSet);
            return result;
        }

        /// <summary>
        /// Delete a datasource
        /// </summary>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        [FunctionName(nameof(FtnDeleteDataSource))]
        public async Task<bool> FtnDeleteDataSource([ActivityTrigger] string dataSource)
        {
            var result = await azureSearchClient.DeleteDataSource(dataSource);
            return result;
        }

        /// <summary>
        /// Delete an index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [FunctionName(nameof(FtnDeleteIndex))]
        public async Task<bool> FtnDeleteIndex([ActivityTrigger] string index)
        {
            var result = await azureSearchClient.DeleteIndex(index);
            return result;
        }

        /// <summary>
        /// Delete an indexer
        /// </summary>
        /// <param name="indexer"></param>
        /// <returns></returns>
        [FunctionName(nameof(FtnDeleteIndexer))]
        public async Task<bool> FtnDeleteIndexer([ActivityTrigger] string indexer)
        {
            var result = await azureSearchClient.DeleteIndexer(indexer);
            return result;
        }

        /// <summary>
        /// Get indexer status
        /// </summary>
        /// <param name="indexer"></param>
        /// <returns></returns>
        [FunctionName(nameof(FtnGetIndexerStatus))]
        public async Task<IndexerStatus> FtnGetIndexerStatus([ActivityTrigger] string indexer)
        {
            var result = await azureSearchClient.GetIndexerStatus(indexer);
            return result;
        }

        /// <summary>
        /// Delete all Azure Search resources
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [FunctionName(nameof(FtnDeleteSearchResources))]
        public async Task<bool> FtnDeleteSearchResources([ActivityTrigger] DeleteSearchResourcesRequest request)
        {
            var deleteSkill = await azureSearchClient.DeleteSkillSet(request.SkillSet);
            var deleteDataSource = await azureSearchClient.DeleteDataSource(request.DataSource);
            var deleteIndex = await azureSearchClient.DeleteIndex(request.Index);
            var deleteIndexer = await azureSearchClient.DeleteIndexer(request.Indexer);

            return deleteSkill && deleteDataSource && deleteIndex && deleteIndexer;
        }        
    }
}
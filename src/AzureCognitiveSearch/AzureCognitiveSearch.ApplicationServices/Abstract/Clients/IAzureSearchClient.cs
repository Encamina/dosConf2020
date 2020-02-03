using AzureCognitiveSearch.Shared.Model.Azure.Search.IndexerStatus;
using System.Threading.Tasks;

namespace AzureCognitiveSearch.ApplicationServices.Abstract.Clients
{
    public interface IAzureSearchClient
    {
        Task<bool> CreateSkillSet(string skillSet, string skillSetJson);
        Task<bool> CreateDataSource(string datasource, string container, string dataSourceJson);
        Task<bool> CreateIndex(string index, string indexJson);
        Task<bool> CreateIndexer(string indexer, string dataSource, string index, string skillSet, string indexerJson);
        Task<bool> DeleteSkillSet(string skillSet);
        Task<bool> DeleteDataSource(string dataSource);
        Task<bool> DeleteIndex(string index);
        Task<bool> DeleteIndexer(string indexer);
        Task<IndexerStatus> GetIndexerStatus(string indexer);        
    }
}

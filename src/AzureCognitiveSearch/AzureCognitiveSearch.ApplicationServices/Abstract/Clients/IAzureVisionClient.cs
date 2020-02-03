using System.Threading.Tasks;

namespace AzureCognitiveSearch.ApplicationServices.Abstract.Clients
{
    public interface IAzureVisionClient
    {
        Task<byte[]> GetThumbnail(string docUrl);
    }
}

using Microsoft.WindowsAzure.Storage.Blob;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureCognitiveSearch.Functions.Common
{
    public static class ContainersTools
    {
        public static async Task<IEnumerable<IListBlobItem>> GetBlobInfoAsync(CloudBlobContainer container, bool flatBlobList)
        {
            BlobContinuationToken continuationToken = null;
            var listBlobs = new List<IListBlobItem>();
            do
            {
                int maxBlobsPerRequest = 1000;
                var response = await container.ListBlobsSegmentedAsync(string.Empty, flatBlobList, BlobListingDetails.None, maxBlobsPerRequest, continuationToken, null, null);
                continuationToken = response.ContinuationToken;
                listBlobs.AddRange(response.Results);
            }
            while (continuationToken != null);
            return listBlobs;
        }
    }
}

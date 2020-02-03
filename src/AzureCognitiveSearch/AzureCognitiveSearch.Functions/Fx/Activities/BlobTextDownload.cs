using AzureCognitiveSearch.Functions.Common;
using AzureCognitiveSearch.Shared.Model.Azure.Fx;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Threading.Tasks;

namespace AzureCognitiveSearch.Functions.Fx.Activities
{
    public static class BlobTextDownload
    {
        [FunctionName(nameof(DownloadBlobText))]
        public static async Task<string> DownloadBlobText([ActivityTrigger] DownloadBlobTextParameters blobParams)
        {
            var blob = GetBlobByName(blobParams.Container, blobParams.File);
            var result = await blob.DownloadTextAsync();
            return result;
        }

        private static CloudBlockBlob GetBlobByName(string containerName, string fileName)
        {
            var container = ConnectionPool.BlobClient.GetContainerReference(containerName);

            return container.GetBlockBlobReference(fileName);
        }
    }
}

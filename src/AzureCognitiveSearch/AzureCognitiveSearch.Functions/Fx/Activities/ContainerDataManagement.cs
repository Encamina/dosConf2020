using AzureCognitiveSearch.Functions.Common;
using AzureCognitiveSearch.Functions.Common.Model;
using AzureCognitiveSearch.Shared.Model.Azure.Fx;
using AzureCognitiveSearch.Shared.Model.Infrastructure;
using AzureCognitiveSearch.Shared.Utils.Extensions;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureCognitiveSearch.Functions.Fx.Activities
{
    public class ContainerDataManagement
    {
        private readonly IAppSettings appSettings;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="appSettings"></param>
        public ContainerDataManagement(IAppSettings appSettings)
        {
            this.appSettings = appSettings;
        }

        /// <summary>
        /// Get Container
        /// </summary>
        /// <param name="containerName"></param>
        /// <returns></returns>
        [FunctionName(nameof(FntGetContainer))]
        public async Task<CloudBlobContainer> FntGetContainer([ActivityTrigger]string containerName)
        {
            var container = GetContainer(containerName);

            var containerExist = await container.ExistsAsync();
            if (!containerExist)
            {
                await container.CreateIfNotExistsAsync();
            }

            return container;
        }

        private CloudBlobContainer GetContainer(string name)
        {
            return ConnectionPool.BlobClient.GetContainerReference(name);
        }

        /// <summary>
        /// Move and delete blob
        /// </summary>
        /// <param name="blob"></param>
        /// <returns></returns>
        [FunctionName(nameof(FntMoveItem))]
        public async Task FntMoveItem([ActivityTrigger] BlobToMove element, ILogger log)
        { 
            CloudBlockBlob destBlob;

            log.LogInformation($"Process {element.ProcessId} (FntMoveItem) - Get blob {element.BlobUri}");
            var srcBlob = new CloudBlob(new Uri(element.BlobUri + element.BlobDeleteToken));

            log.LogInformation($"Process {element.ProcessId} (FntMoveItem) - Move blob {srcBlob.Name} to '{appSettings.DocumentsFolders.ProcessContainer}' container");
            var toContainer = ConnectionPool.ProcessContainer;
            destBlob = toContainer.GetBlockBlobReference(srcBlob.Name);

            var status = await destBlob.MoveAsync(new Uri(element.BlobUri + element.BlobReadToken));

            log.LogInformation($"Process {element.ProcessId} (FntMoveItem) - Blob {srcBlob.Name} status: {status}");
        }

        /// <summary>
        /// Create new Container
        /// </summary>
        /// <param name="input">Container name</param>
        /// <returns>Container name</returns>
        [FunctionName(nameof(FntAddContainer))]
        public async Task<string> FntAddContainer([ActivityTrigger] string name)
        {
            var container = ConnectionPool.BlobClient.GetContainerReference(name);
            var containerExist = await container.ExistsAsync();
            if (!containerExist)
            {
                await container.CreateIfNotExistsAsync();
                await container.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            }
            return container.Name;      
        }

        /// <summary>
        /// Delete container directory
        /// </summary>
        /// <param name="folder">Container and directory name</param>
        [FunctionName(nameof(FntDeleteDirectory))]
        public async Task FntDeleteDirectory([ActivityTrigger] BlobFolder folder, ILogger log)
        {
            try
            {
                log.LogInformation($"Process {folder.ProcessId} (FntDeleteDirectory) - Delete directory '{folder.Folder}' from container ProcessContainer");
                var directory = ConnectionPool.ProcessContainer.GetDirectoryReference(folder.Folder);
                var blobList = await GetAllAsync(directory);
                
                foreach (IListBlobItem item in blobList)
                {
                    if (item.GetType() == typeof(CloudBlob) || item.GetType()?.BaseType == typeof(CloudBlob))
                    {
                        log.LogInformation($"Process {folder.ProcessId} (FntDeleteDirectory) - Delete the blob name {item.Uri}");
                        await ((CloudBlob)item).DeleteIfExistsAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogInformation($"Process {folder.ProcessId} (FntDeleteDirectory) - Delete directory Exception \n Message: {ex.Message} \n StackTrace:" +
                                    $" {ex.StackTrace} \n TargetSite: {ex.TargetSite}");
                throw;
            }           
        }

        /// <summary>
        /// Deletes a container if exists receiving it's name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [FunctionName(nameof(FtnDeleteContainerByName))]
        public async Task<bool> FtnDeleteContainerByName([ActivityTrigger] string name)
        {
            var container = ConnectionPool.BlobClient.GetContainerReference(name);
            return await container.DeleteIfExistsAsync();
        } 
         
        /// <summary>
        /// Get all
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        public async Task<List<IListBlobItem>> GetAllAsync(CloudBlobDirectory container)
        {
            BlobContinuationToken continuationToken = null;
            var listBlobs = new List<IListBlobItem>();
            do
            {
                int maxBlobsPerRequest = 500;
                var response = await container.ListBlobsSegmentedAsync(true, BlobListingDetails.None, maxBlobsPerRequest, continuationToken, null, null);
                continuationToken = response.ContinuationToken;
                listBlobs.AddRange(response.Results);
            }
            while (continuationToken != null);

            return listBlobs;
        }
         
    }
}
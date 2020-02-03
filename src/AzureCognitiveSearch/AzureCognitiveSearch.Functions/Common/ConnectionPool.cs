using AzureCognitiveSearch.Shared.Model.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;

namespace AzureCognitiveSearch.Functions.Common
{
    public static class ConnectionPool
    {
        public static IServiceProvider Container
        {
            get; set;
        }

        public static IConfiguration Configuration
        {
            get; set;
        }

        public static AppSettings AppSettings
        {
            get; set;
        }

        static CloudStorageAccount storageAccount;
        public static CloudStorageAccount StorageAccount
        {
            get
            {
                if (storageAccount == null)
                {
                    InitializeStorageAccount();
                }
                return storageAccount;
            }
        }

        static CloudBlobClient blobClient;
        public static CloudBlobClient BlobClient
        {
            get
            {
                if (blobClient == null)
                {
                    InitializeBlobClient();
                }
                return blobClient;
            }
        }

        static CloudBlobContainer processContainer;
        public static CloudBlobContainer ProcessContainer
        {
            get
            {
                if (processContainer == null)
                {
                    InitializeProcessContainer();
                }
                return processContainer;
            }
        }         

        private static void InitializeStorageAccount()
        {
            var storageKey = AppSettings.StorageKey;
            storageAccount = CloudStorageAccount.Parse(storageKey);
        }

        private static void InitializeBlobClient()
        {
            blobClient = StorageAccount.CreateCloudBlobClient();
        }

        private static void InitializeProcessContainer()
        {
            var processContainerKey = AppSettings.DocumentsFolders.ProcessContainer;
            processContainer = BlobClient.GetContainerReference(processContainerKey);
        } 
       
    }
}

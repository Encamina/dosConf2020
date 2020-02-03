using System;
using System.Threading;
using System.Threading.Tasks;
using OldCopyStatus = Microsoft.WindowsAzure.Storage.Blob.CopyStatus;
using OldCloudBlockBlob = Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob;
using NewCopyStatus = Microsoft.Azure.Storage.Blob.CopyStatus;
using NewCloudBlockBlob = Microsoft.Azure.Storage.Blob.CloudBlockBlob;

namespace AzureCognitiveSearch.Shared.Utils.Extensions
{
    public static class BlobExtensions
    {
        public static async Task<OldCopyStatus> MoveAsync(this OldCloudBlockBlob blob, Uri uri)
        {
            await blob.StartCopyAsync(uri);
            return await CheckOldSDKBlobStatus(blob);
        }

        public static async Task<OldCopyStatus> MoveAsync(this OldCloudBlockBlob targetBlob, OldCloudBlockBlob originBlob)
        {
            await targetBlob.StartCopyAsync(originBlob);
            return await CheckOldSDKBlobStatus(targetBlob);
        }

        public static async Task<(NewCopyStatus status, Uri uri)> MoveAsync(this NewCloudBlockBlob blob, Uri uri)
        {
            await blob.StartCopyAsync(uri);
            return await CheckNewSDKBlobStatus(blob);
        }

        public static async Task<(NewCopyStatus status, Uri uri)> MoveAsync(this NewCloudBlockBlob targetBlob, NewCloudBlockBlob originBlob)
        {
            await targetBlob.StartCopyAsync(originBlob);
            return await CheckNewSDKBlobStatus(targetBlob);
        }

        static async Task<OldCopyStatus> CheckOldSDKBlobStatus(OldCloudBlockBlob blob)
        {
            await blob.FetchAttributesAsync();
            while (blob.CopyState.Status == OldCopyStatus.Pending)
            {
                await blob.FetchAttributesAsync();
                Thread.Sleep(500);
            }

            return blob.CopyState.Status;
        }        

        static async Task<(NewCopyStatus status, Uri uri)> CheckNewSDKBlobStatus(NewCloudBlockBlob blob)
        {
            await blob.FetchAttributesAsync();
            while (blob.CopyState.Status == NewCopyStatus.Pending)
            {
                await blob.FetchAttributesAsync();
                Thread.Sleep(500);
            }

            return (blob.CopyState.Status, blob.Uri);
        }
    }
}

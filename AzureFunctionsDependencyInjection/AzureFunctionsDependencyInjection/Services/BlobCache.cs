using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AzureFunctionsDependencyInjection.Services
{
    public class BlobCache : IBlobCache
    {
        private readonly CloudBlobContainer _container;

        public BlobCache(string storageAccountConnectionString, string blobContainerName)
        {
            _container = GetContainer(storageAccountConnectionString, blobContainerName);
        }


        private CloudBlobContainer GetContainer(string storageAccountConnectionString, string blobContainerNAme)
        {
            try
            {
                var storageAccount = CloudStorageAccount.Parse(storageAccountConnectionString);
                var blobClient = storageAccount.CreateCloudBlobClient();
                return blobClient.GetContainerReference(blobContainerNAme);
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to retrieve blob container by name '{blobContainerNAme}'. {e.Message}");
            }
        }

        public async Task<string> GetBlobContentAsync(string blobName)
        {
            var blob = _container.GetBlobReference(blobName);

            if (await blob.ExistsAsync() == false) return null;

            string content;
            using (var memoryStream = new MemoryStream())
            {
                await blob.DownloadToStreamAsync(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);

                using (var streamReader = new StreamReader(memoryStream))
                {
                    content = streamReader.ReadToEnd();
                }
            }

            return content;
        }

        public async Task SetBlobContentAsync(string blobName, string content)
        {
            var blobReference = _container.GetAppendBlobReference(blobName);
            await blobReference.UploadTextAsync(content);
        }
    }
}

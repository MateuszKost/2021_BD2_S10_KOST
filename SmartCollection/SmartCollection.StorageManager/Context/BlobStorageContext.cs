using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using SmartCollection.StorageManager.Containers;
using SmartCollection.StorageManager.ServiceClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SmartCollection.StorageManager.Context
{
    public class BlobStorageContext<TContainer> : BlobContainerClient, IStorageContext<TContainer> where TContainer:IStorageContainer
    {
        private readonly BlobStorageServiceClient _storage;

        public BlobStorageContext(BlobStorageServiceClient storage)
        {
            _storage = storage;
        }

        public async void AddAsync(TContainer containter, byte[] file, string name)
        {
            var containerClient = _storage.GetBlobContainerClient(containter.GetContainerName());

            // Get a reference to a blob
            var blobClient = containerClient.GetBlobClient(name);

            using(var stream = new MemoryStream(file))
            {
                await blobClient.UploadAsync(stream, true);
                stream.Close();
            }
        }

        public async void DeleteAsync(TContainer containter, string name)
        {
            var container = _storage.GetBlobContainerClient(containter.GetContainerName());

            var blob = container.GetBlobClient(name);

            await blob.DeleteIfExistsAsync();
        }

        public async Task<byte[]> GetAsync(TContainer containter, string name)
        {
            var containerClient = _storage.GetBlobContainerClient(containter.GetContainerName());

            // Get a reference to a blob
            var blobClient = containerClient.GetBlobClient(name);

            BlobDownloadInfo download = await blobClient.DownloadAsync();

            MemoryStream downloadFileStream = new MemoryStream();
            
            await download.Content.CopyToAsync(downloadFileStream);
            
            downloadFileStream.Close();
            
            return downloadFileStream.ToArray();
        }
    }
}

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
            var container = _storage.GetBlobContainerClient(containter.GetName());
            
            await container.CreateIfNotExistsAsync();

            var blob = container.GetBlobClient(name);

            MemoryStream fileStream = new MemoryStream(file);

            await blob.UploadAsync(fileStream);
        }

        public async void DeleteAsync(TContainer containter, string name)
        {
            var container = _storage.GetBlobContainerClient(containter.GetName());

            var blob = container.GetBlobClient(name);

            await blob.DeleteIfExistsAsync();
        }

        public async Task<byte[]> GetAsync(TContainer containter, string name)
        {
            var container = _storage.GetBlobContainerClient(containter.GetName());
            
            var blob = container.GetBlobClient(name);

            if (blob.Exists())
            {
                BlobDownloadInfo download = await blob.DownloadAsync();

                MemoryStream fileStream = new MemoryStream();

                await download.Content.CopyToAsync(fileStream);

                return fileStream.ToArray();
            }
            else return null;
        }
    }
}

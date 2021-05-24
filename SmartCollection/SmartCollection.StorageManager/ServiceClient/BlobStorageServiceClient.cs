using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCollection.StorageManager.ServiceClient
{
    public class BlobStorageServiceClient :BlobServiceClient
    {
        public BlobStorageServiceClient(string connectionString) : base(connectionString)
        {
                
        }
    }
}

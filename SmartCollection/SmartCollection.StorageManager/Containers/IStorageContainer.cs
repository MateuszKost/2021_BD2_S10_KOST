using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCollection.StorageManager.Containers
{
    public interface IStorageContainer
    {
        public string GetBlobName();

        public string GetContainerName();
    }
}

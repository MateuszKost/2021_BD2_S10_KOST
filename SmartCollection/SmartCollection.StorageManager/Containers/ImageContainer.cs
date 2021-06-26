using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCollection.StorageManager.Containers
{
    public class ImageContainer : IStorageContainer
    { 
        public string GetContainerName()
        {
            return "images";
        }
    }
}

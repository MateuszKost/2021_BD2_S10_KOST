using SmartCollection.Models.DBModels;
using SmartCollection.Utilities.TagManagement.TagDownloader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartCollection.Client.Services
{
    public class TagService : ITagService<Tag>
    {
        public IEnumerable<Tag> GetTags(int albumId)
        {
            //var tags = 
            return null;
        }

        //public void UploadTags(IEnumerable<T> tags);
    }
}

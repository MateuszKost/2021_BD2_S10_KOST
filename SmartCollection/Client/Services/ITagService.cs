using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartCollection.Client.Services
{
    public interface ITagService<T> where T : class
    {
        public IEnumerable<T> GetTags(int albumId);

        //public void UploadTags(IEnumerable<T> tags);
    }
}

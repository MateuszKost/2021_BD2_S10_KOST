using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartCollection.Client.Services
{
    public interface ITagService<T> where T : class
    {
        Task<IEnumerable<T>> GetTags(int albumId);

        IEnumerable<string> CreateTagList(string tags);
    }
}

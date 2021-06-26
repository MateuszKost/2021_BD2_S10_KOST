using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCollection.Utilities.TagManagement.TagDownloader
{
    public interface ITagDownloader<T> where T : class
    {
        public Task<T> DownloadTagForAlbumAsync(int albumId); 
    }
}

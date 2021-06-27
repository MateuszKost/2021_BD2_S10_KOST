using SmartCollection.Models.DBModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartCollection.Utilities.TagManagement
{
    public interface ITagManager
    {
        IEnumerable<Tag> DownloadTagForAlbumAsync(int albumId);

        List<int> InsertTags(IEnumerable<string> tags);

        void UpsertImageTags(IEnumerable<int> tagIds, int imageId);

        List<Tag> GetTags(int imageId);

    }
}
using SmartCollection.DataAccess.RepositoryPattern;
using SmartCollection.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCollection.Utilities.TagManagement.TagDownloader
{
    public class TagDownloader : ITagDownloader<IEnumerable<Tag>>
    {
        IUnitOfWork _unitOfWork;
        public TagDownloader(IUnitOfWork unitOfWork)
         => _unitOfWork = unitOfWork;
        
        public async Task<IEnumerable<Tag>> DownloadTagForAlbumAsync(int albumId)
        {
            if (albumId >= 0)
            {
                var images = _unitOfWork.Images.Find(p => p.AlbumId == albumId).ToList();

                var tagOrder = _unitOfWork.TagOrders.GetAll().ToList();

                var tags = new List<Tag>();

                foreach (var tagO in tagOrder)
                    foreach (var img in images)
                        if (tagO.ImageId == img.ImageId)
                            tags.Add(await _unitOfWork.Tags.GetAsync((int)tagO.TagId));

                return tags;
            }
            throw new ArgumentException();
        }
    }
}

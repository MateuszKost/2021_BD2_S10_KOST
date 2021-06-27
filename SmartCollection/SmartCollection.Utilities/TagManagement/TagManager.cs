using SmartCollection.DataAccess.RepositoryPattern;
using SmartCollection.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCollection.Utilities.TagManagement
{
    public class TagManager : ITagManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public TagManager(IUnitOfWork unitOfWork)
         => _unitOfWork = unitOfWork;

        public IEnumerable<Tag> DownloadTagForAlbumAsync(int albumId)
        {
            if (albumId >= 0)
            {
                var images = _unitOfWork.Images.Find(p => p.AlbumId == albumId).Select(x => x.ImageId).ToList();

                var tagIds= _unitOfWork.TagOrders.Find(o => o.ImageId.HasValue && images.Contains((int)o.ImageId)).Select(x => (int)x.TagId);

                return _unitOfWork.Tags.Find(t => tagIds.Contains(t.TagId));
            }
            throw new ArgumentException();
        }

        public List<int> InsertTags(IEnumerable<string> tags)
        {
            tags = new HashSet<string>(tags);
            var existingTags = _unitOfWork.Tags.GetAll().Where(x => tags.Contains(x.Name));

            if(existingTags.Any())
            {
                if (existingTags.Count() == tags.Count())
                    return existingTags.Select(x => x.TagId).ToList();

                var notSaved = tags.Except(existingTags.Select(x => x.Name));

                _unitOfWork.Tags.AddRangeAsync(notSaved.Select(x => new Tag { Name = x }));
                _unitOfWork.Save();

                var saved = _unitOfWork.Tags.Find(x => notSaved.Contains(x.Name));
                var result = new List<int>();
                result.AddRange(saved.Select(x => x.TagId));
                result.AddRange(existingTags.Select(x => x.TagId));
                return result;
            }
            _unitOfWork.Tags.AddRangeAsync(tags.Select(x => new Tag { Name = x }));
            _unitOfWork.Save();

            var newlyAdded = _unitOfWork.Tags.Find(x => tags.Contains(x.Name));

            return newlyAdded.Select(x => x.TagId).ToList();
        }

        public void UpsertImageTags(IEnumerable<int> tagIds, int imageId)
        {
            var newTags = tagIds.Select(id => new ImageTag { ImageId = imageId, TagId = id });
            var existngAssignment = _unitOfWork.TagOrders.Find(x => x.ImageId == imageId && x.TagId.HasValue);

            _unitOfWork.Save();
            if (existngAssignment.Any())
            {
                var toDelete = existngAssignment.Except(newTags);
                var toInsert = newTags.Except(existngAssignment);
                _unitOfWork.TagOrders.RemoveRange(toDelete);
                _unitOfWork.TagOrders.AddRangeAsync(toInsert);
                _unitOfWork.Save();

                return;
            }
            _unitOfWork.TagOrders.AddRangeAsync(tagIds.Select(id => new ImageTag { ImageId = imageId, TagId = id }));
            _unitOfWork.Save();
        }

        public List<Tag> GetTags(int imageId)
        {
            var tagIds = _unitOfWork.TagOrders.Find(x => x.ImageId == imageId && x.TagId.HasValue).Select(x => (int)x.TagId);
            var tags = _unitOfWork.Tags.Find(x => tagIds.Contains(x.TagId));
            return new List<Tag>(tags);
        }
    }
}

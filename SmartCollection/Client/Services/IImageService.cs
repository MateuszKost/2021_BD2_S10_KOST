using SmartCollection.Models.ViewModels;
using SmartCollection.Models.ViewModels.ImagesViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartCollection.Client.Services
{
    public interface IImageService<T> where T : class
    {
        public Task<IEnumerable<T>> GetImagesFromAlbum(int albumId);

        Task<bool> CheckPermissions(int albumId);

        public Task<IEnumerable<T>> GetFilteredImages(FilterParameters filter);

        public Task<Result> UploadImages(IEnumerable<T> images);

        public Task<Result> DeleteImage(int imageId, int albumId);

        public Task<Result> UpdateImage(T image);

        public Task<T> GetImage(int id);

    }
}

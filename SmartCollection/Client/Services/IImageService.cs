using SmartCollection.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartCollection.Client.Services
{
    public interface IImageService<T> where T : class
    {
        public Task<IEnumerable<T>> GetImagesFromAlbum(int albumId);

        public Task<IEnumerable<T>> GetFilteredImages(
            string? tagName,
            string? imageName,
            DateTime? dateFrom,
            DateTime? dateTo);

        public Task<Result> UploadImages(IEnumerable<T> images);

        public Task<Result> DeleteImage(int imageId, int albumId);

        public Task<Result> UpdateImage(T image);

        public Task<T> GetImage(int id);

    }
}

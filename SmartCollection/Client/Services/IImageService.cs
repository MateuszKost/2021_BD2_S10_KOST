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

        public Task<Result> UploadImages(IEnumerable<T> images);

        public Task DeleteImage(T image);

        public Task UpdateImage(T image);

    }
}

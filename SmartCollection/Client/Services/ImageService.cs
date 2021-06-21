using SmartCollection.Models.ViewModels.ImagesViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Net.Http;

namespace SmartCollection.Client.Services
{
    public class ImageService : IImageService<SingleImageViewModel>
         
    {
        private readonly HttpClient _httpClient;

        public ImageService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<SingleImageViewModel>> GetImagesFromAlbum(int albumId)
        {
            var result = await _httpClient.GetFromJsonAsync<ImagesViewModel>("images/getimages/" + albumId);

            if(result.Images != null)
            {
                return result.Images;
            }
            else
            {
                return null;
            }
        }

        public Task UploadImage(SingleImageViewModel image)
        {


            return Task.CompletedTask;
        }

        public Task DeleteImage(SingleImageViewModel image)
        {
            return Task.CompletedTask;
        }

        public Task UpdateImage(SingleImageViewModel image)
        {
            return Task.CompletedTask;
        }

    }
}

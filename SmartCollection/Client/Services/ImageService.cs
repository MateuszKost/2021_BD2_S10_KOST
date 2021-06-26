using SmartCollection.Models.ViewModels.ImagesViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Net.Http;
using SmartCollection.Models.ViewModels;

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

        public async Task<IEnumerable<SingleImageViewModel>> GetFilteredImages(
            string tagName, 
            string imageName, 
            DateTime dateFrom, 
            DateTime dateTo)
        {

            return null;
        }

       
        public async Task<Result> UploadImages(IEnumerable<SingleImageViewModel> images)
        {
            if(images != null)
            {
                ImagesViewModel imagesViewModel = new ImagesViewModel { Images = images };

                var result = await _httpClient.PostAsJsonAsync("images/uploadimages", imagesViewModel);

                if (result.IsSuccessStatusCode)
                    return Result.Success;
                else 
                    return Result.Failure(new[] { "Uploading failed on server" });
            }
            else
            {
                throw new ArgumentNullException("Images cannot be empty");
            }
            
        }

        public Task DeleteImage(SingleImageViewModel image)
        {
            //TODO
            // - optional parameter for albumId, then call API DeleteFromAlbum
            // - no parameter - delete image from server and db - DeleteImage API
            return Task.CompletedTask;
        }

        public Task UpdateImage(SingleImageViewModel image)
        {
            //TODO
            return Task.CompletedTask;
        }

    }
}

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
        private readonly string controller = "images";

        public ImageService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<SingleImageViewModel>> GetImagesFromAlbum(int albumId)
        {
            var result = await _httpClient.GetFromJsonAsync<ImagesViewModel>(controller + "/getimages/" + albumId);

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

                var result = await _httpClient.PostAsJsonAsync(controller + "/uploadimages", imagesViewModel);

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

        public async Task<Result> DeleteImage(int imageId, int albumId)
        {
            HttpResponseMessage result;
            string targetPath;

            if (albumId != 0)
            {
                targetPath = "/" + albumId + "/" + imageId;
                result = await _httpClient.DeleteAsync(controller + "/deletefromalbum" + targetPath);
            }
            else
            {
                targetPath = "/" + imageId;
                result = await _httpClient.DeleteAsync(controller + "/deleteimage" + targetPath);
            }

            return result.IsSuccessStatusCode ? Result.Success : Result.Failure(errors: new[] { result.Content.ToString() });

        }

        public async Task<Result> UpdateImage(SingleImageViewModel image)
        {
            var result = await _httpClient.PostAsJsonAsync<SingleImageViewModel>(controller + "/update", image);
            return result.IsSuccessStatusCode ? Result.Success : Result.Failure(errors: new[] { "Update failed" });
        }

        public async Task<SingleImageViewModel> GetImage(int id)
        {
            var result = await _httpClient.GetFromJsonAsync<SingleImageViewModel>("images/getImage/" + id);
            if (result != null)
                return result;
            else
                return null;
        }

    }
}

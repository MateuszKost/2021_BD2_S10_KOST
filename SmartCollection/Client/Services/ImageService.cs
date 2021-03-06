using SmartCollection.Models.ViewModels.ImagesViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Net.Http;
using SmartCollection.Models.ViewModels;
using SmartCollection.Models.DBModels;
using Newtonsoft.Json;

namespace SmartCollection.Client.Services
{
    public class ImageService : IImageService<SingleImageViewModel>
         
    {
        private readonly HttpClient _httpClient;
        private readonly string api = "images";

        public ImageService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<SingleImageViewModel>> GetImagesFromAlbum(int albumId)
        {
            var result = await _httpClient.GetFromJsonAsync<ImagesViewModel>(api + "/getimages/" + albumId);

            if(result.Images != null)
            {
                return result.Images;
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<SingleImageViewModel>> GetFilteredImages(FilterParameters filter)
        {
            var response = await _httpClient.PostAsJsonAsync<FilterParameters>(api + "/filter", filter);
            var result = await response.Content.ReadAsStringAsync();

            var filteredImages = JsonConvert.DeserializeObject<ImagesViewModel>(result);
            
            return filteredImages.Images;
        }

        public async Task<Result> UploadImages(IEnumerable<SingleImageViewModel> images)
        {
            if(images != null)
            {
                ImagesViewModel imagesViewModel = new ImagesViewModel { Images = images };

                var result = await _httpClient.PostAsJsonAsync(api + "/uploadimages", imagesViewModel);

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
                result = await _httpClient.DeleteAsync(api + "/deletefromalbum" + targetPath);
            }
            else
            {
                targetPath = "/" + imageId;
                result = await _httpClient.DeleteAsync(api + "/deleteimage" + targetPath);
            }

            return result.IsSuccessStatusCode ? Result.Success : Result.Failure(errors: new[] { result.Content.ToString() });

        }

        public async Task<Result> UpdateImage(SingleImageViewModel image)
        {
            var result = await _httpClient.PostAsJsonAsync<SingleImageViewModel>(api + "/update", image);
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

        public async Task<bool> CheckPermissions(int albumId)
        {
            var result = await _httpClient.GetAsync("albums/" + albumId);

            if(result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }
    }
}

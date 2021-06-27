using SmartCollection.Models.ViewModels;
using SmartCollection.Models.ViewModels.AlbumViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SmartCollection.Client.Services
{
    public class AlbumService : IAlbumService
    {
        private readonly HttpClient _httpClient;
        private readonly string api = "albums";
        public AlbumService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IEnumerable<SingleAlbumViewModel>> GetAlbums()
        {
            var result = await _httpClient.GetFromJsonAsync<AlbumViewModel>("albums");
            return result.AlbumViewModelList;
        }

        public async Task<SingleAlbumViewModel> GetAlbum(int id)
        {
            var result = await _httpClient.GetFromJsonAsync<SingleAlbumViewModel>("albums/getalbum/" + id);
            if (result != null)
                return result;
            else
                return null;
        }

        public async Task<Result> UpdateAlbum(SingleAlbumViewModel album)
        {
            var result = await _httpClient.PostAsJsonAsync<SingleAlbumViewModel>(api + "/edit", album);
            return result.IsSuccessStatusCode ? Result.Success : Result.Failure(errors: new[] { "Update failed" });
        }

        public async Task<Result> DeleteAlbum (int id)
        {
            var result = await _httpClient.DeleteAsync(api + "/delete/" + id);
            return result.IsSuccessStatusCode ? Result.Success : Result.Failure(errors: new[] { "Deletion failed" });
        }
    }
}

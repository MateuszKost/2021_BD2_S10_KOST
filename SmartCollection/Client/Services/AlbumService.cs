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
        public AlbumService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IEnumerable<SingleAlbumViewModel>> GetAlbums()
        {
            var result = await _httpClient.GetFromJsonAsync<AlbumViewModel>("albums");
            return result.AlbumViewModelList;
        }
    }
}

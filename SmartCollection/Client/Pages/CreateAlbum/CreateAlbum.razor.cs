using Newtonsoft.Json;
using SmartCollection.Models.ViewModels.CreateAlbumViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SmartCollection.Client.Pages.CreateAlbum
{
    public partial class CreateAlbum
    {
        private CreateAlbumViewModel _createAlbumModel = new();

        public async Task AddAlbum()
        {
            var jsonModel = JsonConvert.SerializeObject(_createAlbumModel);
            await Http.PostAsync("https://localhost:44368/CreateAlbum", new StringContent(jsonModel, Encoding.UTF8, "application/json"));
        }
    }
}

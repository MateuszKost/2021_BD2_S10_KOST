using Microsoft.AspNetCore.Components;
using SmartCollection.Client.Services;
using SmartCollection.Models.ViewModels.ImagesViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SmartCollection.Client.Pages.Images
{
    public partial class Images
    {
        [Parameter]
        public string AlbumId { get; set; }

        private IEnumerable<SingleImageViewModel> images;

        protected override async Task OnInitializedAsync()
        {
            images = await ImageService.GetImagesFromAlbum(int.Parse(AlbumId));
            StateHasChanged();
        }

        private string getPicture()
        {
            Random random = new Random();
            string path = @"\albumPic\" + random.Next(1, 10) + ".jpg";
            return path;
        }

    }
}

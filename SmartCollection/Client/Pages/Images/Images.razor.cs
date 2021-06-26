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

        //[Parameter]
        //public IEnumerable<> Tags { get; set; }

        private readonly FilterImagesViewModel FilterModel = new();

        private IEnumerable<SingleImageViewModel> images;

        protected override async Task OnInitializedAsync()
        {
            images = await ImageService.GetImagesFromAlbum(int.Parse(AlbumId));
            StateHasChanged();
        }

        private void OnFilter()
        {
            Console.WriteLine("Filtering called");
        }

    }
}

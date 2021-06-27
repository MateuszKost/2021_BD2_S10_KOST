using Microsoft.AspNetCore.Components;
using SmartCollection.Client.Services;
using SmartCollection.Models.DBModels;
using SmartCollection.Models.ViewModels.ImagesViewModel;
using SmartCollection.Utilities.TagManagement.TagDownloader;
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

        [Parameter]
        public IEnumerable<Tag> Tags { get; set; }

        [Parameter]
        public int SelectedTagId { get; set; }

        private readonly FilterImagesViewModel FilterModel = new();

        private IEnumerable<SingleImageViewModel> images;

        protected override async Task OnInitializedAsync()
        {
            images = await ImageService.GetImagesFromAlbum(int.Parse(AlbumId));
            Tags = await TagService.GetTags(int.Parse(AlbumId));
            StateHasChanged();
        }

        private void OnFilter()
        {
            //TODO
            // filter method sending request from service to server
            // return is a new list of filtered images or empty list
            Console.WriteLine("Filtering called");
        }

        private void OnTagSelected(ChangeEventArgs e)
        {
            SelectedTagId = int.Parse(e.Value.ToString());
            StateHasChanged();
        }

        private void Navigate(int imageId)
        {
            NavigationManager.NavigateTo("editimage/" + imageId);
        }

        private async Task Delete(int imageId)
        {
            await ImageService.DeleteImage(imageId, int.Parse(AlbumId));
            StateHasChanged();
        }

    }
}

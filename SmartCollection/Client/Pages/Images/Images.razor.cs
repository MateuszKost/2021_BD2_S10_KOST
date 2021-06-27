using Microsoft.AspNetCore.Components;
using SmartCollection.Models.DBModels;
using SmartCollection.Models.ViewModels.ImagesViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartCollection.Client.Pages.Images
{
    public partial class Images
    {
        [Parameter]
        public int AlbumId { get; set; }

        [Parameter]
        public IEnumerable<Tag> Tags { get; set; }

        [Parameter]
        public int SelectedTagId { get; set; }

        private readonly FilterImagesViewModel FilterModel = new();

        private IEnumerable<SingleImageViewModel> images;

        protected override async Task OnInitializedAsync()
        {
            images = await ImageService.GetImagesFromAlbum(AlbumId);
            Tags = await TagService.GetTags(AlbumId);
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
            await ImageService.DeleteImage(imageId, AlbumId);
            StateHasChanged();
        }
    }
}
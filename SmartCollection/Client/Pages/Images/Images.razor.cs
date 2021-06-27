using Microsoft.AspNetCore.Components;
using SmartCollection.Models.DBModels;
using SmartCollection.Models.ViewModels.ImagesViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartCollection.Client.Pages.Images
{
    public partial class Images
    {
        [Parameter]
        public string AlbumId { get; set; }

        [Parameter]
        public IEnumerable<Tag> Tags { get; set; }

        private readonly FilterImagesViewModel FilterModel = new();

        private IEnumerable<SingleImageViewModel> images;

        protected override async Task OnInitializedAsync()
        {
            images = await ImageService.GetImagesFromAlbum(int.Parse(AlbumId));
            Tags = await TagService.GetTags(int.Parse(AlbumId));
            StateHasChanged();
        }

        private async void OnFilter()
        {
            var filteredImages = await ImageService.GetFilteredImages(FilterModel);
            StateHasChanged();

            Console.WriteLine("Filtering called");
            Console.WriteLine("DateFrom: " + FilterModel.DateFrom);
            Console.WriteLine("DateTo: " + FilterModel.DateTo);
            Console.WriteLine("Selected tag: " + FilterModel.TagId);
        }

        private void OnTagSelected(ChangeEventArgs e)
        {
            FilterModel.AlbumId = int.Parse(AlbumId);
            FilterModel.TagId = int.Parse(e.Value.ToString());
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
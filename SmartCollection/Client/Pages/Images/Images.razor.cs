using Microsoft.AspNetCore.Components;
using SmartCollection.Client.Services;
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
        public int AlbumId { get; set; }

        [Parameter]
        public IEnumerable<Tag> Tags { get; set; }

        private readonly FilterParameters FilterModel = new();

        private IEnumerable<SingleImageViewModel> images;

        protected override async Task OnInitializedAsync()
        {
            images = await ImageService.GetImagesFromAlbum(AlbumId);
            Tags = await TagService.GetTags(AlbumId);
            StateHasChanged();
        }

        private async void OnFilter()
        {
            FilterModel.AlbumId = AlbumId;
            _ = FilterModel.DateFrom != null ? FilterModel.DateFrom : default;
            _ = FilterModel.DateTo != null ? FilterModel.DateTo : default;
            _ = FilterModel.ImageName ?? null;
            var filteredImages = await ImageService.GetFilteredImages(FilterModel);
            StateHasChanged();

            Console.WriteLine("Filtering called");
            Console.WriteLine("DateFrom: " + FilterModel.DateFrom);
            Console.WriteLine("DateTo: " + FilterModel.DateTo);
            Console.WriteLine("Selected tag: " + FilterModel.TagId);
        }

        private void OnTagSelected(ChangeEventArgs e)
        {
            int tagId = int.Parse(e.Value.ToString());
            FilterModel.TagId = tagId != 0 ? tagId : -1;

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
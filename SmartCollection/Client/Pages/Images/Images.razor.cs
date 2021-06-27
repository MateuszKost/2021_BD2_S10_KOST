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
            var tagName = SelectedTagId != 0 ? Tags.Where(tag => tag.TagId == SelectedTagId).FirstOrDefault().Name : null;

            var result = ImageService.GetFilteredImages(
                tagName: tagName,
                imageName: FilterModel.Name,
                dateFrom: FilterModel.DateFrom,
                dateTo: FilterModel.DateTo);

            Console.WriteLine("Filtering called");
            Console.WriteLine("DateFrom: " + FilterModel.DateFrom);
            Console.WriteLine("DateTo: " + FilterModel.DateTo);
            Console.WriteLine("Selected tag: " + SelectedTagId);
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
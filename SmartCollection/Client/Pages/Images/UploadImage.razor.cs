using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SmartCollection.Client.Services;
using SmartCollection.Models.ViewModels.AlbumViewModel;
using SmartCollection.Models.ViewModels.ImagesViewModel;
using SmartCollection.Utilities.ImageConverter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SmartCollection.Client.Pages.Images
{

    public partial class UploadImage
    {
        private readonly ImageConverter imageConverter = new ImageConverter { MaxFileSize = 1024 * 1024 * 15 };

        [Parameter]
        public IEnumerable<SingleAlbumViewModel> Albums { get; set; }
        [Parameter]
        public int SelectedAlbumId { get; set; }

        private ImagesViewModel imagesViewModel;

        private IReadOnlyList<IBrowserFile> fileList;
        private List<(string Url, string Name)> images;

        private string ImageUri, ImageName;
        private int ImageCount = 0;
        private bool isLoaded = false;

        protected override async Task OnInitializedAsync()
        {
            Albums = await AlbumService.GetAlbums();
            StateHasChanged();
        }
        private async Task LoadImage(InputFileChangeEventArgs eventArgs)
        {
            if (eventArgs.FileCount > 1)
            {
                fileList = eventArgs.GetMultipleFiles();
                images = new List<(string, string)>();

                foreach (var file in fileList)
                {
                    images.Add((await GetImageUrl(file), file.Name));
                }

                ImageCount = eventArgs.FileCount;
                isLoaded = true;
            }
            else
            {
                ImageCount = 1;
                var file = eventArgs.File;
                images = null;
                ImageUri = await GetImageUrl(file);
                ImageName = file.Name;
                isLoaded = true;
            }
        }

        private async Task<string> GetImageUrl(IBrowserFile file)
        {
            string base64 = await imageConverter.IBrowserFileImageToBase64Async(file);
            string uri = "data:" + file.ContentType+ ";base64," + base64;

            return uri;
        }

        /*
         * NOT FINISHED 
         * TODO: 
         * - components for image: name, description, tags
         * - onclick event on Upload button
         * - modal component with success / error information
         * - getting selected album from <select> field
         * - clearing view after upload or redirect to albums
         */
        private async void UploadImages()
        {
            imagesViewModel = new();
            var requestResult = await ImageService.UploadImages(imagesViewModel.Images);
            if(requestResult.Succeeded)
            {
                Console.WriteLine("Uploaded images");
            }
            else
            {
                Console.WriteLine("Failure: " + requestResult.Errors);
            }
        }
    }
}

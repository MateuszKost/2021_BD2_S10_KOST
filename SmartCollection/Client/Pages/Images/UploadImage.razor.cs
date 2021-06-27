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

        private List<IBrowserFile> fileList;
        private List<(string Url, string Name)> images;

        private int imageCount = 0, maxImageCount = 15;
        private bool isLoaded = false;

        private bool uploadSucceed = false, uploadFailed = false;
        private string errorMessage;

        protected override async Task OnInitializedAsync()
        {
            Albums = await AlbumService.GetAlbums();
            SelectedAlbumId = 0;
            StateHasChanged();
        }

        private async void LoadImage(InputFileChangeEventArgs eventArgs)
        {
            isLoaded = false;
            imageCount = 0;
            fileList = new List<IBrowserFile>();
            images = new List<(string, string)>();

            if (eventArgs.FileCount >= 1 && eventArgs.FileCount <= maxImageCount)
            {
                fileList = eventArgs.GetMultipleFiles(maximumFileCount: 15).ToList();

                foreach (var file in fileList)
                {
                    images.Add((await GetImageUrl(file), file.Name));
                }

                imageCount = eventArgs.FileCount;
                isLoaded = true;
            }
            else if (eventArgs.FileCount > maxImageCount)
            {
                errorMessage = "You can load up to " + maxImageCount.ToString() + " pictures at once.";
                isLoaded = false;
            }

            StateHasChanged();
        }

        private async Task<string> GetImageUrl(IBrowserFile file)
        {
            string base64 = await imageConverter.IBrowserFileImageToBase64Async(file);
            string uri = "data:" + file.ContentType+ ";base64," + base64;

            return uri;
        }

        private void OnSelect(ChangeEventArgs e)
        {
            SelectedAlbumId = int.Parse(e.Value.ToString());
            Console.WriteLine(SelectedAlbumId);
        }

        /*
         * NOT FINISHED 
         * TODO: 
         * - components for image: name, description, tags
         * - modal component with success / error information
         * - clearing view after upload or redirect to albums - redirect to all images done
         */
        private async void UploadImages()
        {
            if(fileList == null)
            {
                uploadFailed = true;
                errorMessage = "Cannot upload nothing";
            }
            else
            {
                uploadFailed = false;
            }

            List<SingleImageViewModel> imagesToUpload = new();

            foreach(var file in fileList)
            {
                SingleImageViewModel image = new SingleImageViewModel()
                {
                    Name = file.Name,
                    Data = imageConverter.IBrowserFileImageToBase64Async(file),
                    Date = DateTime.Now.ToString(),
                    AlbumId = SelectedAlbumId,
                    Description = "Describe your picture"
                };

                imagesToUpload.Add(image);
            }

            var requestResult = await ImageService.UploadImages(imagesToUpload);

            if(requestResult.Succeeded)
            {
                Console.WriteLine("Uploaded images");
                uploadSucceed = true;
                StateHasChanged();
            }
            else
            {
                Console.WriteLine("Failure: " + requestResult.Errors);
                uploadFailed = true;
                errorMessage = string.Join("\n", requestResult.Errors);
                StateHasChanged();
            }
        }
    }
}

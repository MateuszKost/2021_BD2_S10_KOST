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

        private IReadOnlyList<IBrowserFile> fileList;
        private List<(string Url, string Name)> images;

        private string ImageUri, ImageName;
        private int ImageCount = 0;
        private bool isLoaded = false;

        private bool UploadSucceed;
        private bool UploadFailed;
        private string Error;

        protected override async Task OnInitializedAsync()
        {
            Albums = await AlbumService.GetAlbums();
            SelectedAlbumId = 0;
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

        private void OnSelect(ChangeEventArgs e)
        {
            SelectedAlbumId = int.Parse(e.Value.ToString());
            Console.WriteLine(SelectedAlbumId);
        }
        /*
         * NOT FINISHED 
         * TODO: 
         * - components for image: name, description, tags
         * - onclick event on Upload button - done, should be working
         * - modal component with success / error information
         * - getting selected album from <select> field - done
         * - clearing view after upload or redirect to albums - redirect to all images done
         */
        private async void UploadImages()
        {
            if(fileList == null)
            {
                UploadFailed = true;
                Error = "Cannot upload nothing";
            }
            else
            {
                UploadFailed = false;
            }

            List<SingleImageViewModel> imagesToUpload = new();

            for(int i = 0; i < fileList.Count; i++)
            {
                SingleImageViewModel image = new SingleImageViewModel
                {
                    Name = fileList[i].Name,
                    Data = await imageConverter.IBrowserFileImageToBase64Async(fileList[i]),
                    Date = DateTime.Now.ToString(),
                    AlbumId = SelectedAlbumId,
                    Description = "Test description" // TODO DESCRIPTION FIELD
                };

                imagesToUpload.Add(image);
            }
            var requestResult = await ImageService.UploadImages(imagesToUpload);

            if(requestResult.Succeeded)
            {
                Console.WriteLine("Uploaded images");
                UploadSucceed = true;
            }
            else
            {
                Console.WriteLine("Failure: " + requestResult.Errors);
                UploadFailed = true;
                Error = requestResult.Errors.First();
            }
        }
    }
}

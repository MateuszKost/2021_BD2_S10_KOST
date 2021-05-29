using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartCollection.Client.Pages.Images
{

    public partial class UploadImage
    {
        IReadOnlyList<IBrowserFile> fileList;
        List<(string Url, string Name)> images;

        string ImageUri, ImageName;
        int ImageCount = 0;
        bool isLoaded = false;
        long maxFileSize = 1024 * 1024 * 15;

        async Task LoadImage(InputFileChangeEventArgs eventArgs)
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
            var imgFile = await file.RequestImageFileAsync("image/jpeg", 6000, 6000);

            using System.IO.Stream fileStream = imgFile.OpenReadStream(maxFileSize);
            using System.IO.MemoryStream ms = new();

            await fileStream.CopyToAsync(ms);
            var convertedStream = Convert.ToBase64String(ms.ToArray());
            var uri = "data:image/jpeg;base64," + convertedStream;

            return uri;
        }

        List<TempAlbumModel> albums = new List<TempAlbumModel>()
        {
            new TempAlbumModel(){ Id = 1, Name = "Dogs", Brief = "Best pets", PrivacyType = "Private" },
            new TempAlbumModel(){ Id = 2, Name = "Cats", Brief = "All my kitties", PrivacyType = "Private" },
            new TempAlbumModel(){ Id = 3,Name = "Memes", Brief = "Hehehehehe", PrivacyType = "Public" },
            new TempAlbumModel(){ Id = 4, Name = "Wedding", Brief = "Wedding 2012", PrivacyType = "Private" }
        };

    }
    public class TempAlbumModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Brief { get; set; }
        public string PrivacyType { get; set; }
    }
}

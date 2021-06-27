using ImageMagick;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SmartCollection.Utilities.ImageConverter
{
    public class ImageConverter : IImageConverter
    {
        public long MaxFileSize { get; set; }

        public async Task<string> IBrowserFileImageToBase64Async(IBrowserFile file)
        {
            var contentType = file.ContentType;

            // we want only jpeg and png file
            if (contentType.Contains("jpeg") || contentType.Contains("png"))
            {
                using Stream fileStream = file.OpenReadStream(MaxFileSize);
                using var ms = new MemoryStream();
                await fileStream.CopyToAsync(ms);
                return Convert.ToBase64String(ms.ToArray());
            }
            else
                throw new BadImageFormatException();
        }

        public string ImageBytesToBase64(byte[] file)
        {
            return Convert.ToBase64String(file);
        }

        public byte[] Base64ToImage(string base64)
        {
            return Convert.FromBase64String(base64);
        }
    }
}
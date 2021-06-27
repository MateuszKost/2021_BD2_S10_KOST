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

        public string IBrowserFileImageToBase64Async(IBrowserFile file)
        {
            var contentType = file.ContentType;

            // we want only jpeg and png file
            if (contentType.Contains("jpeg") || contentType.Contains("png"))
            {
                using Stream fileStream = file.OpenReadStream(MaxFileSize);
                using var image = new MagickImage(fileStream);
                return image.ToBase64();
            }
            else
                throw new BadImageFormatException();
        }

        public string ImageBytesToBase64(byte[] file)
        {
            using var image = new MagickImage(file);
            return image.ToBase64();
        }

        public byte[] Base64ToImage(string base64)
        {
            using var image = new MagickImage(base64);
            return image.ToByteArray();
        }
    }
}
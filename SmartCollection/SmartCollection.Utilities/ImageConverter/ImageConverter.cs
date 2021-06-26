using Microsoft.AspNetCore.Components.Forms;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                using MemoryStream ms = new();

                await fileStream.CopyToAsync(ms);
                var base64 = Convert.ToBase64String(ms.ToArray());

                return base64;
            }
            else
            {
                throw new BadImageFormatException();
            }

           
        }

        public string ImageBytesToBase64(byte[] file)
        {
            return file != null ? Convert.ToBase64String(file) : null;
        }

        public byte[] Base64ToImage(string base64)
        {
            return base64 != null ? Convert.FromBase64String(base64) : null;
        }
    }
}

using Microsoft.AspNetCore.Components.Forms;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCollection.Utilities.ImageConverter
{
    public class ImageConverter : IImageConverter<IBrowserFile>
    {
        public long MaxFileSize { get; set; }

        public async Task<string> IBrowserFileImageToBase64Async(IBrowserFile file)
        {
            var contentType = file.ContentType;
            IBrowserFile imgFile;

            // we want only jpeg and png file
            if (contentType.Contains("jpeg") || contentType.Contains("png"))
            {
                imgFile = await file.RequestImageFileAsync(contentType, 6000, 6000);
            }
            else
            {
                throw new BadImageFormatException();
            }

            using Stream fileStream = imgFile.OpenReadStream(MaxFileSize);
            using MemoryStream ms = new();

            await fileStream.CopyToAsync(ms);
            var base64 = Convert.ToBase64String(ms.ToArray());

            return base64;
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

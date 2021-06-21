using Microsoft.AspNetCore.Components.Forms;
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
            var imgFile = await file.RequestImageFileAsync("image/jpeg", 6000, 6000);

            using System.IO.Stream fileStream = imgFile.OpenReadStream(MaxFileSize);
            using System.IO.MemoryStream ms = new();

            await fileStream.CopyToAsync(ms);
            var base64 = Convert.ToBase64String(ms.ToArray());

            return base64;
        }

        public async Task<string> ImageBytesToBase64(byte[] file)
        {
            // NOT IMPLEMENTED YET
            return null;
        }

        public byte[] Base64ToImage(string base64)
        {
            // NOT IMPLEMENTED YET
            return null;
        }
    }
}

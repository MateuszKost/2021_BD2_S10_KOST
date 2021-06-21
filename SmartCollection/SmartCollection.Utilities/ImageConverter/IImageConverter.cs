using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCollection.Utilities.ImageConverter
{
    public interface IImageConverter<T>
    {
        public Task<string> IBrowserFileImageToBase64Async(T file);

        public string ImageBytesToBase64(byte[] file);

        public byte[] Base64ToImage(string base64);
    }
}

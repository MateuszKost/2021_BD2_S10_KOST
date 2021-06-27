using ImageMagick;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartCollection.StorageManager.Containers;
using SmartCollection.StorageManager.Context;
using System.Net.Mime;
using System.Threading.Tasks;

namespace SmartCollection.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ThumbnailController : Controller
    {
        private readonly IStorageContext<IStorageContainer> _storageContext;

        public ThumbnailController(IStorageContext<IStorageContainer> storageContext)
        {
            _storageContext = storageContext;
        }

        [HttpGet("{sha1}")]
        public async Task<ActionResult> GetThumbnailAsync(string sha1)
        {
            byte[] bytes = await _storageContext.GetAsync(new ImageContainer(), sha1);
            using var image = new MagickImage(bytes);
            var size = new MagickGeometry(200, 200);
            image.Resize(size);
            return File(image.ToByteArray(), MediaTypeNames.Image.Jpeg);
        }
    }
}
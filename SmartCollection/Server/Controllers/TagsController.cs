using Microsoft.AspNetCore.Mvc;
using SmartCollection.Models.DBModels;
using SmartCollection.Utilities.TagManagement.TagDownloader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartCollection.Server.Controllers
{
    public class TagsController : Controller
    {
        private readonly ITagDownloader<IEnumerable<Tag>> _tagDownloader;

        public TagsController(ITagDownloader<IEnumerable<Tag>> tagDownloader)
        {
            _tagDownloader = tagDownloader;
        }

        [HttpGet]
        [Route("")]
        public async Task<IEnumerable<Tag>> GetTagsFromAlbum(int albumId)
        {
            var tags = await _tagDownloader.DownloadTagForAlbumAsync(albumId);

            if(tags != null)
            {
                return tags;
            }
            else
            {
                return null;
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using SmartCollection.Models.DBModels;
using SmartCollection.Models.ViewModels.TagsViewModel;
using SmartCollection.Utilities.TagManagement.TagDownloader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartCollection.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TagsController : Controller
    {
        private readonly ITagDownloader<IEnumerable<Tag>> _tagDownloader;

        public TagsController(ITagDownloader<IEnumerable<Tag>> tagDownloader)
        {
            _tagDownloader = tagDownloader;
        }

        [HttpGet]
        [Route("get/{albumId}")]
        public async Task<TagsViewModel> GetTagsFromAlbum(int albumId)
        {
            var tags = await _tagDownloader.DownloadTagForAlbumAsync(albumId);

            if(tags.Any())
            {
                return new TagsViewModel() { Tags = tags };
            }
            else
            {
                return new TagsViewModel() { Tags = null };
            }
        }
    }
}

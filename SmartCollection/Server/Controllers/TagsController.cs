using Microsoft.AspNetCore.Mvc;
using SmartCollection.Models.DBModels;
using SmartCollection.Models.ViewModels.TagsViewModel;
using SmartCollection.Utilities.TagManagement;
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
        private readonly ITagManager _tagManager;

        public TagsController(ITagManager tagManager)
        {
            _tagManager = tagManager;
        }

        [HttpGet]
        [Route("get/{albumId}")]
        public async Task<TagsViewModel> GetTagsFromAlbum(int albumId)
        {
            var tags = await _tagManager.DownloadTagForAlbumAsync(albumId);

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

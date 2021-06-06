using Microsoft.AspNetCore.Mvc;
using SmartCollection.Models.ViewModels.AlbumViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartCollection.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AlbumController : Controller
    {  
        [Route("")]
        [HttpGet]
        public async Task<AlbumViewModel> GetAlbums()
        {
          
            return new AlbumViewModel();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using SmartCollection.Models.ViewModels.CreateAlbumViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SmartCollection.Client.Pages.CreateAlbum.CreateAlbum;

namespace SmartCollection.Server.Controllers
{   
    [Route("[controller]")]
    [ApiController]
    public class CreateAlbumController : Controller
    {
        
        public async Task<IActionResult> AddAlbum(CreateAlbumViewModel model)
        {

            return Ok();
        }
    }
}

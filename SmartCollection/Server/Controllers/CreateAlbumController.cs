using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartCollection.DataAccess.RepositoryPattern;
using SmartCollection.Models.DBModels;
using SmartCollection.Models.ViewModels.CreateAlbumViewModel;
using SmartCollection.Utilities.AlbumCreator;
using SmartCollection.Utilities.DatabaseInitializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartCollection.Server.Controllers
{
      [Route("[controller]")]
      [ApiController]
      [Authorize]
      public class CreateAlbumController : Controller
      {
          private readonly UserManager<IdentityUser> _userManager;
          private readonly IAlbumCreator<CreateAlbumViewModel, IUnitOfWork> _albumCreator;
          private readonly IUnitOfWork _unitOfWork;

          public CreateAlbumController(
              UserManager<IdentityUser> userManager,
              IAlbumCreator<CreateAlbumViewModel, IUnitOfWork> albumCreator,
              IUnitOfWork unitOfWork)
          {
              _userManager = userManager;
              _albumCreator = albumCreator;
              _unitOfWork = unitOfWork;
          }


        public async Task<IActionResult> AddAlbum(CreateAlbumViewModel model)
        {
            IdentityUser user = await _userManager.FindByNameAsync(User.Identity.Name);

            var ownedAlbums = _unitOfWork.Albums.Find(a => a.UserId.Equals(user.Id));

            foreach(var album in ownedAlbums)
            {
                if(album.Name.Equals(model.Name))
                {
                    return BadRequest();
                }
            }

            await _albumCreator.CreateAsync(model, user.Id);

            return Ok();
        }
      }
}

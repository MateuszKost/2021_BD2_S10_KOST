using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartCollection.DataAccess.RepositoryPattern;
using SmartCollection.Models.DBModels;
using SmartCollection.Models.ViewModels.AlbumViewModel;
using SmartCollection.Models.ViewModels.CreateAlbumViewModel;
using SmartCollection.Server.User;
using SmartCollection.Utilities.AlbumCreator;
using SmartCollection.Utilities.DatabaseInitializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartCollection.Server.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class CreateAlbumController : Controller
    {
        private readonly IAlbumCreator<CreateAlbumViewModel, IUnitOfWork> _albumCreator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;

        public CreateAlbumController(
              ICurrentUser currentUser,
              IAlbumCreator<CreateAlbumViewModel, IUnitOfWork> albumCreator,
              IUnitOfWork unitOfWork)
        {
            _currentUser = currentUser;
            _albumCreator = albumCreator;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddAlbum(CreateAlbumViewModel model)
        {
            var userId = _currentUser.UserId;
            var ownedAlbums = _unitOfWork.Albums.Find(a => a.UserId.Equals(userId));

            foreach (var album in ownedAlbums)
            {
                if (album.Name.Equals(model.Name))
                {
                    return BadRequest();
                }
            }

            await _albumCreator.CreateAsync(model, userId);

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAlbum(SingleAlbumViewModel album)
        {
            var result = _unitOfWork.Albums.Find(a => a.AlbumId == album.AlbumId).FirstOrDefault();

            if (result != null)
            {
                _unitOfWork.Albums.Remove(result);
                var imagesAlbums = _unitOfWork.ImagesAlbums.Find(ia => ia.AlbumsAlbumId == result.AlbumId).ToList();

                if (imagesAlbums.Any())
                {
                    _unitOfWork.ImagesAlbums.RemoveRange(imagesAlbums);
                }
            }

            return Ok();
        }
    }
}

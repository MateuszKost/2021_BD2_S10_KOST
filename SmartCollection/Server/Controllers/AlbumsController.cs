using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartCollection.DataAccess.RepositoryPattern;
using SmartCollection.Models.ViewModels.AlbumViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SmartCollection.Server.Controllers
{
    
    [Route("[controller]")]
    [ApiController]
    public class AlbumsController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        public AlbumsController(UserManager<IdentityUser> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        //[Authorize]
        [Route("")]
        [HttpGet]
        public async Task<AlbumViewModel> GetAlbums()
        {
            var userId = _userManager.GetUserId(User);

            var albumList = _unitOfWork.Albums.Find(x => x.UserId.Equals(userId)).ToList();

            List<SingleAlbumViewModel> albumViewModelList = new();

            albumViewModelList.Add(new SingleAlbumViewModel
            {
                AlbumId = 0,
                AlbumName = "All Images",
                ImagesCount = _unitOfWork.Images.Find(image => image.UserId.Equals(userId)).ToList().Count,
                AlbumCoverPicture = "no cover yet"
            });

            if (albumList != null && albumList.Any())
            {

                foreach (var album in albumList)
                {
                    SingleAlbumViewModel singleAlbumView = new SingleAlbumViewModel
                    {
                        AlbumId = album.AlbumId,
                        ImagesCount = _unitOfWork.ImagesAlbums.Find(image => image.AlbumsAlbumId == album.AlbumId).ToList().Count,
                        AlbumName = album.Name,
                        AlbumCoverPicture = "no cover yet"
                    };

                    albumViewModelList.Add(singleAlbumView);
                }
            }

            AlbumViewModel albumViewModel = new();
            albumViewModel.AlbumViewModelList = albumViewModelList;

            return albumViewModel;
        }
    }
}

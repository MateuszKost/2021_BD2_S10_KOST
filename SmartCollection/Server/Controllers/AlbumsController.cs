using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartCollection.DataAccess.RepositoryPattern;
using SmartCollection.Models.ViewModels.AlbumViewModel;
using SmartCollection.Server.User;
using SmartCollection.StorageManager.Containers;
using SmartCollection.StorageManager.Context;
using SmartCollection.Utilities.ImageConverter;
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
        private readonly ICurrentUser _currentUser;
        private readonly IUnitOfWork _unitOfWork;

        //TODO UTILITY
        private readonly IStorageContext<IStorageContainer> _storageContext;
        private readonly IImageConverter _imageConverter;

        public AlbumsController(ICurrentUser currentUser, 
            IUnitOfWork unitOfWork, 
            IStorageContext<IStorageContainer> storageContext,
            IImageConverter imageConverter)
        {
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
            _storageContext = storageContext;
            _imageConverter = imageConverter;
        }

        [HttpGet]
        [Route("getalbum/{id}")]
        public async Task<SingleAlbumViewModel> GetAlbumById(
        [FromRoute] int id)
        {
            var album = await _unitOfWork.Albums.GetAsync(id);

            if(album == null)
                return new SingleAlbumViewModel();

            var privacy = await _unitOfWork.Privacies.GetAsync((int)album.PrivacyId);

            var imageId = _unitOfWork.ImagesAlbums.Find(ia => ia.AlbumsAlbumId == album.AlbumId)
                        .First()
                        .ImagesAlbumId;

            var coverImage = (imageId != null) ? await _unitOfWork.Images.GetAsync((int)imageId) : null;
            var coverImageBytes = (coverImage != null) ? await _storageContext.GetAsync(new ImageContainer(), coverImage.ImageSha1) : null;
            var coverImageBase64 = (coverImageBytes != null) ? _imageConverter.ImageBytesToBase64(coverImageBytes) : null;

            return new SingleAlbumViewModel()
            {
                AlbumName = album.Name,
                AlbumId = id,
                Description = album.Description,
                ImagesCount = _unitOfWork.ImagesAlbums.Find(image => image.AlbumsAlbumId == album.AlbumId).ToList().Count,
                IsPublic = privacy.Name.Equals("public"),
                AlbumCoverPicture = coverImageBase64
            };

        }

        //[Authorize]
        [HttpGet]
        [Route("")]
        public async Task<AlbumViewModel> GetAlbums()
        {
            var userId = _currentUser.UserId;
            var albumList = _unitOfWork.Albums.Find(x => x.UserId.Equals(userId)).ToList();

            List<SingleAlbumViewModel> albumViewModelList = new();

            albumViewModelList.Add(new SingleAlbumViewModel
            {
                AlbumId = 0,
                AlbumName = "All Images",
                ImagesCount = _unitOfWork.Images.Find(image => image.UserId.Equals(userId)).ToList().Count,
                Description = "All your images",
                IsPublic = false,
                AlbumCoverPicture = null
            });

            if (albumList != null && albumList.Any())
            {
                foreach (var album in albumList)
                {
                    SingleAlbumViewModel singleAlbumViewModel = await GetAlbumById(album.AlbumId);
                    albumViewModelList.Add(singleAlbumViewModel);
                }
            }

            AlbumViewModel albumViewModel = new();
            albumViewModel.AlbumViewModelList = albumViewModelList;

            return albumViewModel;
        }

        [HttpPost]
        [Route("edit")]
        public async Task<IActionResult> EditAlbum(SingleAlbumViewModel albumModel)
        {
            var album = await _unitOfWork.Albums.GetAsync(albumModel.AlbumId);
            
            var privacy = albumModel.IsPublic ? 
                _unitOfWork.Privacies.Find(p => p.Name == "public").ToList().FirstOrDefault()
                :
                _unitOfWork.Privacies.Find(p => p.Name == "private").ToList().FirstOrDefault();

            if (album == null)
            {
                return BadRequest();
            }

            album.Name = albumModel.AlbumName;
            album.Description = albumModel.Description;
            album.Privacy = privacy;
            album.PrivacyId = privacy.PrivacyId;

            _unitOfWork.Albums.Update(album);
            _unitOfWork.Save();

            return Ok();
        }

        [HttpDelete]
        [Route("delete/{albumId}")]
        public async Task<IActionResult> DeleteAlbum(int albumId)
        {
            var album = await _unitOfWork.Albums.GetAsync(albumId);
            if (album == null) return BadRequest();

            _unitOfWork.Albums.Remove(album);
            _unitOfWork.Save();

            return Ok();
        }
    }
}

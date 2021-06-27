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
                AlbumCoverPicture = null
            });

            if (albumList != null && albumList.Any())
            {
               
                foreach (var album in albumList)
                {
                    var imageId = _unitOfWork.ImagesAlbums.Find(ia => ia.AlbumsAlbumId == album.AlbumId)
                        .First()
                        .ImagesAlbumId;

                    var coverImage = (imageId != null) ? await _unitOfWork.Images.GetAsync((int)imageId) : null;
                    var coverImageBytes = (coverImage != null) ? await _storageContext.GetAsync(new ImageContainer(), coverImage.ImageSha1) : null;
                    var coverImageBase64 = (coverImageBytes != null) ? _imageConverter.ImageBytesToBase64(coverImageBytes) : null;

                    SingleAlbumViewModel singleAlbumView = new SingleAlbumViewModel
                    {
                        AlbumId = album.AlbumId,
                        ImagesCount = _unitOfWork.ImagesAlbums.Find(image => image.AlbumsAlbumId == album.AlbumId).ToList().Count,
                        AlbumName = album.Name,
                        AlbumCoverPicture = coverImageBase64 ?? null
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

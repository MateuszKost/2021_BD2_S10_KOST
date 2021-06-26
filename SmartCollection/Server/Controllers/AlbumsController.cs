﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartCollection.DataAccess.RepositoryPattern;
using SmartCollection.Models.ViewModels.AlbumViewModel;
using SmartCollection.Server.User;
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

        public AlbumsController(ICurrentUser currentUser, IUnitOfWork unitOfWork)
        {
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
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

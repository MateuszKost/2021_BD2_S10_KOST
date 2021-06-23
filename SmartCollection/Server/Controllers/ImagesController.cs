using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartCollection.DataAccess.Context;
using SmartCollection.DataAccess.RepositoryPattern;
using SmartCollection.Models.ViewModels.ImagesViewModel;
using SmartCollection.StorageManager.Containers;
using SmartCollection.StorageManager.Context;
using SmartCollection.Utilities.HashGenerator;
using SmartCollection.Utilities.ImageConverter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SmartCollection.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImagesController : ControllerBase
    {
        private readonly ILogger<ImagesController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStorageContext<IStorageContainer> _storageContext;
        private readonly IHashGenerator _hashGenerator;
        private readonly IImageConverter _imageConverter;
        private readonly UserManager<IdentityUser> _userManager;

        public ImagesController(ILogger<ImagesController> logger,
            IUnitOfWork unitOfWork,
            IStorageContext<IStorageContainer> storageContext,
            IHashGenerator hashGenerator,
            IImageConverter imageConverter,
            UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _storageContext = storageContext;
            _hashGenerator = hashGenerator;
            _imageConverter = imageConverter;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("all")]
        public async Task<ImagesViewModel> GetAllImages()
        {
            var userId = _userManager.GetUserId(User);

            // list of images from db
            var imagesList = _unitOfWork.Images.Find(image => image.UserId.Equals(userId)).ToList();
            List<SingleImageViewModel> imageViewModelList = new();

            if(imagesList.Any())
            {
                foreach(var image in imagesList)
                {
                    var imageDetails = _unitOfWork.ImageDetails.Find(details => details.ImageId == image.ImageId).FirstOrDefault();

                    // get file from blob by its hash
                    byte[] imageFile = await _storageContext.GetAsync(new ImageContainer(), image.ImageSha1);

                    // get file from blob by its name
                    //byte[] imageFile = await _storageContext.GetAsync(new ImageContainer(), imageDetails.Name);

                    // get file from blob by its OriginalName
                    //byte[] imageFile = await _storageContext.GetAsync(new ImageContainer(), imageDetails.OriginalName);

                    SingleImageViewModel singleImageViewModel = new SingleImageViewModel
                    {
                        Id = image.ImageId,
                        Name = imageDetails.Name,
                        Description = imageDetails.Description,
                        Date = imageDetails.Date.ToString(),
                        Data = Convert.ToBase64String(imageFile)
                    };

                    imageViewModelList.Add(singleImageViewModel);
                }

                return new ImagesViewModel { Images = imageViewModelList };
            }

            return null;
        }

        [HttpGet]
        [Route("getimages/{albumId}")]
        public async Task<ImagesViewModel> GetImagesFromAlbum(int albumId)
        {
            if (albumId == 0)
            {
                return await GetAllImages() ?? new ImagesViewModel();
            }

            // list of images from db, from specified album
            var imageAlbums = _unitOfWork.ImagesAlbums.Find(ia => ia.AlbumsAlbumId == albumId).ToList();
            List<SingleImageViewModel> imagesViewModelList = new();

            if (imageAlbums.Any())
            {
                foreach (var ia in imageAlbums)
                {
                    var image = _unitOfWork.Images.Find(image => image.ImageId == ia.ImagesAlbumId).FirstOrDefault();
                    var imageDetails = _unitOfWork.ImageDetails.Find(details => details.ImageId == ia.ImagesAlbumId).FirstOrDefault();

                    // get file from blob by its hash
                    byte[] imageFile = await _storageContext.GetAsync(new ImageContainer(), image.ImageSha1);

                    // get file from blob by its name
                    //byte[] imageFile = await _storageContext.GetAsync(new ImageContainer(), imageDetails.Name);

                    // get file from blob by its OriginalName
                    //byte[] imageFile = await _storageContext.GetAsync(new ImageContainer(), imageDetails.OriginalName);

                    SingleImageViewModel singleImageViewModel = new SingleImageViewModel
                    {
                        Id = image.ImageId,
                        Name = imageDetails.Name,
                        Description = imageDetails.Description,
                        Date = imageDetails.Date.ToString(),
                        Data = Convert.ToBase64String(imageFile)
                    };

                    imagesViewModelList.Add(singleImageViewModel);
                }

                return new ImagesViewModel { Images = imagesViewModelList };
            }

            return null;
        }

        [HttpGet]
        [Route("test")]
        public async Task<ImagesViewModel> Get()
        {
            //getting data from db
            var imageName = _unitOfWork.ImageDetails.GetAll().FirstOrDefault();
            //example view model
            List<SingleImageViewModel> images = new List<SingleImageViewModel>();

            var singleImage = new SingleImageViewModel()
            {
                Name = "MyPhoto",
                Date = "DateAsString",
                Description = "This is Description"
            };

            images.Add(singleImage);

            #region _storageContext example use
            //byte[] myFile = new byte[420];
            //  _storageContext.AddAsync(new ImageContainer(), myFile, "fileFile").ConfigureAwait(false);

            //byte[] receivedFile = await _storageContext.GetAsync(new ImageContainer(), "fileFile").ConfigureAwait(false);

            //_storageContext.DeleteAsync(new ImageContainer(), "fileFile");
            //byte[] receivedFile = await _storageContext.GetAsync(new ImageContainer(), "fileFile").ConfigureAwait(false);
            #endregion

            return new ImagesViewModel() { Images = images };
        }

        [HttpPost]
        [Route("uploadimage")]
        public async Task<ActionResult> UploadImage(SingleImageViewModel image)
        {
            byte[] imageFile = _imageConverter.Base64ToImage(image.Data);

            Models.DBModels.Album album = null;

            if (image.AlbumId != 0 && image.AlbumId != null)
            {
                album = new();
                album = _unitOfWork.Albums.Find(album => album.AlbumId == image.AlbumId).FirstOrDefault();
            }

            try
            {
                Models.DBModels.Image imageModel = new Models.DBModels.Image
                {
                    UserId = _userManager.GetUserId(User),
                    Album = album,
                    //AlbumId = albumId ?? 0,// if entityframework creates id by Album, delete it, if not, then uncomment
                    ImageSha1 = _hashGenerator.GetHash(imageFile),
                };

                _unitOfWork.Images.AddAsync(imageModel);

                Models.DBModels.ImageDetail imageDetails = new Models.DBModels.ImageDetail
                {
                    Date = DateTime.Now,
                    Description = image.Description,
                    Name = image.Name,
                    Image = imageModel,
                    //ImageId = imageModel.ImageId // // if entityframework creates id by Image, delete it, if not, then uncomment
                };

                _unitOfWork.ImageDetails.AddAsync(imageDetails);

                // relation: check if entityframework does it itself
                //if(albumId != 0 && albumId != null) _unitOfWork.ImagesAlbums.AddAsync(
                //    new Models.DBModels.ImageAlbum { 
                //    AlbumsAlbumId = albumId, 
                //    ImagesAlbumId = image.Id, 
                //    AlbumsAlbum = album,
                //    ImagesAlbumNavigation = imageModel
                //});

                // add to blob by hash
                _storageContext.AddAsync(new ImageContainer(), imageFile, imageModel.ImageSha1);

                return Ok();
            }
            catch
            {
                Console.WriteLine("Error during uploading image occured");
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("uploadimages")]
        public async Task<IActionResult> UploadImages([FromBody]ImagesViewModel images)
        {
            if(images != null && images.Images.Any())
            {
                foreach(var image in images.Images)
                {
                    var result = await UploadImage(image) as StatusCodeResult;
                    if (result.StatusCode == 400) return BadRequest();
                }
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost]
        [Route("deleteimage")]
        public async Task<IActionResult> DeleteImage(SingleImageViewModel imageViewModel)
        {
            var result = _unitOfWork.Images.Find(image => image.ImageId == imageViewModel.Id).FirstOrDefault();

            if(result != null)
            {
                _unitOfWork.Images.Remove(result);
                _storageContext.DeleteAsync(new ImageContainer(), result.ImageSha1);
                return Ok();
            }
            else
            {
                Console.WriteLine("No image found");
                return BadRequest();
            }

        }

        [HttpPost] // [HttpDelete]
        [Route("deletefromalbum")]
        public async Task<IActionResult> DeleteImageFromAlbum(SingleImageViewModel image)
        {
            //check if exists
            var result = _unitOfWork.ImagesAlbums
                .Find(ia => ia.ImagesAlbumId == image.Id && ia.ImagesAlbumId == image.AlbumId)
                .FirstOrDefault();

            if(result != null)
            {
                _unitOfWork.ImagesAlbums.Remove(result);
                return Ok();
            }
            else
            {
                Console.WriteLine("No image found");
                return BadRequest();
            }
        }

    }
}

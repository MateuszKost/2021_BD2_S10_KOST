using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartCollection.DataAccess.Context;
using SmartCollection.DataAccess.RepositoryPattern;
using SmartCollection.Models.ViewModels.ImagesViewModel;
using SmartCollection.StorageManager.Containers;
using SmartCollection.StorageManager.Context;
using SmartCollection.Utilities.HashGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly UserManager<IdentityUser> _userManager;

        public ImagesController(ILogger<ImagesController> logger,
            IUnitOfWork unitOfWork,
            IStorageContext<IStorageContainer> storageContext,
            IHashGenerator hashGenerator,
            UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _storageContext = storageContext;
            _hashGenerator = hashGenerator;
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

            if(imagesList != null && imagesList.Any())
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
                        Data = Convert.ToBase64String(imageFile, 0, imageFile.Length)
                    };

                    imageViewModelList.Add(singleImageViewModel);
                }

                return new ImagesViewModel { Images = imageViewModelList };
            }

            return null;
        }

        [HttpGet]
        [Route("albumimages")]
        public async Task<ImagesViewModel> GetImagesFromAlbum(int albumId)
        {
            var userId = _userManager.GetUserId(User);

            // list of images from db, from specified album

            var imageAlbums = _unitOfWork.ImagesAlbums.Find(ia => ia.AlbumsAlbumId == albumId).ToList();
            List<SingleImageViewModel> imagesViewModelList = new();

            if(imageAlbums != null && imageAlbums.Any())
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
                        Data = Convert.ToBase64String(imageFile, 0, imageFile.Length)
                    };

                    imagesViewModelList.Add(singleImageViewModel);
                }

                ImagesViewModel imagesViewModel = new ImagesViewModel
                {
                    Images = imagesViewModelList
                };

                return imagesViewModel;

            }

            return null;
        }

        [HttpGet]
        public async Task<ActionResult<ImagesViewModel>> Get()
        {
            //getting data from db
            var imageName = _unitOfWork.ImageDetails.GetAll().FirstOrDefault();
            //example view model
            List<SingleImageViewModel> images = new List<SingleImageViewModel>();

            var singleImage  = new SingleImageViewModel() { 
            Name  = "MyPhoto",
            Date = "DateAsString",
            Description = "This is Description"};

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

        private void ImageExampleDeleteIt(byte[] myImage)
        {
           string myHashCode = _hashGenerator.GetHash(myImage);
            
            
           _storageContext.AddAsync(new ImageContainer(), myImage, myHashCode);

            if (_storageContext.GetAsync(new ImageContainer(), myHashCode) == null)
                Console.WriteLine("Error, no image in blob");
        }
    }
}

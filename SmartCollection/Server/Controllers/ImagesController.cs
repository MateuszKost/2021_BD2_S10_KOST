using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartCollection.DataAccess.RepositoryPattern;
using SmartCollection.Models.ViewModels.ImagesViewModel;
using SmartCollection.Server.User;
using SmartCollection.StorageManager.Containers;
using SmartCollection.StorageManager.Context;
using SmartCollection.Utilities.HashGenerator;
using SmartCollection.Utilities.ImageConverter;
using SmartCollection.Utilities.TagManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartCollection.Server.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class ImagesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStorageContext<IStorageContainer> _storageContext;
        private readonly IHashGenerator _hashGenerator;
        private readonly IImageConverter _imageConverter;
        private readonly ICurrentUser _currentUser;
        private readonly ITagManager _tagManager;

        public ImagesController(IUnitOfWork unitOfWork,
            IStorageContext<IStorageContainer> storageContext,
            IHashGenerator hashGenerator,
            IImageConverter imageConverter,
            ICurrentUser currentUser,
            ITagManager tagManager)
        {
            _unitOfWork = unitOfWork;
            _storageContext = storageContext;
            _hashGenerator = hashGenerator;
            _imageConverter = imageConverter;
            _currentUser = currentUser;
            _tagManager = tagManager;
        }

        [HttpGet]
        [Route("all")]
        public async Task<ImagesViewModel> GetAllImages()
        {
            var userId = _currentUser.UserId;

            // list of images from db
            var imagesList = _unitOfWork.Images.Find(image => image.UserId.Equals(userId)).ToList();
            List<SingleImageViewModel> imageViewModelList = new();

            if (imagesList.Any())
            {
                foreach (var image in imagesList)
                {
                    var imageDetails = _unitOfWork.ImageDetails.Find(details => details.ImageId == image.ImageId).FirstOrDefault();

                    // get file from blob by its hash
                    byte[] imageFile = await _storageContext.GetAsync(new ImageContainer(), image.ImageSha1);

                    SingleImageViewModel singleImageViewModel = new SingleImageViewModel()
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

            return new ImagesViewModel();
        }

        [HttpGet]
        [Route("getimages/{albumId}")]
        public async Task<ImagesViewModel> GetImagesFromAlbum([FromRoute] int albumId)
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
            return new ImagesViewModel();
        }

        [HttpGet]
        [Route("filter")]
        public async Task<IActionResult> FilterImages(FilterImagesViewModel filterModel)
        {
            return Ok();
        }

        [HttpGet]
        [Route("getimage/{id}")]
        public async Task<ActionResult<SingleImageViewModel>> GetImageById([FromRoute] int id)
        {
            var image = await _unitOfWork.Images.GetAsync(id);
            if (image.UserId != _currentUser.UserId)
            {
                return Forbid();
            }
            if (image != null)
            {
                var imageDetails = _unitOfWork.ImageDetails.Find(details => details.ImageId == id).First();

                if (imageDetails != null)
                {
                    try
                    {
                        var file = await _storageContext.GetAsync(new ImageContainer(), image.ImageSha1);
                        var base64 = _imageConverter.ImageBytesToBase64(file);

                        var tags = _tagManager.GetTags(id);

                        return Ok(new SingleImageViewModel()
                        {
                            Id = id,
                            Name = imageDetails.Name,
                            Description = imageDetails.Description,
                            Date = imageDetails.Date.ToString(),
                            Data = base64,
                            Tags = tags.Select(x => x.Name)
                        });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return NotFound(ex.Message);
                    }
                }
            }
            return new SingleImageViewModel();
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

            #endregion _storageContext example use

            return new ImagesViewModel() { Images = images };
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> UpdateImage(SingleImageViewModel imageModel)
        {
            var imageDetails = _unitOfWork.ImageDetails.Find(d => d.ImageId == imageModel.Id).First();

            if (imageDetails == null) return BadRequest();

            imageDetails.Name = imageModel.Name;
            imageDetails.Description = imageModel.Description;
            imageDetails.Date = Convert.ToDateTime(imageModel.Date);

            var insertedTagsIds =_tagManager.InsertTags(imageModel.Tags);
            _tagManager.UpsertImageTags(insertedTagsIds, imageModel.Id);

            try
            {
                _unitOfWork.ImageDetails.Update(imageDetails);
                _unitOfWork.Save();

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest();
            }
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
                    UserId = _currentUser.UserId,
                    Album = album,
                    //AlbumId = albumId ?? 0,// if entityframework creates id by Album, delete it, if not, then uncomment
                    ImageSha1 = _hashGenerator.GetHash(imageFile),
                };

                _unitOfWork.Images.AddAsync(imageModel);
                _unitOfWork.Save();

                Models.DBModels.ImageDetail imageDetails = new Models.DBModels.ImageDetail
                {
                    Date = DateTime.Now,
                    Description = image.Description,
                    Name = image.Name,
                    Image = imageModel,
                    //ImageId = imageModel.ImageId // // if entityframework creates id by Image, delete it, if not, then uncomment
                };

                _unitOfWork.ImageDetails.AddAsync(imageDetails);
                _unitOfWork.Save();

                // relation: check if entityframework does it itself
                if (album != null && album.AlbumId != 0) _unitOfWork.ImagesAlbums.AddAsync(
                     new Models.DBModels.ImageAlbum
                     {
                         AlbumsAlbumId = album.AlbumId,
                         ImagesAlbumId = image.Id,
                         AlbumsAlbum = album,
                         ImagesAlbumNavigation = imageModel
                     });

                _unitOfWork.Save();

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
        public async Task<IActionResult> UploadImages(ImagesViewModel images)
        {
            if (images != null && images.Images.Any())
            {
                foreach (var image in images.Images)
                {
                    var result = await UploadImage(image) as StatusCodeResult;
                    if (result.StatusCode == 400) return BadRequest();
                }
                return Ok();
            }

            return BadRequest();
        }

        [HttpDelete]
        [Route("deleteimage/{imageId}")]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            var result = _unitOfWork.Images.Find(image => image.ImageId == imageId).FirstOrDefault();
            // TODO DELETE TAGS !

            if (result != null)
            {
                _unitOfWork.Images.Remove(result);
                _unitOfWork.Save();
                _storageContext.DeleteAsync(new ImageContainer(), new[] { result.ImageSha1 });
                return Ok();
            }
            else
            {
                Console.WriteLine("No image found");
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("deletefromalbum/{albumId}/{imageId}")]
        public async Task<IActionResult> DeleteImageFromAlbum(int albumId, int imageId)
        {
            //check if exists
            var result = _unitOfWork.ImagesAlbums
                .Find(ia => ia.ImagesAlbumId == imageId && ia.ImagesAlbumId == albumId)
                .FirstOrDefault();

            if (result != null)
            {
                _unitOfWork.ImagesAlbums.Remove(result);
                _unitOfWork.Save();
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
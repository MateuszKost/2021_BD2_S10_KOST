using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartCollection.DataAccess.Context;
using SmartCollection.DataAccess.RepositoryPattern;
using SmartCollection.Models.ViewModels.ImagesViewModel;
using SmartCollection.StorageManager.Containers;
using SmartCollection.StorageManager.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartCollection.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ImagesController : ControllerBase
    {
        private readonly ILogger<ImagesController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStorageContext<IStorageContainer> _storageContext;

        public ImagesController(ILogger<ImagesController> logger, IUnitOfWork unitOfWork,IStorageContext<IStorageContainer> storageContext)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _storageContext = storageContext;
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
    }
}

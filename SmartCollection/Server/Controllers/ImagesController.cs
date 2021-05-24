using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartCollection.DataAccess.Context;
using SmartCollection.DataAccess.RepositoryPattern;
using SmartCollection.Models.ViewModels.ImagesViewModel;
using SmartCollection.Shared;
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

        public ImagesController(ILogger<ImagesController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
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

            return new ImagesViewModel() { Images = images };
        }
    }
}

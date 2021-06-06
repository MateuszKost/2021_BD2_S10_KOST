using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartCollection.DataAccess.RepositoryPattern;
using SmartCollection.Models.DBModels;
using SmartCollection.Models.ViewModels.CreateAlbumViewModel;
using SmartCollection.Utilities.DatabaseInitializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartCollection.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CreateAlbumController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        public CreateAlbumController(
            IUnitOfWork unitOfWork,
            UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<IActionResult> AddAlbum(CreateAlbumViewModel model)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
           
            Privacy privacy = null;

            if (model.PrivacyType)
                privacy = _unitOfWork.Privacies.Find(p => p.Name == "public").ToList().FirstOrDefault();
            else
                privacy = _unitOfWork.Privacies.Find(p => p.Name == "private").ToList().FirstOrDefault();

            if (privacy != null)
            {
                _unitOfWork.Albums.AddAsync(new Album()
                {

                    Description = model.Brief,
                    Name = model.Name,
                    Privacy = privacy,
                    UserId = user.Id,
                    Images = null,
                    PrivacyId = privacy.PrivacyId

                });

                _unitOfWork.Save();
            }
            return Ok();
        }
    }
}

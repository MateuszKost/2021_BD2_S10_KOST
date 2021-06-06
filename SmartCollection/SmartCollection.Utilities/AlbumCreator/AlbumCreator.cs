using SmartCollection.DataAccess.RepositoryPattern;
using SmartCollection.Models.DBModels;
using SmartCollection.Models.ViewModels.CreateAlbumViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCollection.Utilities.AlbumCreator
{
    public class AlbumCreator : IAlbumCreator<CreateAlbumViewModel,IUnitOfWork>
    {
        private readonly IUnitOfWork _unitOfWork;
        public AlbumCreator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        private async Task<Privacy> GetPrivacyAsync(bool isPublic)
        {
            Privacy privacy = null;

            if (isPublic)
                privacy = _unitOfWork.Privacies.Find(p => p.Name == "public").ToList().FirstOrDefault();
            else
                privacy = _unitOfWork.Privacies.Find(p => p.Name == "private").ToList().FirstOrDefault();

            return privacy;
        }
        public async Task<bool> CreateAsync(CreateAlbumViewModel model, string userId)
        {

            Privacy privacy = await GetPrivacyAsync(model.PrivacyType);

            if (privacy != null)
            {
                _unitOfWork.Albums.AddAsync(new Album()
                {

                    Description = model.Brief,
                    Name = model.Name,
                    Privacy = privacy,
                    UserId = userId,
                    Images = null,
                    PrivacyId = privacy.PrivacyId

                });

                if (_unitOfWork.Save() > 0)
                    return true;
            }

            return false;
        }
    }
}

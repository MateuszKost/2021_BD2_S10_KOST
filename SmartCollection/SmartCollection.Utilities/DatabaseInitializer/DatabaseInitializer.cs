using SmartCollection.DataAccess.RepositoryPattern;
using SmartCollection.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCollection.Utilities.DatabaseInitializer
{
    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly IUnitOfWork _unitOfWork;

        public DatabaseInitializer(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private Task InitializePrivacy()
        {
            if (_unitOfWork.Privacies.GetAll().ToList().Count > 0)
                return Task.CompletedTask;
           
            _unitOfWork.Privacies.AddAsync(new Privacy()
            {
                Name = "public",
                Albums = null
            });

            _unitOfWork.Privacies.AddAsync(new Privacy()
            {
                Name = "private",
                Albums = null
            });

            _unitOfWork.Save();

            return Task.CompletedTask;
        }

        public async Task InitiazlizeDatabase()
        {
            await InitializePrivacy();

            return;
        }
    }
}

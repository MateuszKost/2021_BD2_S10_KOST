using SmartCollection.DataAccess.Context;
using SmartCollection.DataAccess.RepositoryPattern.Content.Interfaces;
using SmartCollection.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCollection.DataAccess.RepositoryPattern.Content.Class
{
    public class ImagesTagRepository : Repository<ImageTag>, IImagesTagRepository
    {
        public ImagesTagRepository(SmartCollectionDbContext db) : base(db)
        {

        }
    }
}

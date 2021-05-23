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
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(SmartCollectionDbContext db) : base(db)
        {

        }
    }
}

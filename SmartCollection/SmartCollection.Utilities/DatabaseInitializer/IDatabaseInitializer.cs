using SmartCollection.DataAccess.RepositoryPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCollection.Utilities.DatabaseInitializer
{
    public interface IDatabaseInitializer
    {
        public Task InitiazlizeDatabase();
    }
}

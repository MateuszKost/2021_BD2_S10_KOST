using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCollection.Utilities.AlbumCreator
{
    public interface IAlbumCreator<TModel,TUnitOfWork> 
        where TModel : class
        where TUnitOfWork : IDisposable
    {
        public Task<bool> CreateAsync(TModel model,string userId);
    }
}

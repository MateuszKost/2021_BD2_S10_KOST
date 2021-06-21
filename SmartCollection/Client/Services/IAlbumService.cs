using SmartCollection.Models.ViewModels.AlbumViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartCollection.Client.Services
{
    public interface IAlbumService
    {
        public Task<IEnumerable<SingleAlbumViewModel>> GetAlbums();
    }
}

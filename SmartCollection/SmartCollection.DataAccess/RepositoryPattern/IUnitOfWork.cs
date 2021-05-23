using SmartCollection.DataAccess.RepositoryPattern.Content.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCollection.DataAccess.RepositoryPattern
{
    public interface IUnitOfWork : IDisposable
    {
        IAlbumRepository Albums { get; }
        IImageDetailRepository ImageDetails { get; }
        IImageRepository Images { get; }
        IImagesAlbumRepository ImagesAlbums { get; }
        IPrivacyRepository Privacies { get; }
        ITagOrderRepository TagOrders { get; }
        ITagRepository Tags { get; }
        IUserCredentialRepository UserCredentials { get; }
        IUserRepository Users { get; }
        IUsersAlbumRepository UsersAlbums { get; }
        int Save();
    }
}

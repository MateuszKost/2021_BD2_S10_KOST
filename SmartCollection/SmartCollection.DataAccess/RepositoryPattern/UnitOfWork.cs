using SmartCollection.DataAccess.Context;
using SmartCollection.DataAccess.RepositoryPattern.Content.Class;
using SmartCollection.DataAccess.RepositoryPattern.Content.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCollection.DataAccess.RepositoryPattern
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SmartCollectionDbContext _db;

        public IAlbumRepository Albums { get; private set; }
        public IImageDetailRepository ImageDetails { get; private set; }
        public IImageRepository Images { get; private set; }
        public IImagesAlbumRepository ImagesAlbums { get; private set; }
        public IPrivacyRepository Privacies { get; private set; }
        public ITagRepository Tags { get; private set; }
        public IApplicationUserRepository ApplicationUsers { get; private set; }
        public IImagesTagRepository ImagesTags { get; private set; }

        public UnitOfWork(SmartCollectionDbContext db)
        {
            _db = db;
            Albums = new AlbumRepository(db);
            ImageDetails = new ImageDetailRepository(db);
            Images = new ImageRepository(db);
            ImagesAlbums = new ImagesAlbumRepository(db);
            Privacies = new PrivacyRepository(db);
            Tags = new TagRepository(db);
            ApplicationUsers = new ApplicationUserRepository(db);
            ImagesTags = new ImagesTagRepository(db);

        }
        public int Save()
        {
            return _db.SaveChanges();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}

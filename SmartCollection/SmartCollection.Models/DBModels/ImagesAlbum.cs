using System;
using System.Collections.Generic;

#nullable disable

namespace SmartCollection.Models.DBModels
{
    public partial class ImagesAlbum
    {
        public int? ImagesAlbumId { get; set; }
        public int? AlbumsAlbumId { get; set; }

        public virtual Album AlbumsAlbum { get; set; }
        public virtual Image ImagesAlbumNavigation { get; set; }
    }
}

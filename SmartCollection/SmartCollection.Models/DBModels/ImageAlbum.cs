using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace SmartCollection.Models.DBModels
{
    public partial class ImageAlbum
    {
        //[Key, Column(Order = 0)]
        public int? ImagesAlbumId { get; set; }
        //[Key, Column(Order = 1)]
        public int? AlbumsAlbumId { get; set; }

        public virtual Album AlbumsAlbum { get; set; }
        public virtual Image ImagesAlbumNavigation { get; set; }
    }
}

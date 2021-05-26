using System;
using System.Collections.Generic;

#nullable disable

namespace SmartCollection.Models.DBModels
{
    public partial class Image
    {
        public int ImageId { get; set; }
        public int? AlbumId { get; set; }
        public string ImageSha1 { get; set; }

        public virtual Album Album { get; set; }
        public virtual ImageDetail ImageDetail { get; set; }
    }
}

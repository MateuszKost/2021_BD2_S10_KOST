using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCollection.Models.ViewModels.AlbumViewModel
{
    public class SingleAlbumViewModel
    {
        public string AlbumName { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        public string AlbumCoverPicture { get; set; }
        public int ImagesCount { get; set; }
        public int AlbumId { get; set; }
    }
}

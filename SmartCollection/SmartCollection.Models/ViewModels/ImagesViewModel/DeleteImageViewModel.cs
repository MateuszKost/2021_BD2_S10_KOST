using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCollection.Models.ViewModels.ImagesViewModel
{
    public class DeleteImageViewModel
    {
        public SingleImageViewModel ImageModel { get; set; }
        public int AlbumId { get; set; }
    }
}

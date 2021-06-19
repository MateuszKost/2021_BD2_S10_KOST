using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCollection.Models.ViewModels.AlbumViewModel
{
    public class AlbumViewModel
    {
        public IEnumerable<SingleAlbumViewModel> AlbumViewModelList { get; set; }
    }
}

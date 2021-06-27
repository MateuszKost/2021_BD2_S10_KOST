using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCollection.Models.ViewModels.ImagesViewModel
{
    public class FilterParameters
    {
        public int AlbumId { get; set; }
        public string ImageName { get; set; }
        public int TagId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}

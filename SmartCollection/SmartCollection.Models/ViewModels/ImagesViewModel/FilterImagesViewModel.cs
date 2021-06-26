using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCollection.Models.ViewModels.ImagesViewModel
{
    public class FilterImagesViewModel
    {
        public string? Name { get; set; }
        public int? TagId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}

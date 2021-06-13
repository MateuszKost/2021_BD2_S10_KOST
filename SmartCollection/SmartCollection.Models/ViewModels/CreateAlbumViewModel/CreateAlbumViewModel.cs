using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCollection.Models.ViewModels.CreateAlbumViewModel
{
    public class CreateAlbumViewModel
    {
        [Required(ErrorMessage = "Album name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Brief description is needed")]
        public string Brief { get; set; }
        public bool PrivacyType { get; set; }
    }
}

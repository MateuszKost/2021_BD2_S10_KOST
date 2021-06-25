using SmartCollection.Models.ViewModels.ImagesViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCollection.Utilities.ImageFilter
{
    public class ImageFilter : IImageFilter<ImagesViewModel>
    {
        public Task<ImagesViewModel> FiltrAsync(string dateSortType, string tagName, string imageName)
        {
            throw new NotImplementedException();
        }
    }
}

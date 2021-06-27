using SmartCollection.Models.DBModels;
using SmartCollection.Models.ViewModels.ImagesViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCollection.Utilities.ImageFilter
{
    public interface IImageFilter<T> where T : class
    {
        public Task<IEnumerable<T>> FilterAsync(FilterParameters filterParameters, List<Image> imagesList);
    }
}

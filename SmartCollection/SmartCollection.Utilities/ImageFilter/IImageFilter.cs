using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCollection.Utilities.ImageFilter
{
    public interface IImageFilter<T> where T : class
    {
        public Task<T> FiltrAsync(string dateSortType,string tagName, string imageName); 
    }
}

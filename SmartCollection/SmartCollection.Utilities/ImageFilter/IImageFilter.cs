using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCollection.Utilities.ImageFilter
{
    public interface IImageFilter<T> where T : class
    {
        public Task<IEnumerable<T>> FilterAsync(string userId, int tagId, string imageName,DateTime dateFrom,DateTime dateTo);
    }
}

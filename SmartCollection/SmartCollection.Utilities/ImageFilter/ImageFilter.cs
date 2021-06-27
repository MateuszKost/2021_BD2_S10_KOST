using SmartCollection.DataAccess.RepositoryPattern;
using SmartCollection.Models.DBModels;
using SmartCollection.Models.ViewModels.ImagesViewModel;
using SmartCollection.Utilities.StaticDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCollection.Utilities.ImageFilter
{
    public class ImageFilter : IImageFilter<ImageDetail>
    {
        private readonly IUnitOfWork _unitOfWork;
        public ImageFilter(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

        private List<Image> FilterByTag(int tagId, List<Image> images)
        {
            List<Image> byTagImages = new();

            //get images that store the tag
            var imageIdList = _unitOfWork.TagOrders.Find(
                p => p.TagId == tagId).Select(p => p.ImageId).ToList();

            foreach (var img in imageIdList)
                byTagImages.Add(images.Where(p => p.ImageId == img).FirstOrDefault());

            return byTagImages;

        }
        private List<ImageDetail> FilterByImageName(string imageName, List<ImageDetail> imagesDetailList)
        {
            return imagesDetailList
                  .Where(p => p.Name
                  .Contains(imageName, StringComparison.OrdinalIgnoreCase))
                  .ToList();
        }

        public async Task<IEnumerable<ImageDetail>> FilterAsync(FilterParameters filterParameters, List<Image> imagesList)
        {
            //get all user's images.
            var images = imagesList;

            if (filterParameters.TagId >= 0)
                images = FilterByTag((int)filterParameters.TagId, images);

            var imgDtList = new List<ImageDetail>();

            foreach (var img in images)
                imgDtList.Add(
                    _unitOfWork.ImageDetails.Find(p => p.ImageId == img.ImageId).FirstOrDefault());

            if (filterParameters.ImageName != null)
                    imgDtList = FilterByImageName(filterParameters.ImageName, imgDtList);

            if (filterParameters.DateFrom != default(DateTime) && filterParameters.DateTo != default(DateTime))
            {
                return imgDtList.Where(p => p.Date >= filterParameters.DateFrom && p.Date <= filterParameters.DateTo);
            }


            return imgDtList;
        }

      }
}

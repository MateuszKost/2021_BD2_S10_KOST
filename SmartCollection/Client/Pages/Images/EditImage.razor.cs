using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartCollection.Models.ViewModels.ImagesViewModel;
using SmartCollection.Models.DBModels;

namespace SmartCollection.Client.Pages.Images
{
    public partial class EditImage
    {
        [Parameter]
        public string ImageId { get; set; }

        private SingleImageViewModel image;
        protected override async Task OnInitializedAsync()
        {
            image = await ImageService.GetImage(int.Parse(ImageId));
            StateHasChanged();
        }

        // temp edit image

        string nName, nDescription, ntagsString;
        IEnumerable<Tag> nTags;

        private async Task update(int imageId, string name, string date, string description, string data, int? albumId, IEnumerable<Tag> tags)
        {
            tags = null; // tymczasowo, aby uniknac bledu przy updatowaniu zdjecia

            SingleImageViewModel image = new SingleImageViewModel {
                Id = imageId,
                Name = name,
                Date = date,
                Description = description,
                Data = data,
                AlbumId = albumId,
                Tags = tags
            };
            await ImageService.UpdateImage(image);
            StateHasChanged();
        }
    }
}

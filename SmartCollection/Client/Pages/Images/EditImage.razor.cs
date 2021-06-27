using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartCollection.Models.ViewModels.ImagesViewModel;
using SmartCollection.Models.DBModels;
using SmartCollection.Utilities.TagManagement.TagCreator;

namespace SmartCollection.Client.Pages.Images
{
    public partial class EditImage
    {
        [Parameter]
        public string ImageId { get; set; }

        private SingleImageViewModel image;
        private readonly ITagCreator _tagCreator;
        public EditImage(ITagCreator tagCreator)   
            => _tagCreator = tagCreator;
        
        protected override async Task OnInitializedAsync()
        {
            image = await ImageService.GetImage(int.Parse(ImageId));
            StateHasChanged();
        }

        // temp edit image

        string nName, nDescription, ntagsString;
        IEnumerable<string> nTags;

        List<Tag> Tags;

        private async Task update(int imageId, string name, string date, string description, string data, int? albumId, string tags)
        {
            nTags = _tagCreator.CreateTagList(tags);
            //tutaj ma byc przesylanie tagow do bazy danych

            

            SingleImageViewModel image = new SingleImageViewModel {
                Id = imageId,
                Name = name,
                Date = date,
                Description = description,
                Data = data,
                AlbumId = albumId,
            };
            await ImageService.UpdateImage(image);
            StateHasChanged();
        }
    }
}

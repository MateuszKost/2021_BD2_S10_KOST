using Microsoft.AspNetCore.Components;
using SmartCollection.Models.ViewModels.ImagesViewModel;
using SmartCollection.Utilities.TagManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartCollection.Client.Pages.Images
{
    public partial class EditImage
    {
        [Parameter]
        public int ImageId { get; set; }

        private string _tags = string.Empty;
        private DateTime _date = DateTime.Now;

        private SingleImageViewModel image = new();

        private bool Success = false;
        private List<string> Errors = new();


        protected override async Task OnInitializedAsync()
        {
            image = await ImageService.GetImage(ImageId);
            if (image.Tags?.Any() == true)
                _tags = string.Join(" ", image.Tags.Select(x => "#" + x));
            _date = DateTime.Parse(image.Date);
        }

        private async Task SubmitChangesAsync()
        {
            image.Date = _date.ToString();
            image.Tags = TagService.CreateTagList(_tags);
            var result = await ImageService.UpdateImage(image);
            if (result.Succeeded)
                Success = true;
            else
            {
                Success = false;
                Errors = result.Errors;
            }  
            StateHasChanged();
        }
    }
}
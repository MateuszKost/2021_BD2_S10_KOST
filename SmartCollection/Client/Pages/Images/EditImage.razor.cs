using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartCollection.Models.ViewModels.ImagesViewModel;

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

        string nName, nDescription, nTags;

        public void Save(string nname, string ndescription, string ntags)
        {
            nName = nname;
            nDescription = ndescription;
            nTags = ntags;
        }


        TempImageModel temp = new TempImageModel() {Name = "nazwa", Description="opis", Tags="Tagi, Tagi2", URL= "/picture/monkey.jpg" };

    }

    public class TempImageModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }
        public string URL { get; set; }
    }
}

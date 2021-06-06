using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace SmartCollection.Client.Pages.Album
{
    public partial class Album
    {
        private void Navigate()
        {
            NavigationManager.NavigateTo("Images");
        }


        protected override async Task OnInitializedAsync()
        {
         //   var data = await Http.GetFromJsonAsync<ImagesViewModel>("images");
         //   model = data.Images.ToList();
        }

    }

}

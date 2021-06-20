using SmartCollection.Models.ViewModels.AlbumViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace SmartCollection.Client.Pages.Album
{
    public partial class Album
    {
        private IEnumerable<SingleAlbumViewModel> albums;
        private void Navigate(int albumId)
        {
            NavigationManager.NavigateTo("Images");
        }

        protected override async Task OnInitializedAsync()
        {
            var result = await Http.GetFromJsonAsync<AlbumViewModel>("albums");
            albums = result.AlbumViewModelList;
            StateHasChanged();
        }

    }

}

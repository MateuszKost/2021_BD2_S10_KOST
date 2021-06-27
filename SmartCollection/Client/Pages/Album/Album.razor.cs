using SmartCollection.Models.ViewModels.AlbumViewModel;
using SmartCollection.Models.ViewModels.ImagesViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SmartCollection.Client.Pages.Album
{
    public partial class Album
    {
        private IEnumerable<SingleAlbumViewModel> albums;

        private void Navigate(int albumId)
        {
            NavigationManager.NavigateTo("images/"+albumId);
        }
        private void NavigateToAlbum(int albumId)
        {
            NavigationManager.NavigateTo("editalbum/"+albumId);
        }


        protected override async Task OnInitializedAsync()
        {
            var result = await Http.GetFromJsonAsync<AlbumViewModel>("albums");
            albums = result.AlbumViewModelList;
            StateHasChanged();
        }

    }

}

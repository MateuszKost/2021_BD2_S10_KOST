using SmartCollection.Models.ViewModels.AlbumViewModel;
using SmartCollection.Models.ViewModels.ImagesViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading;
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
            albums = await AlbumService.GetAlbums();
            StateHasChanged();
        }
        private async Task Delete(int albumId)
        {
            await AlbumService.DeleteAlbum(albumId);
            albums = await AlbumService.GetAlbums();
            StateHasChanged();
        }

        CancellationTokenSource cts = new();
        State state = new("Copy link", "oi oi-clipboard");

        async Task CopyToClipboard(string address)
        {
            var temp = state;
            state = new("Copied", "oi oi-check", IsDisabled: true);
            await ClipboardService.WriteTextAsync(address);
            await Task.Delay(TimeSpan.FromSeconds(2), cts.Token);
            state = temp;
        }

        public void Dispose()
        {
            cts.Cancel(); // Cancel Task.Delay
            cts.Dispose();
        }

        record State(string Text, string ClassName, bool IsDisabled = false);
    }
}

using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using SmartCollection.Client.Shared;
using SmartCollection.Models.ViewModels.CreateAlbumViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SmartCollection.Client.Pages.CreateAlbum
{
    public partial class CreateAlbum
    {
        private CreateAlbumViewModel _createAlbumModel = new();
        private bool ShowError;

        [Parameter]
        public bool ShowDialog { get; set; }

        private string CreatedAlbumName = "";

        private async Task AddAlbum()
        {
            ShowError = false;
            ShowDialog = false;

            var jsonModel = JsonConvert.SerializeObject(_createAlbumModel);
            var result = await Http.PostAsync("https://localhost:44368/CreateAlbum", new StringContent(jsonModel, Encoding.UTF8, "application/json"));

            if (result.IsSuccessStatusCode)
            {
                CreatedAlbumName = _createAlbumModel.Name;
                OnDialogOpen();

                _createAlbumModel = new();
            }
            else
            {
                ShowError = true;
            }
        }

        private void NavigateNotAuthorized()
        {
            NavigationManager.NavigateTo("/", false);
        }

        private void OnCloseDialog()
        {
            ShowDialog = false;
            StateHasChanged();
        }

        private void OnDialogOpen()
        {
            ShowDialog = true;
            StateHasChanged();
        }

    }
}

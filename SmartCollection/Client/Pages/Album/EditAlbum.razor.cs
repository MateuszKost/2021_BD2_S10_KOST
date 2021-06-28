using Microsoft.AspNetCore.Components;
using SmartCollection.Models.ViewModels.AlbumViewModel;
using SmartCollection.Models.ViewModels.ImagesViewModel;
using SmartCollection.Utilities.TagManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartCollection.Client.Pages.Album
{
    public partial class EditAlbum
    {
        [Parameter]
        public int AlbumId { get; set; }

        private SingleAlbumViewModel album = new();
        private bool status = false;

        private bool Success = false;
        private List<string> Errors = new();


        protected override async Task OnInitializedAsync()
        {
            album = await AlbumService.GetAlbum(AlbumId);
            status = album.IsPublic;
        }

        private async Task SubmitChangesAsync()
        {
            album.IsPublic = status;
            var result = await AlbumService.UpdateAlbum(album);
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
using Microsoft.AspNetCore.Components;
using SmartCollection.Models.ViewModels.AlbumViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartCollection.Models.ViewModels.CreateAlbumViewModel;

namespace SmartCollection.Client.Pages.Album
{
    public partial class EditAlbum
    {
        [Parameter]
        public string AlbumId { get; set; }

        //private CreateAlbumViewModel _createAlbumModel = new();

        private SingleAlbumViewModel album;
        protected override async Task OnInitializedAsync()
        {
            album = await AlbumService.GetAlbum(int.Parse(AlbumId));
            StateHasChanged();
        }

        // temp edit image

        string nName, nDescrption;
        bool status;

        private async Task update(string albumName, string description, bool isPublic ,string coverPic, int imgCount, int albumId)
        {
            SingleAlbumViewModel album = new SingleAlbumViewModel
            {
                AlbumName = albumName,
                Description = description,
                IsPublic = isPublic,
                AlbumCoverPicture = coverPic,
                ImagesCount = imgCount,
                AlbumId = albumId
            };
            await AlbumService.UpdateAlbum(album);
            StateHasChanged();
        }
    }
}

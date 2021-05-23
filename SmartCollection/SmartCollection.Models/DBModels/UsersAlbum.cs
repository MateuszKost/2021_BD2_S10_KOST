using System;
using System.Collections.Generic;

#nullable disable

namespace SmartCollection.Models.DBModels
{
    public partial class UsersAlbum
    {
        public int? UsersUserId { get; set; }
        public int? AlbumsUserId { get; set; }

        public virtual Album AlbumsUser { get; set; }
        public virtual User UsersUser { get; set; }
    }
}

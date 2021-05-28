using System;
using System.Collections.Generic;

#nullable disable

namespace SmartCollection.Models.DBModels
{
    public partial class Album
    {
        public Album()
        {
            Images = new HashSet<Image>();
        }

        public int AlbumId { get; set; }
        public string UserId { get; set; }
        public int? PrivacyId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual Privacy Privacy { get; set; }
        public virtual ICollection<Image> Images { get; set; }
    }
}

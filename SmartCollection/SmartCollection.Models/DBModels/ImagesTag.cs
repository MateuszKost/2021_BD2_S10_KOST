using System;
using System.Collections.Generic;

#nullable disable

namespace SmartCollection.Models.DBModels
{
    public partial class ImagesTag
    {
        public int? TagId { get; set; }
        public int? ImageId { get; set; }

        public virtual Image Image { get; set; }
        public virtual Tag Tag { get; set; }
    }
}

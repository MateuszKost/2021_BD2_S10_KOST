using System;
using System.Collections.Generic;

#nullable disable

namespace SmartCollection.Models.DBModels
{
    public partial class ImageDetail
    {
        public int ImageId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public string OriginalName { get; set; }

        public virtual Image Image { get; set; }
    }
}

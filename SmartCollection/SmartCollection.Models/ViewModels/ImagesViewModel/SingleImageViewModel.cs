using SmartCollection.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCollection.Models.ViewModels.ImagesViewModel
{
    public class SingleImageViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }
        public string Description { get; set; }
        public string Data { get; set; }
        public int? AlbumId { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}

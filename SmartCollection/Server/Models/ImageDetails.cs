using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartCollection.Server.Models
{
    public class ImageDetails
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public string OriginalName { get; set; }
    }
}

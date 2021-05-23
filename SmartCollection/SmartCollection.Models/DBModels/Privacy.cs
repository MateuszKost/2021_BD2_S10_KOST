using System;
using System.Collections.Generic;

#nullable disable

namespace SmartCollection.Models.DBModels
{
    public partial class Privacy
    {
        public Privacy()
        {
            Albums = new HashSet<Album>();
        }

        public int PrivacyId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Album> Albums { get; set; }
    }
}

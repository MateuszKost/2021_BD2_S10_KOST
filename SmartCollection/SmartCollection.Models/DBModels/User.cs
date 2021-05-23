using System;
using System.Collections.Generic;

#nullable disable

namespace SmartCollection.Models.DBModels
{
    public partial class User
    {
        public User()
        {
            Albums = new HashSet<Album>();
            Images = new HashSet<Image>();
        }

        public int UserId { get; set; }
        public int? CredentialsId { get; set; }
        public string Name { get; set; }

        public virtual UserCredential UserCredential { get; set; }
        public virtual ICollection<Album> Albums { get; set; }
        public virtual ICollection<Image> Images { get; set; }
    }
}

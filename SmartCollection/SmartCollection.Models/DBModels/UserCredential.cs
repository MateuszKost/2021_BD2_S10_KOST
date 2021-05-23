using System;
using System.Collections.Generic;

#nullable disable

namespace SmartCollection.Models.DBModels
{
    public partial class UserCredential
    {
        public int UserId { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }

        public virtual User User { get; set; }
    }
}

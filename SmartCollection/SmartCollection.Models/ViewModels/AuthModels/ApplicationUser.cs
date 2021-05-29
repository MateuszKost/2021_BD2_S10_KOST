using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCollection.Models.ViewModels.AuthModels
{
    public class ApplicationUser
    {
        public bool isAuthenticated { get; set; }
        public string UserName { get; set; }
        public string Id { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SmartCollection.Models.ViewModels.AuthModels
{
   [JsonObject]
    public class LoginModel
    {
        [JsonProperty]
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [JsonProperty]
        [Required(ErrorMessage = "Password is required")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Must be between 6 and 255 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [JsonProperty]
        public bool RememberMe { get; set; }
    }
}

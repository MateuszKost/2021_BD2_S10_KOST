using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCollection.Models.ViewModels.AuthModels
{
    [JsonObject]
    public class RegisterModel
    {
        [JsonProperty]
        [Required(ErrorMessage = "Your First Name is required")]
        public string FirstName { get; set; }
        [JsonProperty]
        [Required(ErrorMessage = "Your Last Name is required")]
        public string LastName { get; set; }
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
        [Required(ErrorMessage = "Confirm Your password")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Must be between 6 and 255 characters")]
        [Compare("Password", ErrorMessage = "Passwords must be the same!")]
        public string ConfirmPassword { get; set; }

    }
}

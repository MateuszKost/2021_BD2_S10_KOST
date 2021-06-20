using System.ComponentModel.DataAnnotations;

namespace SmartCollection.Models.ViewModels.AuthModels
{
    public class ChangePasswordModel
    {
        [Required]
        [MinLength(6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [MinLength(6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [MinLength(6)]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword))]
        public string ConfirmNewPassword { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace SmartCollection.Models.ViewModels.AuthModels
{
    public class ChangeSettingsModel
    {
        [Required(AllowEmptyStrings = false)]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string LastName { get; set; }

        [MinLength(6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [MinLength(6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [MinLength(6)]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword))]
        public string ConfirmNewPassword { get; set; }
    }
}

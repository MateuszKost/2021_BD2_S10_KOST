using SmartCollection.Models.ViewModels.AuthModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartCollection.Client.Pages.Login
{
    public partial class Login
    {
        private readonly LoginModel model = new LoginModel();

        public bool ShowErrors { get; set; }

        public IEnumerable<string> Errors { get; set; }

        public async Task OnSubmit()
        {
            var result = await AuthService.Login(model);

            if (result.Succeeded)
            {
                ShowErrors = false;
                NavigationManager.NavigateTo("/");
            }
            else
            {
                Errors = result.Errors;
                ShowErrors = true;
            }
        }
    }
}

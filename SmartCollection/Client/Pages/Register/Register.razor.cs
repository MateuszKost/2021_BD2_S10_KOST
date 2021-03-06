using Microsoft.AspNetCore.Components;
using SmartCollection.Models.ViewModels.AuthModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartCollection.Client.Pages.Register
{
    public partial class Register
    {
        private readonly RegisterModel model = new RegisterModel();

        public bool ShowErrors { get; set; }

        public bool Success { get; set; } = false;

        public IEnumerable<string> Errors { get; set; }

        private async Task OnSubmit()
        {
            var result = await AuthService.Register(model);

            if (result.Succeeded)
            {
                ShowErrors = false;
                Success = true;
                NavigationManager.NavigateTo("/login");
            }
            else
            {
                Errors = result.Errors;
                ShowErrors = true;
            }
        }
    }
}

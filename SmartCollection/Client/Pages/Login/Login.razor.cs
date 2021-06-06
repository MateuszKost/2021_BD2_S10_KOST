using SmartCollection.Models.ViewModels.AuthModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using System.Net.Http.Json;
using Newtonsoft.Json;
using System.Text;
using SmartCollection.Client.Authorization;

namespace SmartCollection.Client.Pages.Login
{
    public partial class Login
    {
        private LoginModel LoginModel = new();
        private string Error = "";
        private bool ShowError;

        public async Task OnSubmit()
        {
            ShowError = false;

            var result = await AuthService.Login(LoginModel);

            if (result.Successful)
            {
                NavigationManager.NavigateTo("/");
            }
            else
            {
                Error = result.Error;
                ShowError = true;
            }
        }
    }
}

using Microsoft.AspNetCore.Components;
using SmartCollection.Models.ViewModels.AuthModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SmartCollection.Client.Pages.Profile
{
    public partial class Profile
    {
        private readonly ChangeSettingsModel settings = new ChangeSettingsModel();

        private readonly LoginModel loginModel = new LoginModel();

        public bool ShowErrors { get; set; }

        public IEnumerable<string> Errors { get; set; }

        protected override async Task OnInitializedAsync() => await LoadDataAsync();

        private async Task SubmitChangesAsync()
        {
            if(settings.FirstName.Contains(" ") ||
               settings.LastName.Contains(" ") ||
               settings.FirstName.Contains("_") ||
               settings.LastName.Contains("_"))
            {
                Errors = new string[] { "First name or last name contains forbidden characters. " };
                ShowErrors = true;
                return;
            }

            var response = await Http.PutAsJsonAsync("ChangeSettings", settings);

            if (response.IsSuccessStatusCode)
            {
                ShowErrors = false;
                bool confirmed = await JsRuntime.InvokeAsync<bool>("confirm", new[] { "Your settings were changed successfully!\nYou will be redirected to login page." });
                if (confirmed)
                {
                    await AuthService.Logout();
                    NavigationManager.NavigateTo("/login");
                }
            }
            else
            {
                Errors = await response.Content.ReadFromJsonAsync<string[]>();
                ShowErrors = true;
            }
        }

        private async Task DeleteAccountAsync()
        {
            bool ensured = await JsRuntime.InvokeAsync<bool>("confirm", new[] { "Are you sure you want to delete yout account?\nThis is irreversible!" });
            if (ensured)
            {
                var response = await Http.PostAsJsonAsync("DeleteAccount", loginModel);

                if (response.IsSuccessStatusCode)
                {
                    ShowErrors = false;
                    bool confirmed = await JsRuntime.InvokeAsync<bool>("confirm", new[] { "Your account has been deleted.\nThank you for using Smart Collection!" });
                    if (confirmed)
                    {
                        await AuthService.Logout();
                        NavigationManager.NavigateTo("/");
                    }
                }
                else
                {
                    Errors = await response.Content.ReadFromJsonAsync<string[]>();
                    ShowErrors = true;
                }
            }
        }

        private async Task LoadDataAsync()
        {
            var state = await AuthState.GetAuthenticationStateAsync();
            var user = state.User;
            loginModel.Email = user.FindFirstValue(ClaimTypes.Email);
            var names = user.FindFirstValue(ClaimTypes.Name).Split("_");
            settings.FirstName = names[0];
            settings.LastName = names[1];
        }
    }
}

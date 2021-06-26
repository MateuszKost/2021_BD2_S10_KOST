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

        [Parameter]
        public bool ShowDialog { get; set; }

        public IEnumerable<string> Errors { get; set; }

        protected override async Task OnInitializedAsync() => await LoadDataAsync();

        private async Task SubmitChangesAsync()
        {
            var response = await Http.PutAsJsonAsync("changesettings", settings);

            if (response.IsSuccessStatusCode)
            {
                ShowErrors = false;

                OnDialogOpen();

                await AuthService.Logout();

                NavigationManager.NavigateTo("/login");
            }
            else
            {
                Errors = await response.Content.ReadFromJsonAsync<string[]>();
                ShowErrors = true;
            }
        }

        private async Task DeleteAccountAsync()
        {
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

        private void OnCloseDialog()
        {
            ShowDialog = false;
            StateHasChanged();
        }

        private void OnDialogOpen()
        {
            ShowDialog = true;
            StateHasChanged();
        }
    }
}

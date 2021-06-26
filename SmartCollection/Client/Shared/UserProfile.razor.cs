using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace SmartCollection.Client.Shared
{
    public partial class UserProfile
    {
        private string Name = string.Empty;

        protected override async Task OnInitializedAsync() => await LoadDataAsync();

        private async Task LoadDataAsync()
        {
            var state = await AuthState.GetAuthenticationStateAsync();
            if (state.User.Claims.Any())
                Name = state.User.FindFirstValue(ClaimTypes.Name).Replace("_", " ");
        }

        private async Task LogoutUser()
        {
            await AuthService.Logout();
            NavigationManager.NavigateTo("/");
        }

        private async Task NavigateToProfile()
        {
            NavigationManager.NavigateTo("/profile");
        }
    }
}

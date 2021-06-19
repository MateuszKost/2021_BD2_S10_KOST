using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace SmartCollection.Client.Shared
{
    public partial class UserProfile
    {
        private string Token;
        [Parameter]
        public string UserName { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Token = await LocalStorage.GetItemAsync<string>("authToken");

            if(!string.IsNullOrEmpty(Token))
            {
                var decryptedToken = new JwtSecurityToken(Token);
                var claims = decryptedToken.Claims.ToList();
                var underscoredUserName = claims.FirstOrDefault(claimRecord => claimRecord.Type == ClaimTypes.Name).Value;

                UserName = DivideUserName(underscoredUserName);
            }
            else
            {
                UserName = null;
            }

            StateHasChanged();
        }

        private string DivideUserName(string username)
        {
            username = username.Replace('_', ' ');
            return username;
        }

        private void LogoutUser()
        {
            AuthService.Logout();
            UserName = null;
            Token = null;
            StateHasChanged();
            NavigationManager.NavigateTo("/", true);
        }

        private void HandleNavigation(string target)
        {
            if (target.Equals("login"))
                NavigationManager.NavigateTo("/login", false);
            else
                NavigationManager.NavigateTo("/register", false);
        }
    }
}

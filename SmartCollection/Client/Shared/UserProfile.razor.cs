using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;


namespace SmartCollection.Client.Shared
{
    public partial class UserProfile
    {
        
        private string Token;
        [Parameter]
        public string UserName { get; set; }

        /*
         * TODO
         * Fix this to display UserName after login.
         * Currently displays UserName AFTER refresh when user is logged.
         */
        protected override async Task OnParametersSetAsync()
        {
            Token = await LocalStorage.GetItemAsync<string>("authToken");

            if(!string.IsNullOrEmpty(Token))
            {
                var decryptedToken = new JwtSecurityToken(Token);
                var claims = decryptedToken.Claims.ToList();
                var underscoredUserName = claims.FirstOrDefault(claimRecord => claimRecord.Type == JwtRegisteredClaimNames.Name).Value;

                UserName = DivideUserName(underscoredUserName);
                //ShouldRender();
            }
            else
            {
                UserName = null;
            }
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
            ShouldRender();
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

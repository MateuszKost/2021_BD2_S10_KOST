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
        private string Username;

        protected override async Task OnInitializedAsync()
        {
            Token = await LocalStorage.GetItemAsync<string>("authToken");
            var decryptedToken = new JwtSecurityToken(Token);

            var claims = decryptedToken.Claims.ToList();
            Username = claims.FirstOrDefault(claimRecord => claimRecord.Type == JwtRegisteredClaimNames.Name).Value;

        }
    }
}

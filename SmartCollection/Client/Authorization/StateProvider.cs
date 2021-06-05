using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using SmartCollection.Models.ViewModels.AuthModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SmartCollection.Client.Authorization
{
    public class StateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        public ApplicationUser _currentUser { get; set; }

        public StateProvider(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            //var identity = new ClaimsIdentity();
            var tokenString = await _localStorage.GetItemAsync<string>("authToken");

            if (string.IsNullOrEmpty(tokenString))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            var token = new JwtSecurityToken(tokenString);

            var claims = token.Claims;

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", tokenString);

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims, "jwtAuthType")));
        }

        public void StateChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
        
       
        //private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        //{
        //    var claims = new List<Claim>();
        //    var payload = jwt.Split('.')[1];
        //    var jsonBytes = ParseBase64WithoutPadding(payload);

        //    //var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes); // bylo

        //    var keyValuePairs = JsonConvert.DeserializeObject<Dictionary<string, object>>(Convert.ToBase64String(jsonBytes)); // jest

        //    keyValuePairs.TryGetValue(ClaimTypes.Role, out object roles);

        //    if (roles != null)
        //    {
        //        if (roles.ToString().Trim().StartsWith("["))
        //        {
        //            var parsedRoles = JsonConvert.DeserializeObject<string[]>(roles.ToString());

        //            foreach (var parsedRole in parsedRoles)
        //            {
        //                claims.Add(new Claim(ClaimTypes.Role, parsedRole));
        //            }
        //        }
        //        else
        //        {
        //            claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
        //        }

        //        keyValuePairs.Remove(ClaimTypes.Role);
        //    }

        //    claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));

        //    return claims;
        //}

        //private byte[] ParseBase64WithoutPadding(string base64)
        //{
        //    switch (base64.Length % 4)
        //    {
        //        case 2: base64 += "=="; break;
        //        case 3: base64 += "="; break;
        //    }
        //    return Convert.FromBase64String(base64);
        //}

        //public async Task<ApplicationUser> GetCurrentUser()
        //{
        //    //var result = await _httpClient.GetAsync("https://localhost:44368/GetCurrentUser");
        //    //if (result.IsSuccessStatusCode)
        //    //{
        //    //    string jsonObject = await result.Content.ReadAsStringAsync();
        //    //    var currentUser = JsonConvert.DeserializeObject<ApplicationUser>(jsonObject);
        //    //    return currentUser;
        //    //}
        //    //    if (_currentUser != null && _currentUser.IsAuthenticated) return _currentUser;
        //    //_currentUser = await _authService.GetCurrentUser();
        //    return _currentUser;
        //}

        //public async Task Login(LoginModel loginModel)
        //{
        //    var result = await _authService.Login(loginModel);
        //    NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        //}
        //public async Task Register(RegisterModel registerModel)
        //{
        //    await _authService.Register(registerModel);
        //    NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        //}

        //private async Task<ApplicationUser> GetCurrentUser()
        //{
        //    if (_currentUser != null && _currentUser.IsAuthenticated) return _currentUser;
        //    _currentUser = await _authService.GetCurrentUser();
        //    return _currentUser;
        //}
    }

}

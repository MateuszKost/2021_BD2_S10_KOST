using SmartCollection.Models.ViewModels.AuthModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using System.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace SmartCollection.Client.Authorization
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private IConfiguration _configuration;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public AuthService(HttpClient httpClient, StateProvider authenticationStateProvider, ILocalStorageService localStorage, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
            _localStorage = localStorage;
            _configuration = configuration;
        }

        public async Task<LoginResult> Login(LoginModel loginModel)
        {
            var jsonModel = JsonConvert.SerializeObject(loginModel);
            var response = await _httpClient.PostAsync("https://localhost:44368/Login", new StringContent(jsonModel, Encoding.UTF8, "application/json"));
            var jsonResult = await response.Content.ReadAsStringAsync();
            Console.WriteLine("RESULT  " + jsonResult);
            var loginResult = JsonConvert.DeserializeObject<LoginResult>(jsonResult);

            // fail
            if (!response.IsSuccessStatusCode)
            {
                return loginResult;
            }

            await _localStorage.SetItemAsync("authToken", loginResult.Token);
            // success
            ((StateProvider)_authenticationStateProvider).AuthenticateUser(loginModel.Email);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", loginResult.Token);
            return loginResult;
        }

        public async Task<RegisterResult> Register(RegisterModel registerModel)
        {
            var jsonModel = JsonConvert.SerializeObject(registerModel);
            var response = await _httpClient.PostAsync("https://localhost:44368/Register", new StringContent(jsonModel, Encoding.UTF8, "application/json"));
            var jsonResult = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<RegisterResult>(jsonResult);
            return result;
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((StateProvider)_authenticationStateProvider).Logout();
            _httpClient.DefaultRequestHeaders.Authorization = null;
            //var result = await _httpClient.PostAsync("https://localhost:44368/Logout", null);
            //result.EnsureSuccessStatusCode();
        }

        public async Task<ApplicationUser> GetCurrentUser()
        {
            var result = await _httpClient.GetAsync("https://localhost:44368/GetCurrentUser");
            if (result.IsSuccessStatusCode)
            {
                string jsonObject = await result.Content.ReadAsStringAsync();
                var currentUser = JsonConvert.DeserializeObject<ApplicationUser>(jsonObject);
                return currentUser;
            }
            else throw new Exception(await result.Content.ReadAsStringAsync());
        }
    }
}

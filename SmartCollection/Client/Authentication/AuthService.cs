using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using SmartCollection.Models.ViewModels;
using SmartCollection.Models.ViewModels.AuthModels;
using SmartCollection.Client.Extensions;

namespace SmartCollection.Client.Authentication
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient httpClient;
        private readonly ILocalStorageService localStorage;
        private readonly AuthenticationStateProvider authenticationStateProvider;

        private const string LoginPath = "/login";
        private const string RegisterPath = "/register";

        public AuthService(
            HttpClient httpClient,
            ILocalStorageService localStorage,
            AuthenticationStateProvider authenticationStateProvider)
        {
            this.httpClient = httpClient;
            this.localStorage = localStorage;
            this.authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<Result> Register(RegisterModel model)
            => await httpClient
                .PostAsJsonAsync(RegisterPath, model)
                .ToResult();

        public async Task<Result> Login(LoginModel model)
        {
            var response = await httpClient.PostAsJsonAsync(LoginPath, model);

            if (!response.IsSuccessStatusCode)
            {
                var errors = await response.Content.ReadFromJsonAsync<string[]>();

                return Result.Failure(errors);
            }

            var responseAsString = await response.Content.ReadAsStringAsync();

            var responseObject = JsonSerializer.Deserialize<LoginResult>(responseAsString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var token = responseObject.Token;

            await localStorage.SetItemAsync("authToken", token);

            ((ApiAuthenticationStateProvider)authenticationStateProvider).MarkUserAsAuthenticated(model.Email);

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return Result.Success;
        }

        public async Task Logout()
        {
            await localStorage.RemoveItemAsync("authToken");

            ((ApiAuthenticationStateProvider)authenticationStateProvider).MarkUserAsLoggedOut();

            httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}

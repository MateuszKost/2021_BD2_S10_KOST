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

namespace SmartCollection.Client.Pages.Login
{
    public partial class Login
    {
        private LoginModel LoginModel = new();
        private string LoginWarning = "d-none";
        public async Task OnSubmit()
        {
            HttpClient Http = new();
            var json = JsonConvert.SerializeObject(LoginModel);
            
            var result = await Http.PostAsync("https://localhost:44368/Login", new StringContent(json, Encoding.UTF8, "application/json"));
            Console.WriteLine(result.StatusCode);
            if (result.IsSuccessStatusCode) NavigationManager.NavigateTo("/");
            else LoginWarning = "block";
        }
    }
}

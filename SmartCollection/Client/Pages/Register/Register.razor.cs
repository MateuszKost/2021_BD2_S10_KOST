using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using SmartCollection.Client.Authorization;
using SmartCollection.Models.ViewModels.AuthModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SmartCollection.Client.Pages.Register
{
    public partial class Register
    {
        private bool ShowErrors, ShowSuccess;
        private IEnumerable<string> Errors;
        private RegisterModel RegisterModel = new();

        private async Task OnSubmit()
        {
            ShowErrors = false;
            ShowSuccess = false;
            var result = await AuthService.Register(RegisterModel);

            if (result.Succesfull)
            {
                ShowSuccess = true;
            }
            else
            {
                ShowErrors = true;
                foreach(var e in Errors)
                {
                    Console.WriteLine(e);
                }
            }
            //var json = JsonConvert.SerializeObject(RegisterModel);

            //var result = await Http.PostAsync("https://localhost:44368/Register", new StringContent(json, Encoding.UTF8, "application/json"));
            //Console.WriteLine(result.StatusCode);
            //if (result.IsSuccessStatusCode) NavigationManager.NavigateTo("/");
            //await _stateProvider.Register(RegisterModel);
            //else RegisterWarning = "block";
        }

    }
}

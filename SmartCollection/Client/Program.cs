using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using SmartCollection.Client.Authentication;
using SmartCollection.Client.Services;
using SmartCollection.Models.DBModels;
using SmartCollection.Models.ViewModels.ImagesViewModel;
using SmartCollection.Utilities.TagManagement.TagCreator;
using SmartCollection.Utilities.TagManagement.TagSender;

namespace SmartCollection.Client
{
    public class Program
    {
        private const string ClientName = "SmartCollection.ServerAPI";
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder
               .Services
               .AddAuthorizationCore()
               .AddBlazoredLocalStorage()
               .AddScoped<ApiAuthenticationStateProvider>()
               .AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>()
               .AddScoped(sp => sp
                   .GetRequiredService<IHttpClientFactory>()
                   .CreateClient(ClientName))
               .AddTransient<IAuthService, AuthService>()
               .AddTransient<AuthenticationHeaderHandler>()
               .AddTransient<IImageService<SingleImageViewModel>, ImageService>()
               .AddTransient<IAlbumService, AlbumService>()
               .AddTransient<ITagService<Tag>, TagService>()
               .AddTransient<ITagCreator, TagCreator>()
               .AddTransient<ITagSender, TagSender>()
               .AddHttpClient(
                   ClientName,
                   client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
               .AddHttpMessageHandler<AuthenticationHeaderHandler>();
               

            await builder.Build().RunAsync();
        }
    }
}

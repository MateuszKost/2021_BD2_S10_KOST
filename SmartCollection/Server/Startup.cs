using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmartCollection.DataAccess.Context;
using SmartCollection.DataAccess.RepositoryPattern;
using SmartCollection.StorageManager.Containers;
using SmartCollection.StorageManager.Context;
using SmartCollection.StorageManager.ServiceClient;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SmartCollection.Utilities.DatabaseInitializer;
using SmartCollection.Models.ViewModels.CreateAlbumViewModel;
using SmartCollection.Utilities.AlbumCreator;
using SmartCollection.Utilities.HashGenerator;
using SmartCollection.Server.User;
using SmartCollection.Server.Identity;
using SmartCollection.Utilities.ImageConverter;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace SmartCollection.Server
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddNewtonsoftJson();
            services.AddRazorPages();
            services.AddDbContext<SmartCollectionDbContext>(
                
                options => options.UseNpgsql(
                    Configuration.GetConnectionString("SmartCollectionDB"))
               
                );

            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.User.RequireUniqueEmail = true;
                options.Password = new PasswordOptions
                {
                    RequireDigit = false,
                    RequiredLength = 6,
                    RequireLowercase = true,
                    RequireUppercase = false,
                    RequireNonAlphanumeric = false
                };
            })
            .AddEntityFrameworkStores<SmartCollectionDbContext>();

            var key = Encoding.ASCII.GetBytes(Configuration["Jwt:SecurityKey"]);

            services.AddAuthentication(authentication =>
            {
                authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(bearer =>
           {
               bearer.RequireHttpsMetadata = false;
               bearer.SaveToken = true;
               bearer.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(key),
                   ValidateIssuer = false,
                   ValidateAudience = false
               };
           });

            services.AddSession();

            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUser, CurrentUser>();

            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<IJwtGeneratorService, JwtGeneratorService>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IImageConverter, ImageConverter>();
            
            services.AddSingleton(provider =>
            new BlobStorageServiceClient(Configuration.GetConnectionString("BlobStorage")));
            services.AddScoped<IStorageContext<IStorageContainer>, BlobStorageContext<IStorageContainer>>();

            services.AddScoped<IDatabaseInitializer, DatabaseInitializer>();
            services.AddScoped<IAlbumCreator<CreateAlbumViewModel,IUnitOfWork>, AlbumCreator>();
            services.AddScoped<IHashGenerator, Sha1HashGenerator>();
            
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}

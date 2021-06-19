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
using Newtonsoft;
using System.Threading.Tasks;
using SmartCollection.Models.ViewModels.AuthModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SmartCollection.Utilities.DatabaseInitializer;
using SmartCollection.Models.ViewModels.CreateAlbumViewModel;
using SmartCollection.Utilities.AlbumCreator;
using SmartCollection.Utilities.HashGenerator;
using SmartCollection.Server.User;
using SmartCollection.Server.Identity;

namespace SmartCollection.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
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

            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUser, CurrentUser>();

            //.AddFacebook(options =>
            //{
            //    options.AppId = Configuration["Authentication:Facebook:AppId"];
            //    options.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
            //})
            //.AddGoogle(options =>
            //{
            //    options.ClientId = Configuration["Authentication:Google:ClientId"];
            //    options.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
            //    options.CallbackPath = "/welcome";
            //});

            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<IJwtGeneratorService, JwtGeneratorService>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            services.AddSingleton(provider =>
            new BlobStorageServiceClient(Configuration.GetConnectionString("BlobStorage")));
            services.AddScoped<IStorageContext<IStorageContainer>, BlobStorageContext<IStorageContainer>>();

            services.AddScoped<IDatabaseInitializer, DatabaseInitializer>();
            services.AddScoped<IAlbumCreator<CreateAlbumViewModel,IUnitOfWork>, AlbumCreator>();
            services.AddScoped<IHashGenerator, Sha1HashGenerator>();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

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

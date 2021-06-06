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
using Microsoft.AspNetCore.Components.Authorization;
using SmartCollection.Models.ViewModels.AuthModels;
using SmartCollection.Client.Authorization;
using Microsoft.AspNetCore.Authentication;
using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using SmartCollection.Utilities.DatabaseInitializer;
using SmartCollection.Models.ViewModels.CreateAlbumViewModel;
using SmartCollection.Utilities.AlbumCreator;

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
                // added password details
                options.Password = new PasswordOptions
                {
                    RequireDigit = false,
                    RequiredLength = 6,
                    RequireLowercase = true,
                    RequireUppercase = false,
                    RequireNonAlphanumeric = false
                };

            })
            .AddEntityFrameworkStores<SmartCollectionDbContext>()
            .AddDefaultTokenProviders();

            //services.AddIdentityServer().AddApiAuthorization<IdentityUser, SmartCollectionDbContext>(op =>
            //op.IdentityResources);

            //services.AddAuthentication()
            //.AddIdentityServerJwt();

            //services.ConfigureApplicationCookie(options =>
            //{
            //    options.Cookie.HttpOnly = false;
            //});

            services.AddTransient<ITokenService, TokenService>();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).
            AddJwtBearer(jwtOptions =>
            {
                jwtOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SecurityKey"]))
                };
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            services.AddSingleton(provider =>
            new BlobStorageServiceClient(Configuration.GetConnectionString("BlobStorage")));
            services.AddScoped<IStorageContext<IStorageContainer>, BlobStorageContext<IStorageContainer>>();

            services.AddScoped<IDatabaseInitializer, DatabaseInitializer>();
            services.AddScoped<IAlbumCreator<CreateAlbumViewModel,IUnitOfWork>, AlbumCreator>();
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

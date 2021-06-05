using SmartCollection.Models.ViewModels.AuthModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartCollection.Client.Authorization
{
    public interface IAuthService
    {
        Task<LoginResult> Login(LoginModel loginModel);
        //Task<RegisterResult> Register(RegisterModel registerModel);
        Task Logout();
        Task<ApplicationUser> GetCurrentUser();
    }
}

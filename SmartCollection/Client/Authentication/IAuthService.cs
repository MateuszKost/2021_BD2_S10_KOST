using SmartCollection.Models.ViewModels;
using SmartCollection.Models.ViewModels.AuthModels;
using System.Threading.Tasks;

namespace SmartCollection.Client.Authentication
{
    public interface IAuthService
    {
        Task<Result> Register(RegisterModel model);

        Task<Result> Login(LoginModel model);

        Task Logout();
    }
}

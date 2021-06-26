using SmartCollection.Models.ViewModels;
using SmartCollection.Models.ViewModels.AuthModels;
using System.Threading.Tasks;

namespace SmartCollection.Server.Identity
{
    public interface IIdentityService
    {
        Task<Result> RegisterAsync(RegisterModel model);

        Task<Result<LoginResult>> LoginAsync(LoginModel model);

        Task<Result> ChangeSettingsAsync(ChangeSettingsModel model, string userId);
    }
}

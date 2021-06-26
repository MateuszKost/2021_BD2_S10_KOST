using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SmartCollection.Models.ViewModels;
using SmartCollection.Models.ViewModels.AuthModels;

namespace SmartCollection.Server.Identity
{
    public class IdentityService : IIdentityService
    {
        private const string InvalidErrorMessage = "Invalid email or password.";

        private readonly UserManager<IdentityUser> userManager;
        private readonly IJwtGeneratorService jwtGenerator;

        public IdentityService(
            UserManager<IdentityUser> userManager,
            IJwtGeneratorService jwtGenerator)
        {
            this.userManager = userManager;
            this.jwtGenerator = jwtGenerator;
        }

        public async Task<Result> RegisterAsync(RegisterModel model)
        {
            var user = new ApplicationUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email
            };

            var identityResult = await userManager.CreateAsync(user, model.Password);

            var errors = identityResult.Errors.Select(e => e.Description);

            return identityResult.Succeeded
                ? Result.Success
                : Result.Failure(errors);
        }

        public async Task<Result<LoginResult>> LoginAsync(LoginModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return InvalidErrorMessage;
            }

            var passwordValid = await userManager.CheckPasswordAsync(user, model.Password);
            if (!passwordValid)
            {
                return InvalidErrorMessage;
            }

            var token = jwtGenerator.GenerateJwt(user);

            return new LoginResult { Token = token };
        }

        public async Task<Result> ChangeSettingsAsync(ChangeSettingsModel model, string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return InvalidErrorMessage;
            }

            IdentityResult result = null;

            if (!string.IsNullOrEmpty(model.Password) &&
                !string.IsNullOrEmpty(model.NewPassword) &&
                !string.IsNullOrEmpty(model.ConfirmNewPassword))
            {
                result = await userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);
            }

            if(user.UserName.Split("_") is var dupa)
            {

            }

            var errors = result.Errors.Select(e => e.Description);

            return result.Succeeded
                ? Result.Success
                : Result.Failure(errors);
        }
    }
}

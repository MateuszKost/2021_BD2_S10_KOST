using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartCollection.Models.ViewModels.AuthModels;
using SmartCollection.Server.Identity;
using SmartCollection.Server.User;
using SmartCollection.Server.Extensions;

namespace SmartCollection.Server.Controllers
{
    [Route("")]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService identity;
        private readonly ICurrentUser currentUser;

        public IdentityController(
            IIdentityService identity,
            ICurrentUser currentUser)
        {
            this.identity = identity;
            this.currentUser = currentUser;
        }

        [HttpPost(nameof(Register))]
        public async Task<ActionResult> Register([FromBody] RegisterModel model)
            => await identity
                .RegisterAsync(model)
                .ToActionResult();

        [HttpPost(nameof(Login))]
        public async Task<ActionResult<LoginResult>> Login([FromBody] LoginModel model)
            => await identity
                .LoginAsync(model)
                .ToActionResult();


        [Authorize]
        [HttpPut(nameof(ChangeSettings))]
        public async Task<ActionResult> ChangeSettings([FromBody] ChangeSettingsModel model)
            => await identity
                .ChangeSettingsAsync(model, currentUser.UserId)
                .ToActionResult();
    }
}

using Microsoft.AspNetCore.Identity;

namespace SmartCollection.Server.Identity
{
    public interface IJwtGeneratorService
    {
        string GenerateJwt(IdentityUser user);
    }
}

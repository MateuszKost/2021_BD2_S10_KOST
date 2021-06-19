using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SmartCollection.Utilities.TokenService
{
    public interface ITokenService
    {
        public JwtSecurityToken CreateJwtToken(IdentityUser user);
        public IEnumerable<Claim> GetTokenClaims(IdentityUser user);
    }
}

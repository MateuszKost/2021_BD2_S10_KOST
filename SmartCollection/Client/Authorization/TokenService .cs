using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SmartCollection.Client.Authorization
{
    public class TokenService : ITokenService
    {
        private IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public JwtSecurityToken CreateJwtToken(IdentityUser user)
        {
            var signKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecurityKey"]));
            var credentials = new SigningCredentials(signKey, SecurityAlgorithms.HmacSha256);
            var expireInMins = _configuration.GetValue<int>("Jwt:ExpireInMinutes");

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: GetTokenClaims(user),
                //notBefore: DateTime.Now,
                expires: DateTime.Now.Add(TimeSpan.FromMinutes(expireInMins)),
                signingCredentials: credentials
                );

            return token;
        }
        public IEnumerable<Claim> GetTokenClaims(IdentityUser user)
        {

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            return claims;
        }
    }
}

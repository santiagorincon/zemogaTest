using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ZemogaTechnicalTest.Tools
{
    public class JwtAuthentication
    {
        public string SecurityKey { get; set; }
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }

        public SymmetricSecurityKey SymmetricSecurityKey => new SymmetricSecurityKey(Convert.FromBase64String(SecurityKey));
        public SigningCredentials SigningCredentials => new SigningCredentials(SymmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        public string GenerateToken(IEnumerable<Claim> claims)
        {
            var token = new JwtSecurityToken(
                issuer: ValidIssuer,
                audience: ValidAudience,
                expires: DateTime.UtcNow.AddHours(2),
                notBefore: DateTime.UtcNow,
                signingCredentials: SigningCredentials,
                claims: claims
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

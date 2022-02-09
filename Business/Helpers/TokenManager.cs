using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Totem.Business.Core.AppSettings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Totem.Business.Helpers
{
    public class TokenManager
    {
        private readonly JWT _jwtSettings;

        public TokenManager(IOptions<AppSettings> appSettings)
        {
            _jwtSettings = appSettings.Value.Jwt;
        }

        /// <summary>
        /// Generates JTW token from userId 
        /// </summary>
        public string BuildToken(long userId)
        {
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Token));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            Claim[] claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.NameId, userId.ToString())
            };

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(4),
                signingCredentials: creds);

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(token);
        }
    }
}

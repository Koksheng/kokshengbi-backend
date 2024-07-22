using kokshengbi.Application.Common.Interfaces.Authentication;
using kokshengbi.Application.Common.Interfaces.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace kokshengbi.Infrastructure.Authentication
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IDateTimeProvider _dateTimeProvider;

        public JwtTokenGenerator(IDateTimeProvider dateTimeProvider, IOptions<JwtSettings> jwtOptions)
        {
            _dateTimeProvider = dateTimeProvider;
            _jwtSettings = jwtOptions.Value;
        }

        public string GenerateToken(int userId, string userName, string userRole)
        {
            ////var keyBytes = Encoding.UTF8.GetBytes("super-secret-key-with-256-bits");
            //var keyBytes = new byte[32];
            //using (var rng = RandomNumberGenerator.Create())
            //{
            //    rng.GetBytes(keyBytes);
            //}

            //// Ensure key size is at least 256 bits (32 bytes)
            //if (keyBytes.Length < 32)
            //{
            //    throw new ArgumentException("Key size must be at least 256 bits (32 bytes).");
            //}

            //var signingKey = new SymmetricSecurityKey(keyBytes);
            //var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)), SecurityAlgorithms.HmacSha256);

            var claims = new[]
                    {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.GivenName, userName.ToString()),
                new Claim(JwtRegisteredClaimNames.FamilyName, userName.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, userId.ToString()),
                new Claim(ClaimTypes.Role, userRole) // Add the role claim
            };

            var securityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                expires: _dateTimeProvider.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                claims: claims,
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(securityToken);

        }
    }
}

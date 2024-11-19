using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace HamAndCheeseToastie.Services
{
    public class TokenService
    {
        private readonly string _secretKey;

        public TokenService(IConfiguration configuration)
        {
            _secretKey = configuration["Jwt:Key"];
        }

        public string GenerateToken(string userId, int expiryMinutes = 360)
        {
            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, userId),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(expiryMinutes),
                SigningCredentials = credentials,
                Issuer = "https://192.168.1.2:7276", // Set your issuer
                Audience = "http://localhost:3000" // Set your audience (this should match your React app's frontend URL)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_secretKey);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,          // Make sure Issuer is validated
                ValidIssuer = "https://192.168.1.2:7276", // Your API's issuer
                ValidateAudience = true,        // Make sure Audience is validated
                ValidAudience = "http://localhost:3000", // Your React app's audience
                ClockSkew = TimeSpan.Zero
            };


            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                return principal;
            }
            catch
            {
                return null; // Invalid token
            }
        }

        /// <summary>
        /// Generates a reset token for password recovery.
        /// </summary>
        /// <param name="expiryMinutes">The expiration time in minutes for the reset token.</param>
        /// <returns>A reset token as a string.</returns>
        public string GenerateResetToken(int expiryMinutes = 60)
        {
            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique ID for the reset token
                    new Claim("Purpose", "PasswordReset") // Custom claim to indicate purpose
                }),
                Expires = DateTime.UtcNow.AddMinutes(expiryMinutes),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}

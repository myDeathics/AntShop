using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using Shop.Models.JWT;

namespace Shop.Services
{
    public class UserService
    {
        public static string HashPassword(string password)
        {
            byte[] salt = new byte[128 / 8];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            }
            // derive a 256-bit subkey (use HMACSHA256 with 100 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100,
                numBytesRequested: 256 / 8));
            return hashed;
        }
        public static JwtSecurityToken GenerateJWTToken(ClaimsIdentity identity)
        {
            var now = DateTime.UtcNow;
            var token = new JwtSecurityToken(
                issuer: JWTModel.ISSUER,
                    audience: JWTModel.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(JWTModel.LIFETIME)),
                    signingCredentials: new SigningCredentials(JWTModel.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
                );
            return token;
        }
    }
}

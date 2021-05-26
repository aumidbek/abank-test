using System;
using System.Security.Cryptography;

namespace Asakabank.IdentityApi.Helpers {
    public class RefreshTokenGenerator : IRefreshTokenGenerator {
        public string GenerateToken() {
            var randomNumber = new byte[64];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
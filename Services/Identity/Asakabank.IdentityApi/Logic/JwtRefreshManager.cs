using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Asakabank.Base;
using Asakabank.IdentityApi.Helpers;
using Asakabank.IdentityApi.Models;
using Microsoft.IdentityModel.Tokens;

namespace Asakabank.IdentityApi.Logic {
    public class JwtRefreshManager : IJwtRefreshManager {
        private readonly byte[] _key;
        private readonly IJwtAuthenticationManager _jWtAuthenticationManager;

        public JwtRefreshManager(byte[] key, IJwtAuthenticationManager jWtAuthenticationManager) {
            _key = key;
            _jWtAuthenticationManager = jWtAuthenticationManager;
        }

        public async Task<AuthenticationResponse> Refresh(RefreshCred refreshCred) {
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(refreshCred.Token,
                new TokenValidationParameters {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(_key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
                }, out var validatedToken);
            if (validatedToken is not JwtSecurityToken jwtToken || !jwtToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase)) {
                return new AuthenticationResponse {
                    Code = (int) ActionResult.InvalidTokenPassed,
                    Message = ActionResult.InvalidTokenPassed.ToDescription()
                };
            }

            //var userName = principal.Identity?.Name ?? "";


            return await _jWtAuthenticationManager.Authenticate(principal.Identity?.Name ?? "",
                principal.Claims.ToArray(), refreshCred.RefreshToken);
        }
    }
}
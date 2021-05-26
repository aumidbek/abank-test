using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Asakabank.Base;
using Asakabank.IdentityApi.Entities;
using Asakabank.IdentityApi.Helpers;
using Asakabank.IdentityApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Asakabank.IdentityApi.Logic {
    public class JwtAuthenticationManager : IJwtAuthenticationManager {
        private readonly string _tokenKey;
        private readonly string _userApiUri;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;

        private readonly IServiceProvider _serviceProvider;
        //private readonly IDbRepository _dbRepository;

        public JwtAuthenticationManager(string tokenKey, string userApiUri,
            IRefreshTokenGenerator refreshTokenGenerator,
            IServiceProvider serviceProvider) {
            _tokenKey = tokenKey;
            _userApiUri = userApiUri;
            _refreshTokenGenerator = refreshTokenGenerator;
            _serviceProvider = serviceProvider;
            //_dbRepository = dbRepository;
        }

        public async Task<AuthenticationResponse> Authenticate(string username, string password) {
            var userAuthResult =
                await ApiClient.UserAuth(_userApiUri, new UserCred {Username = username, Password = password});
            if (userAuthResult?.Code != 0)
                return null;

            var tokenCreatedAt = DateTime.Now;
            var token = GenerateTokenString(username, tokenCreatedAt);
            var refreshToken = _refreshTokenGenerator.GenerateToken();

            using (var scope = _serviceProvider.CreateScope()) {
                var dbRepository = scope.ServiceProvider.GetRequiredService<IDbRepository>();

                var entity = await dbRepository.Get<DbUserToken>()
                    .FirstOrDefaultAsync(x => x.UserId == userAuthResult.User.Id);
                if (entity == null) {
                    entity = new DbUserToken(Guid.NewGuid()) {
                        Username = userAuthResult.User.Username,
                        Token = token,
                        RefreshToken = refreshToken,
                        TokenCreatedAt = tokenCreatedAt,
                        Expires = tokenCreatedAt.AddMinutes(2),
                        UserId = userAuthResult.User.Id
                    };
                    await dbRepository.Add(entity);
                }
                else {
                    entity.Token = token;
                    entity.RefreshToken = refreshToken;
                    entity.TokenCreatedAt = tokenCreatedAt;
                    entity.Expires = tokenCreatedAt.AddMinutes(2);
                    await dbRepository.Update(entity);
                }

                await dbRepository.SaveChangesAsync();
            }

            return new AuthenticationResponse {
                Token = token,
                RefreshToken = refreshToken,
                Expires = tokenCreatedAt.AddMinutes(2),
                Code = (int) ActionResult.Success,
                Message = ActionResult.Success.ToDescription()
            };
        }

        public async Task<AuthenticationResponse> Authenticate(string username, Claim[] claims, string refreshToken) {
            using var scope = _serviceProvider.CreateScope();
            var dbRepository = scope.ServiceProvider.GetRequiredService<IDbRepository>();

            var entity =
                await dbRepository.Get<DbUserToken>().FirstOrDefaultAsync(x => x.Username.Equals(username));
            if (entity == null || !refreshToken.Equals(entity.RefreshToken)) {
                return new AuthenticationResponse {
                    Code = (int) ActionResult.InvalidTokenPassed,
                    Message = ActionResult.InvalidTokenPassed.ToDescription()
                };
            }

            var tokenCreatedAt = DateTime.Now;
            var token = GenerateTokenString(username, tokenCreatedAt, claims);
            refreshToken = _refreshTokenGenerator.GenerateToken();

            entity.Token = token;
            entity.RefreshToken = refreshToken;
            entity.TokenCreatedAt = tokenCreatedAt;
            entity.Expires = tokenCreatedAt.AddMinutes(2);
            await dbRepository.Update(entity);
            await dbRepository.SaveChangesAsync();
            
            return new AuthenticationResponse {
                Token = token,
                RefreshToken = refreshToken,
                Code = (int) ActionResult.Success,
                Message = ActionResult.Success.ToDescription()
            };
        }

        private string GenerateTokenString(string username, DateTime expires, Claim[] claims = null) {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_tokenKey);
            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(
                    claims ?? new[] {
                        new Claim(ClaimTypes.Name, username)
                    }),
                //NotBefore = expires,
                Expires = expires.AddMinutes(2),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }
    }
}
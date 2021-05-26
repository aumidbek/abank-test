using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Asakabank.Base;
using Asakabank.IdentityApi.Entities;
using Asakabank.IdentityApi.Helpers;
using Asakabank.IdentityApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Asakabank.IdentityApi.Logic {
    public class JwtAuthenticationManager : IJwtAuthenticationManager {
        private readonly IDictionary<string, string> _users = new Dictionary<string, string> {
            {"test1", "password1"},
            {"test2", "password2"}
        };

        private readonly string _tokenKey;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;
        private readonly IDbRepository _dbRepository;

        public JwtAuthenticationManager(string tokenKey, IRefreshTokenGenerator refreshTokenGenerator,
            IDbRepository dbRepository) {
            _tokenKey = tokenKey;
            _refreshTokenGenerator = refreshTokenGenerator;
            _dbRepository = dbRepository;
        }

        public async Task<AuthenticationResponse> Authenticate(string username, string password) {
            //todo: Get user from UserAPI microservice using RabbitMQ
            if (!_users.Any(u => u.Key == username && u.Value == password))
                return null;

            var tokenCreatedAt = DateTime.Now;
            var token = GenerateTokenString(username, tokenCreatedAt);
            var refreshToken = _refreshTokenGenerator.GenerateToken();

            var entity = await _dbRepository.Get<DbUserToken>().FirstOrDefaultAsync(x => x.UserId == Guid.NewGuid());//todo: fix this UserId
            if (entity == null) {
                entity = new DbUserToken(Guid.NewGuid()) {
                    Username = "test1",
                    Token = token,
                    RefreshToken = refreshToken,
                    TokenCreatedAt = tokenCreatedAt,
                    Expires = tokenCreatedAt.AddMinutes(2),
                    UserId = Guid.NewGuid()
                };
                await _dbRepository.Add(entity);
            }
            else {
                entity.Token = token;
                entity.RefreshToken = refreshToken;
                entity.TokenCreatedAt = tokenCreatedAt;
                entity.Expires = tokenCreatedAt.AddMinutes(2);
                await _dbRepository.Update(entity);
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
            var entity = await _dbRepository.Get<DbUserToken>().FirstOrDefaultAsync(x => x.UserId == Guid.NewGuid());//todo: fix this UserId
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
            await _dbRepository.Update(entity);
            
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
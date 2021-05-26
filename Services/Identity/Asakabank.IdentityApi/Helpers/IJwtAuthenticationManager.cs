using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Asakabank.IdentityApi.Models;

namespace Asakabank.IdentityApi.Helpers {
    public interface IJwtAuthenticationManager {
        //IDictionary<string, string> UsersRefreshTokens { get; }
        Task<AuthenticationResponse> Authenticate(string username, string password);
        Task<AuthenticationResponse> Authenticate(string username, Claim[] claims, string refreshToken);
    }
}
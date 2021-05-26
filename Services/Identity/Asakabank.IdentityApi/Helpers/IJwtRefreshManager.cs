using System.Threading.Tasks;
using Asakabank.IdentityApi.Models;

namespace Asakabank.IdentityApi.Helpers {
    public interface IJwtRefreshManager {
        Task<AuthenticationResponse> Refresh(RefreshCred refreshCred);
    }
}
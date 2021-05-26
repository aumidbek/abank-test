using System.Threading.Tasks;
using Asakabank.UserApi.ServiceModels;

namespace Asakabank.UserApi.Core {
    public interface IUserService {
        Task<UserCreateResponse> Create(UserCreate user);
        Task<UserConfirmResponse> Confirm(UserConfirm user);
        Task<UserAuthResponse> Auth(UserCred user);
    }
}
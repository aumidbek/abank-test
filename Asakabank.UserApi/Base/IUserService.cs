using System.Threading.Tasks;
using Asakabank.UserApi.ServiceModels;

namespace Asakabank.UserApi.Base {
    public interface IUserService {
        Task<UserCreateResponse> Create(UserCreate user);
        Task<UserConfirmResponse> Confirm(UserConfirm user);
        Task<UserAuthResponse> Auth(UserAuth user);
    }
}
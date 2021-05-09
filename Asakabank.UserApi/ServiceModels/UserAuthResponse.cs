using Asakabank.Base;
using Asakabank.UserApi.Models;

namespace Asakabank.UserApi.ServiceModels {
    public class UserAuthResponse : BaseResponse {
        /// <summary>
        /// Данные пользователя
        /// </summary>
        public User User { get; set; }
    }
}
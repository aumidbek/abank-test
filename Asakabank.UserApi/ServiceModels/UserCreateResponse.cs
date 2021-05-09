using System;
using Asakabank.Base;

namespace Asakabank.UserApi.ServiceModels {
    public class UserCreateResponse : BaseResponse {
        /// <summary>
        /// Ид. пользователя
        /// </summary>
        public Guid UserId { get; set; }
    }
}
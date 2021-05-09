using System;

namespace Asakabank.UserApi.ServiceModels {
    public class UserAuth {
        /// <summary>
        /// Ид. пользователя
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Пароль (ПИН код) пользователя
        /// </summary>
        public string Password { get; set; }
    }
}
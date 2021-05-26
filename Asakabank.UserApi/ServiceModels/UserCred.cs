using System;

namespace Asakabank.UserApi.ServiceModels {
    public class UserCred {
        /// <summary>
        /// Логин
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Пароль (ПИН код) пользователя
        /// </summary>
        public string Password { get; set; }
    }
}
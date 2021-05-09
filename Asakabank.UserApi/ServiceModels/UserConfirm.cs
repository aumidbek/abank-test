using System;

namespace Asakabank.UserApi.ServiceModels {
    public class UserConfirm {
        /// <summary>
        /// Ид. пользователя
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Одноразовый код из СМС
        /// </summary>
        public string OTP { get; set; }

        /// <summary>
        /// Пароль (ПИН код) пользователя
        /// </summary>
        public string Password { get; set; }
    }
}
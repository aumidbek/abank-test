using System;

namespace Asakabank.IdentityApi.Models {
    public class UserToken {
        /// <summary>
        /// Ид. 
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Ид. пользователя
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>

        public string Username { get; set; }

        /// <summary>
        /// Токен
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Токен обновления
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// Дата создания токена
        /// </summary>
        public DateTime TokenCreatedAt { get; set; }

        /// <summary>
        /// Срок токена
        /// </summary>
        public DateTime Expires { get; set; }
    }
}
using System;

namespace Asakabank.UserApi.Models {
    public class User {
        /// <summary>
        /// Ид. пользователя
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Имя пользователя/Номер телефона
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string Lastname { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string Firstname { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public string Middlename { get; set; }

        /// <summary>
        /// Серия и номер паспорта
        /// </summary>
        public string Passport { get; set; }

        /// <summary>
        /// Признак принадлежности банку
        /// </summary>
        public bool IsIdentified { get; set; }
    }
}
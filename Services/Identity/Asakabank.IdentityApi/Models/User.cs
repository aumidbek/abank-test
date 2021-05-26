using System;
using System.Text.Json.Serialization;

namespace Asakabank.IdentityApi.Models {
    public class User {
        /// <summary>
        /// Ид. пользователя
        /// </summary>
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Имя пользователя/Номер телефона
        /// </summary>
        [JsonPropertyName("username")]
        public string Username { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        [JsonPropertyName("lastname")]
        public string Lastname { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        [JsonPropertyName("firstname")]
        public string Firstname { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        [JsonPropertyName("middlename")]
        public string Middlename { get; set; }

        /// <summary>
        /// Серия и номер паспорта
        /// </summary>
        [JsonPropertyName("passport")]
        public string Passport { get; set; }

        /// <summary>
        /// Признак принадлежности банку
        /// </summary>
        [JsonPropertyName("isIdentified")]
        public bool IsIdentified { get; set; }
    }
}
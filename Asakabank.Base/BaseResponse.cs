using System.Text.Json.Serialization;

namespace Asakabank.Base {
    public class BaseResponse {
        /// <summary>
        /// Код ошибки
        /// </summary>
        [JsonPropertyName("code")]
        public int Code { get; set; }

        /// <summary>
        /// Текст ошибки
        /// </summary>
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
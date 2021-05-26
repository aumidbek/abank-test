using System.Text.Json.Serialization;
using Asakabank.Base;

namespace Asakabank.IdentityApi.Models {
    public class UserAuthResponse : BaseResponse {
        /// <summary>
        /// Данные пользователя
        /// </summary>
        [JsonPropertyName("user")]
        public User User { get; set; }
    }
}
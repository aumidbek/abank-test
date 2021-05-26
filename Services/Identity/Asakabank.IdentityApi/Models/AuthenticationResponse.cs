using System;
using System.Text.Json.Serialization;
using Asakabank.Base;

namespace Asakabank.IdentityApi.Models {
    public class AuthenticationResponse : BaseResponse {
        [JsonPropertyName("token")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Token { get; set; }

        [JsonPropertyName("refreshToken")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string RefreshToken { get; set; }

        [JsonPropertyName("expires")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public DateTime Expires { get; set; }
    }
}
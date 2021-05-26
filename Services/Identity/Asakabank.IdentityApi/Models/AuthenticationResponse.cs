using System;
using Asakabank.Base;

namespace Asakabank.IdentityApi.Models {
    public class AuthenticationResponse : BaseResponse {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expires { get; set; }
    }
}
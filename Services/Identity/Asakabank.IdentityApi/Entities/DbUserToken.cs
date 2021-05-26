using System;
using Asakabank.Base;

namespace Asakabank.IdentityApi.Entities {
    public class DbUserToken : BaseEntity {
        public DbUserToken(Guid id) : base(id) {
        }
        
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }

        public DateTime TokenCreatedAt { get; set; }
        public DateTime Expires { get; set; }
    }
}
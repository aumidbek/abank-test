using System;
using System.ComponentModel.DataAnnotations;
using Asakabank.Base;

namespace Asakabank.UserApi.Entities {
    public class DbUser : BaseEntity {
        public DbUser(Guid id) : base(id) {
        }

        [Required]
        public string Username { get; set; }

        public string Password { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Passport { get; set; }

        public string OTP { get; set; }
        public DateTime OTPSentTime { get; set; }
        public bool IsIdentified { get; set; }
    }
}
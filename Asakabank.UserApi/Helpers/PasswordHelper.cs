using System.Runtime.InteropServices.ComTypes;
using Asakabank.UserApi.Entities;
using Microsoft.AspNetCore.Identity;

namespace Asakabank.UserApi.Helpers {
    public static class PasswordHelper {
        public static string HashPassword(DbUser user, string password) {
            //todo: check this
            var ph = new PasswordHasher<DbUser>();
            return ph.HashPassword(user, password);
        }

        public static PasswordVerificationResult VerifyHashedPassword(DbUser user, string password) {
            var ph = new PasswordHasher<DbUser>();
            return ph.VerifyHashedPassword(user, user.Password, password);
        }
    }
}
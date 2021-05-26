using System.ComponentModel;

namespace Asakabank.Base {
    public enum ActionResult {
        [Description("Success")] Success = 0,

        [Description("Object not found")] ObjectNotFound = -101,

        [Description("Invalid Token Passed")] InvalidTokenPassed = -301,
    }
}
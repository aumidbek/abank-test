using Asakabank.IdentityApi.Helpers;
using Asakabank.IdentityApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asakabank.IdentityApi.Controllers {
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class TokenController : ControllerBase {
        private readonly IJwtAuthenticationManager _jWtAuthenticationManager;
        private readonly IJwtRefreshManager _tokenRefresher;

        public TokenController(IJwtAuthenticationManager jWtAuthenticationManager, IJwtRefreshManager tokenRefresher) {
            _jWtAuthenticationManager = jWtAuthenticationManager;
            _tokenRefresher = tokenRefresher;
        }

        // GET: token
        [HttpGet]
        public IActionResult Get() {
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("auth")]
        public IActionResult Authenticate([FromBody] UserCred userCred) {
            var authResult = _jWtAuthenticationManager.Authenticate(userCred.Username, userCred.Password);

            if (authResult == null)
                return Unauthorized();

            return Ok(authResult);
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public IActionResult Refresh([FromBody] RefreshCred refreshCred) {
            var authResult = _tokenRefresher.Refresh(refreshCred);

            if (authResult == null)
                return Unauthorized();

            return Ok(authResult);
        }
    }
}
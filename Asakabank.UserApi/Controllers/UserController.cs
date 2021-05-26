using System.Threading.Tasks;
using Asakabank.UserApi.Core;
using Asakabank.UserApi.ServiceModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Asakabank.UserApi.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IUserService userService) {
            _logger = logger;
            _userService = userService;
        }

        /// <summary>
        /// Регистрация пользователя по номеру телефона
        /// </summary>
        /// <param name="user">Номер телефона</param>
        /// <returns>Ид. пользователя</returns>
        [HttpPost("create")]
        public async Task<IActionResult> Create(UserCreate user) {
            var result = await _userService.Create(user);

            return Ok(result);
        }

        /// <summary>
        /// Подтверждение регистрации
        /// </summary>
        /// <param name="user">Данные для подтверждения</param>
        /// <returns>Код и текст ошибки</returns>
        [HttpPost("confirm")]
        public async Task<IActionResult> Confirm(UserConfirm user) {
            var result = await _userService.Confirm(user);

            return Ok(result);
        }

        /// <summary>
        /// Аутентификация пользователя
        /// </summary>
        /// <param name="user">Данные для аутентификации</param>
        /// <returns>Данные пользователя</returns>
        [HttpPost("auth")]
        public async Task<IActionResult> Auth(UserCred user) {
            var result = await _userService.Auth(user);

            return Ok(result);
        }
    }
}
using System;
using System.Linq;
using System.Threading.Tasks;
using Asakabank.UserApi.Base;
using Asakabank.UserApi.Core;
using Asakabank.UserApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Asakabank.UserApi.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class RegistrationController : ControllerBase {
        private readonly ILogger<RegistrationController> _logger;
        private readonly DataContext _context;
        private readonly IBlogService _blogService;
        public RegistrationController(ILogger<RegistrationController> logger, IBlogService blogService) {
            _logger = logger;
            _blogService = blogService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid id)
        {
            var blog = await _blogService.Get(id);

            return Ok(blog);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Blog blog)
        {
            var blogId = await _blogService.Create(blog);

            return Ok(blogId);
        }

        [HttpPut]
        public async Task<IActionResult> Update(Blog blog)
        {
            await _blogService.Update(blog);

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _blogService.Delete(id);

            return Ok();
        }
    }
}
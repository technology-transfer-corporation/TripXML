using Microsoft.AspNetCore.Mvc;
using VirtualCreditCard.Core.Service;
using VirtualCreditCard.Infrastructure.Service;
using VirtualCreditCard.Models.Dto;

namespace VirtualCreditCard.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        public AuthController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Login([FromBody] UserLoginRqDto user)
        {
            var dbUser = await _userService.FindUserByNameAsync(user.Username);

            if (dbUser == null)
            {
                return NotFound("User not found.");
            }

            var isValid = dbUser.Password == user.Password;

            if (!isValid)
            {
                return BadRequest("Could not authenticate user.");
            }

            var token = _tokenService.BuildToken(user.Username);

            var response = new UserLoginRsDto()
            {
                Token = token
            };

            return Ok(response);
        }
    }
}

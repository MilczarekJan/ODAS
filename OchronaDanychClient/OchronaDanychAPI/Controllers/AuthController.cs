using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using OchronaDanychShared;
using OchronaDanychShared.Auth;
using OchronaDanychAPI.Services.AuthService;

namespace OchronaDanychAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            this._authService = authService;
        }

        [HttpGet("Secret"), Authorize]
        public string SecretText()
        {
            return "secret";
        }

        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login(UserLoginDTO userLoginDTO)
        {
            var response = await  _authService.Login(userLoginDTO.Email, userLoginDTO.Password);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegisterDTO userRegisterDTO)
        {
            var user = new User()
            {
                Email = userRegisterDTO.Email,
                Username = userRegisterDTO.Username,
                Balance = userRegisterDTO.Balance,
                DocumentNumber = userRegisterDTO.DocumentNumber
            };

            var response = await _authService.Register(user, userRegisterDTO.Password);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        
        }

        [HttpPost("change-password"), Authorize]
        public async Task<ActionResult<ServiceResponse<bool>>> ChangePassword([FromBody] string newPassword)
        {
            var userId = User.FindFirstValue(ClaimTypes.Email);
            var response = await _authService.ChangePassword(userId, newPassword);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        /*
        [HttpPut("changebalance")]
        public async Task<ActionResult<ServiceResponse<string>>> ChangeBalance(string amount)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var response = await _authService.ChangeBalance(email, amount);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
        */
    }
}

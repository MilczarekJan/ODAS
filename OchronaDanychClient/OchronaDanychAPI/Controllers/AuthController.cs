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

        [HttpGet("document"), Authorize]
        public async Task<ActionResult<ServiceResponse<string>>> Document(string email)
        {
            var response = await _authService.GetDocumentNumber(email);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("balance"), Authorize]
        public async Task<ActionResult<ServiceResponse<string>>> Balance(string email) 
        {
            var response = await _authService.GetBalance(email);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
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
                Balance = userRegisterDTO.Balance
            };

            var response = await _authService.Register(user, userRegisterDTO.Password, userRegisterDTO.DocumentNumber);
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

        [HttpPost("check-password")]
        public async Task<ActionResult<ServiceResponse<bool>>> CheckPassword(string email, [FromBody] string password)
        {
            var response = await _authService.CheckPassword(email, password);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}

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
        public async Task<ActionResult<ServiceResponse<string>>> Document()
        {
            var userEmailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email); //Teraz pobierane z User.Claims

            if (userEmailClaim == null)
            {
                return Unauthorized();
            }

            var authorizedEmail = userEmailClaim.Value;
            var response = await _authService.GetDocumentNumber(authorizedEmail);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("balance"), Authorize]
        public async Task<ActionResult<ServiceResponse<string>>> Balance() 
        {
            var userEmailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email); //Teraz pobierane z User.Claims

            if (userEmailClaim == null)
            {
                return Unauthorized();
            }

            var authorizedEmail = userEmailClaim.Value;
            var response = await _authService.GetBalance(authorizedEmail);
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
            var userEmailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email); //Teraz pobierane z User.Claims

            if (userEmailClaim == null)
            {
                return Unauthorized();
            }

            var authorizedEmail = userEmailClaim.Value;
            var response = await _authService.ChangePassword(authorizedEmail, newPassword);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("check-password"), Authorize]
        public async Task<ActionResult<ServiceResponse<bool>>> CheckPassword([FromBody] string password)
        {
            var userEmailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email); //Teraz pobierane z User.Claims

            if (userEmailClaim == null)
            {
                return Unauthorized();
            }

            var authorizedEmail = userEmailClaim.Value;
            var response = await _authService.CheckPassword(authorizedEmail, password);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("check-user")]
        public async Task<ActionResult<ServiceResponse<string>>> CheckUser([FromBody] string email) 
        {
            var response = await _authService.CheckUser(email);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

    }
}

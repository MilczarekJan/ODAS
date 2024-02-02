using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using OchronaDanychShared;
using OchronaDanychShared.Models;
using OchronaDanychAPI.Services.TransferService;
using OchronaDanychShared.Auth;
using System.Security.Claims;

namespace OchronaDanychAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class TransferController : Controller
    {
        private readonly ITransferService _transferService; //shoeservice, bêdzie o odpowiedziach dotycz¹cych butów
        private readonly IConfiguration _configuration;
        private readonly ILogger<TransferController> _logger;

        public TransferController(ITransferService transferService, IConfiguration configuration, ILogger<TransferController> logger)
        {
            _transferService = transferService;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet, Authorize]
        public async Task<ActionResult<ServiceResponse<List<BankTransfer>>>> GetBankTransfers()
        {
            var userEmailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email); //Teraz pobierane z User.Claims

            if (userEmailClaim == null)
            {
                return Unauthorized();
            }

            var authorizedEmail = userEmailClaim.Value;
            var result = await _transferService.GetTransfersAsync(authorizedEmail);

            if (result.Success)
                return Ok(result);
            else
                return StatusCode(500, $"Internal server error {result.Message}");
        }


        [HttpPost("createTransfer")]
        public async Task<ActionResult<ServiceResponse<int>>> CreateTransfer([FromBody] BankTransferDTO transfer)
        {
            var userEmailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email); //Tak samo jak powy¿ej

            if (userEmailClaim == null)
            {
                return Unauthorized();
            }

            var authorizedEmail = userEmailClaim.Value;
            if (authorizedEmail != transfer.Sender_Email) {
                return Forbid();
            }
            
            var response = await _transferService.CreateTransfer(transfer);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);

        }
    }
}
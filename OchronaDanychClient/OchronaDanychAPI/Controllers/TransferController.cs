using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using OchronaDanychShared;
using OchronaDanychShared.Models;
using OchronaDanychAPI.Services.TransferService;

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

        [HttpGet, Authorize]//, Authorize
        public async Task<ActionResult<ServiceResponse<List<BankTransfer>>>> GetBankTransfers()
        {

            var result = await _transferService.GetTransfersAsync();

            if (result.Success)
                return Ok(result);
            else
                return StatusCode(500, $"Internal server error {result.Message}");
        }
    }
}
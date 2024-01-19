using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using OchronaDanychShared;
using OchronaDanychShared.Models;

namespace OchronaDanychAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class TransferController : Controller
    {
        private readonly IShoeService _shoeService; //shoeservice, bêdzie o odpowiedziach dotycz¹cych butów
        private readonly IConfiguration _configuration;
        private readonly ILogger<ShoeController> _logger;
        private readonly IValidateShoeService _validateShoeService;

        public TransferController(IShoeService shoeService, IConfiguration configuration, ILogger<ShoeController> logger, IValidateShoeService validateShoeService)
        {
            _shoeService = shoeService;
            _configuration = configuration;
            _logger = logger;
            _validateShoeService = validateShoeService;
        }

        [HttpGet, Authorize]//, Authorize
        public async Task<ActionResult<ServiceResponse<List<BankTransfer>>>> GetBankTransfers()
        {

            var result = await _shoeService.GetTransfersAsync();

            if (result.Success)
                return Ok(result);
            else
                return StatusCode(500, $"Internal server error {result.Message}");
        }
    }
}
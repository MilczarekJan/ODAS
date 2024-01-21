using OchronaDanychShared.Models;
using OchronaDanychShared;

namespace OchronaDanychAPI.Services.TransferService
{
    public interface ITransferService
    {
        Task<ServiceResponse<List<BankTransfer>>> GetTransfersAsync();
    }
}

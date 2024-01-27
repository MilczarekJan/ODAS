using OchronaDanychShared.Models;
using OchronaDanychShared;
using OchronaDanychShared.Auth;

namespace OchronaDanychAPI.Services.TransferService
{
    public interface ITransferService
    {
        Task<ServiceResponse<List<BankTransfer>>> GetTransfersAsync();
        Task<ServiceResponse<string>> CreateTransfer(BankTransfer transfer);
    }
}

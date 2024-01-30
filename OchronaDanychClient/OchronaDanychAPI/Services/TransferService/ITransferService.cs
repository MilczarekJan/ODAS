using OchronaDanychShared.Models;
using OchronaDanychShared;
using OchronaDanychShared.Auth;

namespace OchronaDanychAPI.Services.TransferService
{
    public interface ITransferService
    {
        Task<ServiceResponse<List<BankTransfer>>> GetTransfersAsync(string email);
        Task<ServiceResponse<List<BankTransfer>>> GetTransfersByMailAsync(string email);
        Task<ServiceResponse<string>> CreateTransfer(BankTransferDTO transfer);

    }
}

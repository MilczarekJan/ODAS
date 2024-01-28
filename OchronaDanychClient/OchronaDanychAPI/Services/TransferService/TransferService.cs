using Microsoft.EntityFrameworkCore;
using OchronaDanychAPI.Models;
using OchronaDanychShared;
using OchronaDanychShared.Auth;
using OchronaDanychShared.Models;

namespace OchronaDanychAPI.Services.TransferService
{
    public class TransferService : ITransferService
    {
        private readonly DataContext _dataContext;

        public TransferService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<ServiceResponse<List<BankTransfer>>> GetTransfersAsync()
        {
            var transfers = await _dataContext.BankTransfers.ToListAsync();

            try
            {
                var response = new ServiceResponse<List<BankTransfer>>()
                {
                    Data = transfers,
                    Message = "Ok",
                    Success = true
                };

                return response;
            }
            catch (Exception)
            {
                return new ServiceResponse<List<BankTransfer>>()
                {
                    Data = null,
                    Message = "Problem with database",
                    Success = false
                };
            }

        }

        public async Task<ServiceResponse<List<BankTransfer>>> GetTransfersByMailAsync(string email) {
            var transfers = await _dataContext.BankTransfers
                .Where(t => t.Sender_Email == email)
                .ToListAsync();
            var transfersReceived = await _dataContext.BankTransfers
                .Where(t => t.Recipient_Email == email).ToListAsync();
            var allTransfers = transfers.Concat(transfersReceived).ToList();
            try
            {
                var response = new ServiceResponse<List<BankTransfer>>()
                {
                    Data = allTransfers,
                    Message = "Ok",
                    Success = true
                };

                return response;
            }
            catch (Exception)
            {
                return new ServiceResponse<List<BankTransfer>>()
                {
                    Data = null,
                    Message = "Problem with database",
                    Success = false
                };
            }
        }

        public async Task<ServiceResponse<string>> CreateTransfer(BankTransfer transfer)
        {
            await _dataContext.BankTransfers.AddAsync(transfer);
            await _dataContext.SaveChangesAsync();
            return new ServiceResponse<string> { Success = true, Data = transfer.Title, Message = "Transaction successful!" };
        }
    }
}

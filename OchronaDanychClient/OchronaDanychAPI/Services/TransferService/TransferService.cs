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
        public async Task<ServiceResponse<List<BankTransfer>>> GetTransfersAsync(string email)
        {
            var transfers = await _dataContext.BankTransfers
                .Where(t => t.Sender_Email.ToString() == email)
                .ToListAsync();
            var transfersReceived = await _dataContext.BankTransfers
                .Where(t => t.Recipient_Email.ToString() == email).ToListAsync();
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

        public async Task<ServiceResponse<string>> CreateTransfer(BankTransferDTO transfer)
        {
            var sender = await _dataContext.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == transfer.Sender_Email.ToLower());
            var recipient = await _dataContext.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == transfer.Recipient_Email.ToLower());

            if (sender == null || recipient == null) {
                return new ServiceResponse<string> { Success = false, Data = transfer.Title, Message = "Wrong transaction" };
            }

            if (sender.Balance < transfer.Amount || transfer.Amount <= 0)
            {
                return new ServiceResponse<string> { Success = false, Data = transfer.Title, Message = "Not enough funds" };
            }

            if (transfer.Recipient_Name == "USER") {
                transfer.Recipient_Name = recipient.Username;
            }

            sender.Balance -= transfer.Amount;
            recipient.Balance += transfer.Amount;
            BankTransfer transferToAdd = new BankTransfer(transfer);
            _dataContext.BankTransfers.Add(transferToAdd);
            _dataContext.Users.Update(sender);
            _dataContext.Users.Update(recipient);
            await _dataContext.SaveChangesAsync();
            return new ServiceResponse<string> { Success = true, Data = transfer.Title, Message = "Transaction successful!" };
        }
    }
}

using Ganss.Xss;
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
            email = SanitizeInput(email);
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
            transfer.Title = SanitizeInput(transfer.Title);
            transfer.Sender_Email = SanitizeInput(transfer.Sender_Email);
            transfer.Recipient_Email = SanitizeInput(transfer.Recipient_Email);

            if (transfer.Sender_Email == transfer.Recipient_Email) 
            {
                return new ServiceResponse<string> { Success = false, Data = null, Message = "Wrong recipient email" };
            }

            var sender = await _dataContext.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == transfer.Sender_Email.ToLower());
            var recipient = await _dataContext.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == transfer.Recipient_Email.ToLower());

            if (sender == null || recipient == null) {
                return new ServiceResponse<string> { Success = false, Data = null, Message = "Wrong transaction" };
            }

            if (sender.Balance < transfer.Amount || transfer.Amount <= 0)
            {
                return new ServiceResponse<string> { Success = false, Data = null, Message = "Not enough funds" };
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
            return new ServiceResponse<string> { Success = true, Data = null, Message = "Transaction successful!" };
        }
        private string SanitizeInput(string input)
        {
            HtmlSanitizerOptions options = new HtmlSanitizerOptions();
            options.AllowedTags = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { };
            options.AllowedAttributes = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { };
            options.AllowedCssProperties = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { };
            HtmlSanitizer sanitizer = new HtmlSanitizer(options);
            input = sanitizer.Sanitize(input);
            return input;
        }
    }
}

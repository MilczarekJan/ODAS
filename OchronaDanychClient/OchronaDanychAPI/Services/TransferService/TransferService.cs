using Microsoft.EntityFrameworkCore;
using OchronaDanychAPI.Models;
using OchronaDanychShared;
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
    }
}

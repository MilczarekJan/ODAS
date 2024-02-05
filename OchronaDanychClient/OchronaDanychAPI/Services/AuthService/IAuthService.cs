using OchronaDanychShared;
using OchronaDanychShared.Auth;

namespace OchronaDanychAPI.Services.AuthService
{
    public interface IAuthService
    {
        Task<ServiceResponse<string>> Login(string email, string password);

        Task<ServiceResponse<string>> Register(User user, string password, string documentNumber);

        Task<bool> UserExists(string email);

        Task<ServiceResponse<bool>> ChangePassword(string userMail, string newPassword);

        Task<ServiceResponse<string>> GetDocumentNumber(string email);

        Task<ServiceResponse<string>> GetBalance(string email);

        Task<ServiceResponse<bool>> CheckPassword(string email, string password);

        Task<ServiceResponse<string>> CheckUser(string email);
    }
}

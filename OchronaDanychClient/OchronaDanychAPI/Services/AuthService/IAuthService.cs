using OchronaDanychShared;
using OchronaDanychShared.Auth;

namespace OchronaDanychAPI.Services.AuthService
{
    public interface IAuthService
    {
        Task<ServiceResponse<string>> Login(string email, PasswordPair[] password);

        Task<ServiceResponse<string>> Register(User user, string password, string documentNumber);

        Task<bool> UserExists(string email);

        Task<ServiceResponse<bool>> ChangePassword(string userMail, string newPassword);

        //Task<ServiceResponse<bool>> ChangeBalance(string email, string amount);
    }
}

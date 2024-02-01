using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OchronaDanychShared.Auth;

namespace OchronaDanychShared.Services
{
    public interface IAuthService
    {
        Task<ServiceResponse<string>> Login(UserLoginDTO userLoginDto);

        Task<ServiceResponse<int>> Register(UserRegisterDTO userRegisterDTO);

        Task<bool> ChangePassword(string token, string newPassword);

        Task<bool> CheckPassword(string token, string email, string password);

	}
}

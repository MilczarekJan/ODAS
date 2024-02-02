using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using OchronaDanychShared.Auth;

namespace OchronaDanychShared.Services
{
    public class AuthService : IAuthService
    {
    
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;

        }

        public async Task<ServiceResponse<string>> Login(UserLoginDTO userLoginDto)
        {
            var result = await _httpClient.PostAsJsonAsync("https://localhost:7230/api/Auth/login", userLoginDto);

            var data =  await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();

            return data;
        }

        public async Task<ServiceResponse<int>> Register(UserRegisterDTO userRegisterDTO)
        {
            var result = await _httpClient.PostAsJsonAsync("https://p05shopapiwindows.azurewebsites.net/api/auth/register/", userRegisterDTO);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<int>>();
        }

        public async Task<bool> ChangePassword(string token, string newPassword)
        {
            if (!_httpClient.DefaultRequestHeaders.Contains("Authorization"))
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token));
            }
            var result = await _httpClient.PostAsJsonAsync("https://localhost:7230/api/Auth/change-password", newPassword);
			var jsonResponse = await result.Content.ReadAsStringAsync();
			var responseObj = JsonConvert.DeserializeObject<JObject>(jsonResponse);
			bool success = responseObj.Value<bool>("success");
            if (success)
            {
                return true;
            }
            else return false;
        }

        public async Task<bool> CheckPassword(string token, string password)
        {
            var uri = "https://localhost:7230/api/Auth/check-password";
            if (!_httpClient.DefaultRequestHeaders.Contains("Authorization"))
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token));
            }
            var result = await _httpClient.PostAsJsonAsync(uri, password);
			var jsonResponse = await result.Content.ReadAsStringAsync();
			var responseObj = JsonConvert.DeserializeObject<JObject>(jsonResponse);
			bool success = responseObj.Value<bool>("success");
			if (success)
			{
				return true;
			}
			else return false;
		}
    }
}

using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http;
using OchronaDanychShared.Models;
using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;

namespace OchronaDanychShared.Services
{
    public class AddTransferService
    {
        private const string base_url = "https://localhost:7230/api/Transfer/createTransfer";
		//https://localhost:7230/api/Transfer/createTransfer
		private readonly HttpClient _httpClient;

        public AddTransferService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<bool> AddTransferFromApi(string token, BankTransferDTO transfer)
        {
            var uri = base_url;
            if (!_httpClient.DefaultRequestHeaders.Contains("Authorization"))
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token));
            }
            var response = await _httpClient.PostAsJsonAsync(uri, transfer);
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();

            var responseObj = JsonConvert.DeserializeObject<JObject>(jsonResponse);
            bool success = responseObj.Value<bool>("success");
            if (success) //&& responseObj.Value<string>("data") != null
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

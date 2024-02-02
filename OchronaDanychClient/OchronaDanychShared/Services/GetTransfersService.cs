using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http;
using OchronaDanychShared.Models;

namespace OchronaDanychShared.Services
{
    public class GetTransfersService
    {
        private const string base_url = "https://localhost:7230/api/Transfer";
        //https://localhost:7230/api/Transfer?email=milczarekjanek%40gmail.com
        private readonly HttpClient _httpClient;

        public GetTransfersService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<BankTransfer>> GetTransfersFromApi(string token)
        {
            var uri = base_url;
            if (!_httpClient.DefaultRequestHeaders.Contains("Authorization"))
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token));
            }
            var response = await _httpClient.GetAsync(uri); //"https://localhost:7230/api/Transfer"
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var responseObj = JsonConvert.DeserializeObject<JObject>(jsonResponse);
            bool success = responseObj.Value<bool>("success");
            if (success) //&& responseObj.Value<string>("data") != null
            {
                var balanceData = responseObj["data"].ToObject<List<BankTransfer>>();
                return balanceData;
            }
            else
            {
                return null;
            }

        }
    }
}

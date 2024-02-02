using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using OchronaDanychShared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OchronaDanychShared.Services
{
    public class GetBalanceService
    {
        private const string base_url = "https://localhost:7230/api/Auth/balance";
        private readonly HttpClient _httpClient;

        public GetBalanceService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> GetBalanceFromApi(string token)
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
                var balanceData = responseObj["data"].ToObject<string>();
                return balanceData;
            }
            else
            {
                return null;
            }

        }
    }
}

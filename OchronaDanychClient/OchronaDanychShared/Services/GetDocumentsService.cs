using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OchronaDanychShared.Services
{
    public class GetDocumentsService
    {
        private const string base_url = "https://localhost:7230/api/Auth/document";
        //https://localhost:7230/api/Transfer?email=milczarekjanek%40gmail.com
        private readonly HttpClient _httpClient;

        public GetDocumentsService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> GetDocumentsFromApi(string token)
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

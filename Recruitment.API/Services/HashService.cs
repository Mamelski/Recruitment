using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Recruitment.API.Options;
using Recruitment.Contracts;

namespace Recruitment.API.Services
{
    public class HashService : IHashService
    {
        private readonly HttpClient _httpClient;
        private readonly IHashCalculatorOptions _hashCalculatorOptions;

        public HashService(HttpClient httpClient, IHashCalculatorOptions hashCalculatorOptions)
        {
            _httpClient = httpClient;
            _hashCalculatorOptions = hashCalculatorOptions;
        }
        
        public async Task<string> GetHash(Credentials credentials)
        {
            var jsonCredentials = JsonConvert.SerializeObject(credentials);
            var data = new StringContent(jsonCredentials, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_hashCalculatorOptions.CalculateHashUri, data);

            response.EnsureSuccessStatusCode();
            
            var result = response.Content.ReadAsStringAsync().Result;
            return result;
        }
    }
}
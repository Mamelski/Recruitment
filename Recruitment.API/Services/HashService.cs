using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Recruitment.API.Options;
using Recruitment.Contracts;

namespace Recruitment.API.Services
{
    public class HashService : IHashService
    {
        private HttpClient _httpClient;

        public HashService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public Task<string> GetHash(Credentials credentials)
        {
            throw new System.NotImplementedException();
        }
    }
}
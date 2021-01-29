using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Recruitment.API.Options;
using Recruitment.Contracts;

namespace Recruitment.API.Services
{
    public class HashService : IHashService
    {
        private readonly IHashCalculatorOptions _options;

        public HashService(IHashCalculatorOptions options)
        {
            _options = options;
        }
        public Task<string> GetHash(Credentials credentials)
        {
            throw new System.NotImplementedException();
        }
    }
}
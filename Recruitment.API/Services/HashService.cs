using System.Threading.Tasks;
using Recruitment.Contracts;

namespace Recruitment.API.Services
{
    public class HashService : IHashService
    {
        public Task<string> GetHash(Credentials credentials)
        {
            throw new System.NotImplementedException();
        }
    }
}
using System.Threading.Tasks;
using Recruitment.Contracts;

namespace Recruitment.API.Services
{
    public interface IHashService
    {
        Task<string> GetHash(Credentials credentials);
    }
}
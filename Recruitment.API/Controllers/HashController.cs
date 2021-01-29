using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Recruitment.API.Services;
using Recruitment.Contracts;

namespace Recruitment.API.Controllers
{
    [ApiController]
    [Route("api/")]
    public class HashController : ControllerBase
    {
        private readonly ILogger<HashController> _logger;
        private readonly IHashService _hashService;

        public HashController(ILogger<HashController> logger, IHashService hashService)
        {
            _logger = logger;
            _hashService = hashService;
        }

        [HttpPost("hash")]
        public async Task<IActionResult> GetHash([FromBody] Credentials credentials)
        {
            _logger.LogInformation("Hash endpoint called with user login {Login}", credentials.Login);
            
            var hash = await _hashService.GetHash(credentials);
            
            return Ok(hash);
        }
    }
}
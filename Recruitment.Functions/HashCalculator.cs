using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Recruitment.Contracts;

namespace Recruitment.Functions
{
    public static class HashCalculator
    {
        [FunctionName("HashCalculator")]
        public static async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post")]
            HttpRequest request, ILogger log)
        {
            var credentials = await ParseInputCredentials(request);

            if (!ValidateCredentials(credentials))
            {
                // 422 - may be something different
                return new UnprocessableEntityObjectResult("Login and password in body cannot be null or whitespace.");
            }
            
            var input = credentials.Login + credentials.Password;
            var hash = CalculateMd5Hash(input);

            return new OkObjectResult(hash);
        }

        private static async Task<Credentials> ParseInputCredentials(HttpRequest request)
        {
            var content = await new StreamReader(request.Body).ReadToEndAsync();
            var credentials = JsonConvert.DeserializeObject<Credentials>(content);

            return credentials;
        }
        
        // Here instead of returning bool maybe we can use exceptions and then filters
        // with some logic common for other functions
        private static bool ValidateCredentials(Credentials credentials)
        {
            return !string.IsNullOrWhiteSpace(credentials?.Login) 
                   && !string.IsNullOrWhiteSpace(credentials.Password);
        }

        private static string CalculateMd5Hash(string input)
        {
            using var md5 = MD5.Create();
            var inputBytes = Encoding.ASCII.GetBytes(input);
            var hashBytes = md5.ComputeHash(inputBytes);
                
            var sb = new StringBuilder();
            foreach (var t in hashBytes)
            {
                sb.Append(t.ToString("X2"));
            }

            return sb.ToString();
        }
    }
}
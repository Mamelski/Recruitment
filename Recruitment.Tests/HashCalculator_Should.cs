using System.IO;
using System.Text;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Recruitment.Contracts;
using Recruitment.Functions;
using Shouldly;
using Xunit;

namespace Recruitment.Tests
{
    public class HashCalculator_Should
    {
        [Fact]
        public async Task Return_unprocessableEntityObjectResult_when_credentials_are_null()
        {
            var request = PrepareRequestWithCredentials(null);

            var result = await HashCalculator.CalculateHash(request) as UnprocessableEntityObjectResult;

            result.ShouldNotBeNull();
        }
        
        [Theory]
        [InlineAutoData("aaaaaa", null)]
        [InlineAutoData("aaaaaa", "")]
        [InlineAutoData("aaaaaa", " ")]
        [InlineAutoData("aaaaaa", "     ")]
        [InlineAutoData(null, "aaaaaa")]
        [InlineAutoData("", "aaaaaa")]
        [InlineAutoData(" ","aaaaaa")]
        [InlineAutoData("     ","aaaaaa")]
        public async Task Return_unprocessableEntityObjectResult_when_login_or_password_is_null_or_whitespace(
            string login,
            string password)
        {
            var credentials = new Credentials {Login = login, Password = password};
            var request = PrepareRequestWithCredentials(credentials);

            var result = await HashCalculator.CalculateHash(request) as UnprocessableEntityObjectResult;

            result.ShouldNotBeNull();
        }
        
        [Theory, AutoData]
        public async Task Return_hash_for_correct_credentials(
            Credentials credentials)
        {
            var request = PrepareRequestWithCredentials(credentials);

            var result = await HashCalculator.CalculateHash(request) as OkObjectResult;

            // We could check hash length and things like that but I don't know the exact requirements
            result.ShouldNotBeNull();
            result.Value.ShouldNotBeNull();
        }
        
        [Theory, AutoData]
        public async Task Return_the_same_hash_for_the_same_credentials(
            Credentials credentials)
        {
            var request1 = PrepareRequestWithCredentials(credentials);
            var request2 = PrepareRequestWithCredentials(credentials);

            var result1 = await HashCalculator.CalculateHash(request1) as OkObjectResult;
            var result2 = await HashCalculator.CalculateHash(request2) as OkObjectResult;

            result1.ShouldNotBeNull();
            result2.ShouldNotBeNull();

            result1.Value.ShouldNotBeNull();
            result2.Value.ShouldNotBeNull();
            
            result1.Value.ShouldBe(result2.Value);
        }

        private HttpRequest PrepareRequestWithCredentials(Credentials credentials)
        {
            var jsonCredentials = JsonConvert.SerializeObject(credentials);
            
            var context = new DefaultHttpContext();
            var request = context.Request;
            request.Body = new MemoryStream(Encoding.UTF8.GetBytes(jsonCredentials));

            return request;
        }
    }
}
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Recruitment.API.Controllers;
using Recruitment.API.Services;
using Recruitment.Contracts;
using Shouldly;
using Xunit;

namespace Recruitment.Tests
{
    public class HashController_Should
    {
        [Theory, AutoData]
        public async Task Call_Hash_service_with_correct_credentials(
            Mock<ILogger<HashController>> logger,
            Mock<IHashService> hashServiceMock,
            Credentials credentials)
        {
            var sut = new HashController(logger.Object, hashServiceMock.Object);

            await sut.GetHash(credentials);
            
            hashServiceMock.Verify(hashService => hashService.GetHash(credentials), Times.Once);
        }

        [Theory, AutoData]
        public async Task Return_hash_for_correct_credentials(
            Mock<ILogger<HashController>> logger,
            Mock<IHashService> hashServiceMock,
            Credentials credentials,
            string hash)
        {
            hashServiceMock.Setup(hashService => hashService.GetHash(credentials))
                .ReturnsAsync(hash);
            
            var sut = new HashController(logger.Object, hashServiceMock.Object);

            var response = await sut.GetHash(credentials) as OkObjectResult;
            
            response.ShouldNotBeNull();
        }
    }
}
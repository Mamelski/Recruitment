using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Newtonsoft.Json;
using Recruitment.API.Options;
using Recruitment.API.Services;
using Recruitment.Contracts;
using RichardSzalay.MockHttp;
using Shouldly;
using Xunit;

namespace Recruitment.Tests
{
    public class HashService_Should
    {
        private static readonly HashCalculatorOptions Options = new HashCalculatorOptions
        {
            BaseUrl = "http://localhost:7071",
            CalculateHashUri = "api/HashCalculator"
        };

        private readonly string _calculateHashUri = Options.BaseUrl + "/" + Options.CalculateHashUri;
        
        [Theory, AutoData]
        public async Task Call_http_client_with_correct_request(
            Credentials credentials,
            MockHttpMessageHandler mockHttp,
            string hash)
        {
            var request = mockHttp.When(HttpMethod.Post,_calculateHashUri)
                .WithContent(JsonConvert.SerializeObject(credentials))
                .Respond("application/json", hash);

            var client = new HttpClient(mockHttp) {BaseAddress = new Uri(Options.BaseUrl)};
            var sut = new HashService(client, Options);

            await sut.GetHash(credentials);
            
            mockHttp.GetMatchCount(request).ShouldBe(1);
        }
        
        [Theory, AutoData]
        public async Task Return_hash_for_correct_credentials(
            Credentials credentials,
            MockHttpMessageHandler mockHttp,
            string hash)
        {
            mockHttp.When(HttpMethod.Post,_calculateHashUri)
                .WithContent(JsonConvert.SerializeObject(credentials))
                .Respond("application/json", hash);

            var client = new HttpClient(mockHttp) {BaseAddress = new Uri(Options.BaseUrl)};
            var sut = new HashService(client, Options);

            var result = await sut.GetHash(credentials);
            
            result.ShouldBe(hash);
        }
        
        [Theory, AutoData]
        public void Throw_error_when_client_response_status_code_is_not_200(
            Credentials credentials,
            MockHttpMessageHandler mockHttp)
        {
            mockHttp.When(HttpMethod.Post, _calculateHashUri)
                .WithContent(JsonConvert.SerializeObject(credentials))
                .Respond(HttpStatusCode.InternalServerError);

            var client = new HttpClient(mockHttp) {BaseAddress = new Uri(Options.BaseUrl)};
            var sut = new HashService(client, Options);

            Should.Throw<HttpRequestException>(async () => await sut.GetHash(credentials));
        }
    }
}